using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 6f;

    private Vector3 movement;

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");//"raw" means value is ONLY -1,0 & 1. So it snap to full speed instead of accelerating to full.
        float y = Input.GetAxisRaw("Vertical");

        Move(x, y);
    }

    void Move(float x, float y)
    {
        movement.Set(x, y, 0f);
        movement = movement.normalized * Speed * Time.deltaTime;
        this.transform.position += movement;
    }
}