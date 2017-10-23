using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : VectorStuff
{
    /*    
    public GameObject bulletGO;
    public int noOfBulletSetsToSpawn;//DO ME OR MAKE THIS A TIMER? THAT RESETS
    public float firePeriod;
    public float startDelay;

    public int noBulletsPerSet;
    public float radiusFromCenter;
    public List<float> aim;
    public float bulletInitialRot,angleSeperationBetweenBullets;

    public float spawnerRotAngularSpeed;
    public float spawnerRotAngleToPingPong;
    public float spawnerRotAngularAcceleration;
    public float spawnerInitialRot;
    public bool SpawnerTargetsPlayer;

    
    public float bulletInitialSpeed = 1f;
    public float bulletAngularSpeed;
    public float bulletAcceleration;
    public float bulletRotWeightageDiff, bulletAccelWeightageDiff;
    public int rotMode,weightageMode;
    

    public List<GameObject> bulletList = new List<GameObject>();

    private float timeSinceLastShot;
    private bool StartDelayEnd;
    private Transform playerTransform;

    void Start()
    {
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, spawnerInitialRot));
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        //=====StartDelay Start=====
        if (!StartDelayEnd && timeSinceLastShot >= startDelay)
        {
            StartDelayEnd = true;
            timeSinceLastShot = 0f;
            Debug.Log("startDelay End");
        }
        else
        {
            return;
        }
        //=====StartDelay End=====
    }

    void FixedUpdate()
    {
        //=====StartDelay Start=====
        if (!StartDelayEnd)
        {
            return;
        }
        //=====StartDelay End=====
        //=====Spawner Rotation Start=====

        if (spawnerRotAngleToPingPong > 0)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, spawnerRotAngleToPingPong * Mathf.Sin(Time.timeSinceLevelLoad * spawnerRotAngularSpeed * 0.03f)));//Add tracking player
        }
        if (SpawnerTargetsPlayer)
        {
            if (spawnerRotAngleToPingPong <= 0)
            {
                this.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
                this.transform.rotation *= Quaternion.Euler(new Vector3(0, 0, -Mathf.Atan2(playerTransform.position.x - this.transform.position.x, playerTransform.position.y - this.transform.position.y) * Mathf.Rad2Deg));
        }
        else if (!SpawnerTargetsPlayer && spawnerRotAngleToPingPong <= 0)
        {
            spawnerRotAngularSpeed += spawnerRotAngularAcceleration * Time.deltaTime;
            this.transform.rotation *= Quaternion.Euler(Time.deltaTime * new Vector3(0, 0, spawnerRotAngularSpeed));
        }

        this.transform.rotation *= Quaternion.Euler(new Vector3(0, 0, spawnerInitialRot));
        //=====Spawner Rotation End=====

        //=====Movement of bullets start=====
        for (int n = 0; n < bulletList.Count; n++)
        {
            BulletAI bulletAI = bulletList[n].GetComponent<BulletAI>();
            if (bulletAcceleration * bulletAI.accelWeightage < 0 && Vector3.SqrMagnitude(bulletAI.velocity) < 0.01f) //to allow for bullets to deccelerate & move backwards in case of negative
            {
                bulletAI.accelWeightage = -bulletAI.accelWeightage;
                bulletAI.velocity = -bulletAI.velocity;
            }
            bulletAI.velocity = AlterVector(bulletAI.velocity, bulletAngularSpeed * bulletAI.rotWeightage * Time.deltaTime, bulletAcceleration * bulletAI.accelWeightage * Time.deltaTime);//need to put this in another function so can work with multi pattern?

            bulletList[n].transform.position += bulletAI.velocity * Time.deltaTime;
        }
        //=====Movement of bullet end=====

        //=====Instatiating new bullets start=====
        
        if (timeSinceLastShot >= firePeriod)
        {
            for (int i = 0; i < aim.Count; i++)
            {//MAKE SO CAN SPAWN AT OTHER PLACES TOO
                SpawnBulletSet(bulletGO, noBulletsPerSet, this.transform.position, radiusFromCenter, angleSeperationBetweenBullets, aim[i], bulletInitialRot, rotMode, bulletRotWeightageDiff, bulletAccelWeightageDiff, weightageMode);
            }
            timeSinceLastShot = 0;
        }

    }

    void SpawnBulletSet(GameObject bullet, int bulletPerSet, Vector3 spawnLot, float spawnRadius, float bulletAngleSeperation, float aimRot, float bulletRot, int bulletRotType, float rotWeightageDiff, float accelWeightageDiff, int bulletWeightageType)//bulletRotType:   0 = local rot | 1 = world rot | 2 = targeted rot 
    {
        List<Vector3> bulletSpawnLotList = new List<Vector3>();
        List<float> bulletSpawnRotList = new List<float>();

        //=====Setting bullet spawn location start=====
        Vector3 spawnLot_cal = new Vector3(0, spawnRadius, 0);
        spawnLot_cal = AlterVector(spawnLot_cal, aimRot  + (this.transform.rotation.eulerAngles.z) , 0);//Change to center vector
        spawnLot_cal = AlterVector(spawnLot_cal, -bulletAngleSeperation * (bulletPerSet - 1) * 0.5f, 0);//changes to most side vector

        for (int i = 0; i < bulletPerSet; i++)//Sweeps across end to end and adds the vectors to bulletSpawnLotList then move vector by spawnLot
        {
            bulletSpawnLotList.Add(spawnLot_cal);
            bulletSpawnLotList[i] = spawnLot + bulletSpawnLotList[i];
            spawnLot_cal = AlterVector(spawnLot_cal, bulletAngleSeperation, 0f);
        }
        //=====Setting bullet spawn location end=====
        //=====Setting bullet spawn rotation start=====
        if (bulletRotType == 0) // 0 = local rot
        {
            for (int i = 0; i < bulletSpawnLotList.Count; i++)
            {
                bulletSpawnRotList.Add(90 + bulletRot + Mathf.Atan2(spawnLot.y - bulletSpawnLotList[i].y, spawnLot.x - bulletSpawnLotList[i].x) * Mathf.Rad2Deg);//currently rotates so bullet y axis points away from boss
            }
        }
        else if (bulletRotType == 1) // 1 = world rot
        {
            for (int i = 0; i < bulletSpawnLotList.Count; i++)
            {
                bulletSpawnRotList.Add(bulletRot);
            }
        }
        else if (bulletRotType == 2) // 2 = targeted rot
        {
            for (int i = 0; i < bulletSpawnLotList.Count; i++)
            {
                bulletSpawnRotList.Add(-Mathf.Atan2(playerTransform.position.x - bulletSpawnLotList[i].x, playerTransform.position.y - bulletSpawnLotList[i].y) * Mathf.Rad2Deg + bulletInitialRot);
            }
        }
        else
        {
            Debug.Log("bulletRotType invalid value");
        }
        //=====Setting bullet spawn rotation end=====
        //=====Instatiating bullets start=====
        for (int i = 0; i < bulletSpawnLotList.Count; i++)
        {
            GameObject instantiatedbullet = Instantiate(bullet, bulletSpawnLotList[i], Quaternion.Euler(new Vector3(0,0,bulletSpawnRotList[i])));
            instantiatedbullet.GetComponent<BulletAI>().SetUpAfterChangingBulletPatterns(bulletInitialSpeed);
            if (bulletWeightageType == 0)
            {
                instantiatedbullet.GetComponent<BulletAI>().rotWeightage = 1;
                instantiatedbullet.GetComponent<BulletAI>().accelWeightage = 1;
            }
            else if (bulletWeightageType == 1) // shape like > or <
            {
                instantiatedbullet.GetComponent<BulletAI>().rotWeightage = 1 - rotWeightageDiff * Mathf.Abs(i - 0.5f * (bulletSpawnLotList.Count - 1));
                instantiatedbullet.GetComponent<BulletAI>().accelWeightage = 1 - accelWeightageDiff * Mathf.Abs(i - 0.5f * (bulletSpawnLotList.Count - 1));
            }
            else if (bulletWeightageType == 2) // shape like / or \
            {                
                instantiatedbullet.GetComponent<BulletAI>().rotWeightage = 1 - (i * rotWeightageDiff);
                instantiatedbullet.GetComponent<BulletAI>().accelWeightage = 1 - (i * accelWeightageDiff);
            }
            bulletList.Add(instantiatedbullet);
        }
        //=====Instatiating bullets end=====
    }
    */
}
