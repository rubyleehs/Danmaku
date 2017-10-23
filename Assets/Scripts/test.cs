using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class boolAndTime
{
    public float timeTest;
    public bool _bool;
    public float _duration;
}

public class test : MonoBehaviour {

    public List<boolAndTime> boolTime;

    public GameObject GO;

	// Update is called once per frame
	void Update () {
        for (int i = 0; i < boolTime.Count;)
        {
            if(Time.time >= boolTime[i].timeTest + boolTime[i]._duration)
            {
                if (boolTime[i]._bool)
                {

                }
            }
        }
	}

}
