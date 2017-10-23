using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAIStats
{
    public GameObject enemyGO;
    public Sprite enemySprite;

    public float angRotSpeed;
    public float angToPingPong;
    public float angRotAcceleration;
    public float initialRot;
    public bool TargetsPlayer;

    public float startTime = 0f; //rmb chnage this when adding enemies

    public List<Danmaku> danmakuList;
}

[System.Serializable]
public class Danmaku
{
    public List<BulletMovement> bulletMovementList;
}

[System.Serializable]
public class BulletMovement
{
    public Sprite bulletSprite;

    public List<GameObject> bulletGOList;

    public float radiusFromCenter;
    public int bulletsPerSet;
    public float degreeSeperationBetweenBullets;

    public float bulletInitialSpeed;
    public float bulletInitialRot;
    public int bulletInitialRotMode; //can use to target player

    public float bulletAcceleration;
    public float bulletAngRotSpeed;

    public float verticalAddOnVectorCenter;
    public float verticalAddOnVectorRange;

    public float horizontalAddOnVectorCenter;
    public float horizontalAddOnVectorRange;

    public float addOnsPingPongSpeed;
    public float relativeHorizontalAddOnPhase;

    public float fireStartDelay;

    public float bulletInitialSpeedWeightageDiff;
    public float bulletAngSpeedWeightageDiff;
    public float bulletAccWeightageDiff;
    public int bulletWeightageMode;

    public List<float> aimList;
    public float aimRotSpeed;

    public float firePeriod;

    public float timeOfLastShot = 0f;
    public bool FireStartDelayEnd;

    public float durationOfBulletMovement;
}


public class LevelManager : VectorStuff
{
    public List<EnemyAIStats> enemyAIStatsList;

    public GameObject bulletGO;

    private List<GameObject> allBulletsGOList;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < enemyAIStatsList.Count; i++)
        {
            enemyAIStatsList[i].enemyGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, enemyAIStatsList[i].initialRot));
        }
    }

    void Update()
    {
        for (int i = 0; i < enemyAIStatsList.Count; i++)
        {
            EnemyAIStats _enemyAIStats = enemyAIStatsList[i];

            for (int d = 0; d < _enemyAIStats.danmakuList.Count; d++)
            {
                Danmaku _danmaku = _enemyAIStats.danmakuList[d];

                for (int n = 0; n < _danmaku.bulletMovementList.Count; n++)
                {
                    BulletMovement _bulletMovement = _danmaku.bulletMovementList[n];

                    //=====StartDelay Start=====
                    if (!_bulletMovement.FireStartDelayEnd && Time.time - _bulletMovement.timeOfLastShot >= _bulletMovement.fireStartDelay)//start delay isnt working/implemented for bullets spawning bullets. Figure it out.
                    {
                        _bulletMovement.FireStartDelayEnd = true;
                        _bulletMovement.timeOfLastShot = Time.time;
                    }
                    //=====StartDelay End=====
                    /*
                    if (_danmaku.bulletMovementList.Count == 1)//No point to continue if only 1 pattern
                    {
                        return;
                    }
                    //=====Looping bullets' Bullet Movements Start=====
                    for (int b = 0; b < _bulletMovement.bulletGOList.Count; b++)
                    {
                        if (_bulletMovement.bulletGOList[b].GetComponent<BulletAI>().bulletMovementPatternStartTime + _bulletMovement.durationOfBulletMovement <= Time.time)
                        {
                            Debug.Log("bullet Start time = " + _bulletMovement.bulletGOList[b].GetComponent<BulletAI>().bulletMovementPatternStartTime + "current Time = " + Time.time);
                            _bulletMovement.bulletGOList[b].GetComponent<BulletAI>().bulletMovementPatternStartTime = Time.time;
                            if (n + 1 < _danmaku.bulletMovementList.Count)
                            {
                                _danmaku.bulletMovementList[n + 1].bulletGOList.Add(_bulletMovement.bulletGOList[b]);
                                _bulletMovement.bulletGOList[b].GetComponent<BulletAI>().SetUpAfterChangingBulletPatterns(_danmaku.bulletMovementList[n + 1].bulletInitialSpeed);
                                _bulletMovement.bulletGOList.RemoveAt(b);
                            }
                            else
                            {
                                _danmaku.bulletMovementList[0].bulletGOList.Add(_bulletMovement.bulletGOList[b]);
                                _bulletMovement.bulletGOList[b].GetComponent<BulletAI>().SetUpAfterChangingBulletPatterns(_danmaku.bulletMovementList[0].bulletInitialSpeed);
                                _bulletMovement.bulletGOList.RemoveAt(b);
                            }
                            b--;
                        }
                    }
                    //=====Looping bullets' Bullet Movements End=====
                    */
                }
            }
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < enemyAIStatsList.Count; i++)
        {
            EnemyAIStats _enemyAIStats = enemyAIStatsList[i];

            //=====Spawner Rotation Start=====

            if (_enemyAIStats.angToPingPong > 0)
            {
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _enemyAIStats.angToPingPong * Mathf.Sin((Time.timeSinceLevelLoad - _enemyAIStats.startTime) * _enemyAIStats.angRotSpeed * 0.03f)));
            }
            if (_enemyAIStats.TargetsPlayer)
            {
                if (_enemyAIStats.angToPingPong <= 0)
                {
                    this.transform.rotation = Quaternion.Euler(Vector3.zero);
                }
                this.transform.rotation *= Quaternion.Euler(new Vector3(0, 0, -Mathf.Atan2(playerTransform.position.x - this.transform.position.x, playerTransform.position.y - this.transform.position.y) * Mathf.Rad2Deg));
            }
            else if (_enemyAIStats.angToPingPong <= 0)
            {
                _enemyAIStats.angRotSpeed += _enemyAIStats.angRotAcceleration * Time.deltaTime;
                this.transform.rotation *= Quaternion.Euler(Time.deltaTime * new Vector3(0, 0, _enemyAIStats.angRotSpeed));
            }

            this.transform.rotation *= Quaternion.Euler(new Vector3(0, 0, _enemyAIStats.initialRot));
            //=====Spawner Rotation End=====


            for (int d = 0; d < _enemyAIStats.danmakuList.Count; d++)
            {
                Danmaku _danmaku = _enemyAIStats.danmakuList[d];

                for (int n = 0; n < _danmaku.bulletMovementList.Count; n++)
                {
                    BulletMovement _bulletMovement = _danmaku.bulletMovementList[n];
                    //=====Instatiating new bullets from enemies ONLY start=====
                    if (n == 0 && _bulletMovement.FireStartDelayEnd && Time.time - _bulletMovement.timeOfLastShot >= _bulletMovement.firePeriod)
                    {
                        SpawnBulletSet(bulletGO, _bulletMovement, _enemyAIStats.enemyGO);
                        _bulletMovement.timeOfLastShot = Time.time;
                    }
                    //=====Instatiating new bullets from enemies ONLY end=====
                    //=====Movement of bullets start=====
                    for (int b = 0; b < _bulletMovement.bulletGOList.Count; b++)
                    {
                        BulletAI _bulletAI = _bulletMovement.bulletGOList[b].GetComponent<BulletAI>();
                        if (_bulletMovement.bulletAcceleration * _bulletAI.accelWeightage < 0 && Vector3.SqrMagnitude(_bulletAI.velocity) < 0.01f) //to allow for bullets to deccelerate & move backwards in case of negative
                        {
                            _bulletAI.accelWeightage = -_bulletAI.accelWeightage;
                            _bulletAI.velocity = -_bulletAI.velocity;
                        }
                        _bulletAI.velocity = AlterVector(_bulletAI.velocity, _bulletMovement.bulletAngRotSpeed * _bulletAI.rotWeightage * Time.deltaTime, _bulletMovement.bulletAcceleration * _bulletAI.accelWeightage * Time.deltaTime);//need to put this in another function so can work with multi pattern?

                        Vector3 horizontalAddOnV3_cal = _bulletAI.transform.right * (_bulletMovement.horizontalAddOnVectorCenter + Mathf.Sin((Time.timeSinceLevelLoad - _bulletAI.bulletMovementPatternStartTime) * _bulletMovement.addOnsPingPongSpeed * _bulletMovement.relativeHorizontalAddOnPhase) * _bulletMovement.horizontalAddOnVectorRange);
                        Vector3 verticalAddOnV3_cal = _bulletAI.transform.up * (_bulletMovement.verticalAddOnVectorCenter + Mathf.Cos((Time.timeSinceLevelLoad - _bulletAI.bulletMovementPatternStartTime) * _bulletMovement.addOnsPingPongSpeed) * _bulletMovement.verticalAddOnVectorRange);

                        Vector3 _netAddOnV3 = horizontalAddOnV3_cal + verticalAddOnV3_cal;

                        _bulletMovement.bulletGOList[b].transform.position += (_bulletAI.velocity + _netAddOnV3) * Time.deltaTime;
                        //=====Movement of bullets end=====
                        //=====Spawning of bullets start=====
                        if (n+1 != _danmaku.bulletMovementList.Count && _danmaku.bulletMovementList[n+1].bulletsPerSet > 0 && _danmaku.bulletMovementList[n + 1].aimList.Count > 0)
                        {
                            if (Time.time >= _bulletAI.timeOfLastShot + _danmaku.bulletMovementList[n + 1].firePeriod)
                            {
                                _bulletAI.timeOfLastShot = Time.time;
                                SpawnBulletSet(bulletGO, _danmaku.bulletMovementList[n+1], _bulletMovement.bulletGOList[b]);
                            }
                            
                        }
                        //=====Spawning of bullets end=====
                    }
                    //=====Looping bullets' Bullet Movements Start===== 
                    for (int b = 0; b < _bulletMovement.bulletGOList.Count; b++)//This must be seperated from above loop. else switching screw stuff in above loop
                    {
                        if (_bulletMovement.bulletGOList[b].GetComponent<BulletAI>().bulletMovementPatternStartTime + _bulletMovement.durationOfBulletMovement <= Time.time)
                        {
                            if (n + 1 < _danmaku.bulletMovementList.Count && (_danmaku.bulletMovementList[n+1].bulletsPerSet == 0 || _danmaku.bulletMovementList[n + 1].aimList.Count == 0))
                            {
                                _danmaku.bulletMovementList[n + 1].bulletGOList.Add(_bulletMovement.bulletGOList[b]);
                                SetUpAfterChangingBulletPatterns(_bulletMovement.bulletGOList[b], _danmaku.bulletMovementList[n + 1].bulletInitialSpeed, _danmaku.bulletMovementList[n + 1].bulletInitialRot, _danmaku.bulletMovementList[n + 1].bulletInitialRotMode);
                                _bulletMovement.bulletGOList.RemoveAt(b);
                            }
                            else
                            {
                             
                                BulletMovement bulletMovementToAssignBulletTo;

                                for (int q = n - 1; true ; q--)
                                {
                                    if (q < 0)
                                    {
                                        Debug.Log("This shouldn't happen unless values were changed in editor midgame");
                                        bulletMovementToAssignBulletTo = _danmaku.bulletMovementList[0];
                                        break;
                                    }
                                    if (_danmaku.bulletMovementList[q].bulletsPerSet > 0 && _danmaku.bulletMovementList[q].aimList.Count > 0)
                                    {
                                        bulletMovementToAssignBulletTo = _danmaku.bulletMovementList[q];
                                        break;
                                    }
                                }
                                bulletMovementToAssignBulletTo.bulletGOList.Add(_bulletMovement.bulletGOList[b]);
                                SetUpAfterChangingBulletPatterns(_bulletMovement.bulletGOList[b], bulletMovementToAssignBulletTo.bulletInitialSpeed, bulletMovementToAssignBulletTo.bulletInitialRot, bulletMovementToAssignBulletTo.bulletInitialRotMode);
                                _bulletMovement.bulletGOList.RemoveAt(b);
                            }
                            b--;
                        }
                        
                    }
                    //=====Looping bullets' Bullet Movements End=====
                }
            }
        }
    }

    void SpawnBulletSet(GameObject _bulletGO, BulletMovement _bulletMovement, GameObject _BulletSpawnGO)//bulletRotType:   0 = local rot | 1 = world rot | 2 = targeted rot 
    {//Make _BulletSpawnGO into a list so bullets can spawn bullets
        List<Vector3> bulletSpawnLotList = new List<Vector3>();
        List<float> bulletSpawnRotList = new List<float>();

        //=====Setting bullet spawn location start=====
        for (int i = 0; i < _bulletMovement.aimList.Count; i++)
        {
            Vector3 spawnLot_cal = new Vector3(0, _bulletMovement.radiusFromCenter, 0);
            spawnLot_cal = AlterVector(spawnLot_cal, _bulletMovement.aimList[i] + (_BulletSpawnGO.transform.rotation.eulerAngles.z), 0);//Change to center vector
            spawnLot_cal = AlterVector(spawnLot_cal, -_bulletMovement.degreeSeperationBetweenBullets * (_bulletMovement.bulletsPerSet - 1) * 0.5f, 0);//changes to most side vector

            for (int n = 0; n < _bulletMovement.bulletsPerSet; n++)//Sweeps across end to end and adds the vectors to bulletSpawnLotList then move vector by spawnLot
            {
                bulletSpawnLotList.Add(spawnLot_cal + _BulletSpawnGO.transform.position);
                spawnLot_cal = AlterVector(spawnLot_cal, _bulletMovement.degreeSeperationBetweenBullets, 0f);
            }
        }
        //=====Setting bullet spawn location end=====
        //=====Setting bullet spawn rotation start=====
        if (_bulletMovement.bulletInitialRotMode == 0) // 0 = local rot
        {
            for (int i = 0; i < bulletSpawnLotList.Count; i++)
            {
                bulletSpawnRotList.Add(90 + _bulletMovement.bulletInitialRot + Mathf.Atan2(_BulletSpawnGO.transform.position.y - bulletSpawnLotList[i].y, _BulletSpawnGO.transform.position.x - bulletSpawnLotList[i].x) * Mathf.Rad2Deg);//currently rotates so bullet y axis points away from boss
            }
        }
        else if (_bulletMovement.bulletInitialRotMode == 1) // 1 = world rot
        {
            for (int i = 0; i < bulletSpawnLotList.Count; i++)
            {
                bulletSpawnRotList.Add(_bulletMovement.bulletInitialRot);
            }
        }
        else if (_bulletMovement.bulletInitialRotMode == 2) // 2 = targeted rot
        {
            for (int i = 0; i < bulletSpawnLotList.Count; i++)
            {
                bulletSpawnRotList.Add(-Mathf.Atan2(playerTransform.position.x - bulletSpawnLotList[i].x, playerTransform.position.y - bulletSpawnLotList[i].y) * Mathf.Rad2Deg + _bulletMovement.bulletInitialRot);
            }
        }
        else
        {
            Debug.Log("bulletRotType invalid value");
        }
        //=====Setting bullet spawn rotation end=====
        //=====Instatiating bullets start=====
        for (int i = 0, n = 0; i < bulletSpawnLotList.Count; i++, n++)//ADD OBJECT POOLING
        {
            GameObject instantiatedbullet = Instantiate(_bulletGO, bulletSpawnLotList[i], Quaternion.Euler(new Vector3(0, 0, bulletSpawnRotList[i])));
            BulletAI _bulletAI = instantiatedbullet.GetComponent<BulletAI>();

            if (n >= _bulletMovement.bulletsPerSet)
            {
                n = 0;
            }
            if (_bulletMovement.bulletWeightageMode == 0)
            {
                _bulletAI.rotWeightage = 1;
                _bulletAI.accelWeightage = 1;
                _bulletAI.initialSpeedWeightage = 1;
            }
            else if (_bulletMovement.bulletWeightageMode == 1) // shape like > or <
            {
                _bulletAI.rotWeightage = 1 - _bulletMovement.bulletAngSpeedWeightageDiff * Mathf.Abs(n - 0.5f * (_bulletMovement.bulletsPerSet - 1));
                _bulletAI.accelWeightage = 1 - _bulletMovement.bulletAccWeightageDiff * Mathf.Abs(n - 0.5f * (_bulletMovement.bulletsPerSet - 1));
                _bulletAI.initialSpeedWeightage = 1 - _bulletMovement.bulletInitialSpeedWeightageDiff * Mathf.Abs(n - 0.5f * (_bulletMovement.bulletsPerSet - 1));
            }
            else if (_bulletMovement.bulletWeightageMode == 2) // shape like / or \
            {
                _bulletAI.rotWeightage = 1 - (n * _bulletMovement.bulletAngSpeedWeightageDiff);
                _bulletAI.accelWeightage = 1 - (n * _bulletMovement.bulletAccWeightageDiff);
                _bulletAI.initialSpeedWeightage = 1 - (n * _bulletMovement.bulletInitialSpeedWeightageDiff);
            }

            SetUpAfterChangingBulletPatterns(instantiatedbullet, _bulletMovement.bulletInitialSpeed, 0f, 0);
            _bulletMovement.bulletGOList.Add(instantiatedbullet);
        }
        //=====Instatiating bullets end=====
    }

    public void SetUpAfterChangingBulletPatterns(GameObject _bulletGO, float _initialSpeed, float _initialRot, int _rotMode)
    {
        BulletAI _bulletAI = _bulletGO.GetComponent<BulletAI>();

        _bulletAI.bulletMovementPatternStartTime = Time.time;
        _bulletAI.timeOfLastShot = Time.time;

        if (_rotMode == 0) // 0 = local rot
        {
            _bulletGO.transform.rotation *= Quaternion.Euler(new Vector3(0,0,_initialRot));//adds rotation
        }
        else if (_rotMode == 1) // 1 = world rot
        {
            _bulletGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _initialRot));

        }
        else if (_rotMode == 2) // 2 = targeted rot
        {
            _bulletGO.transform.rotation = Quaternion.Euler(new Vector3(0,0,-Mathf.Atan2(playerTransform.position.x - _bulletGO.transform.position.x, playerTransform.position.y - _bulletGO.transform.position.y) * Mathf.Rad2Deg + _initialRot));
        }
        else
        {
            Debug.Log("_rotMode invalid value");
        }


        if (_initialSpeed == 0)
        {
            _initialSpeed = 0.000000000000000001f;//Epsilon doesnt work lol
        }

        if (_bulletAI.initialSpeedWeightage == 0)
        {
            _bulletAI.initialSpeedWeightage = 0.000000000000000001f;//Epsilon doesnt work lol
        }

        _bulletAI.velocity = _bulletGO.transform.up * _initialSpeed * _bulletAI.initialSpeedWeightage;//if want to inherit velo, do it during value setting

    }

}

