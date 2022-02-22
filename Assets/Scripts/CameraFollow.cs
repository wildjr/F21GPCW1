using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adapted from https://answers.unity.com/questions/600577/camera-rotation-around-player-while-following.html
public class CameraFollow : MonoBehaviour
{
    public float turnSpeed = 4.0f;
    public Transform player;

    public Vector3 initialOffset;

    private Vector3 offset;

    void Start()
    {
        offset = new Vector3(player.position.x + initialOffset.x, player.position.y + initialOffset.y, player.position.z + initialOffset.z);
    }

    void LateUpdate()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * (turnSpeed/2), Vector3.right) * offset;
        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * (turnSpeed/2), Vector3.back) * offset;
        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }
}
