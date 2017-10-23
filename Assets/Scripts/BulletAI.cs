using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAI : MonoBehaviour
{
    public Sprite sprite;
    public Vector3 velocity;//follows world direction
    public float accelWeightage, rotWeightage, initialSpeedWeightage;
    public float bulletMovementPatternStartTime;
    public float timeOfLastShot;
}


