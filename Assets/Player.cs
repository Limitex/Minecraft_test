using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;

    public GameObject MainCamera;

    private OperationKeys keys;

    private float mouse_sensitivity = 3f;
    private float move_sensitivity = 5000f;

    private static readonly float RADIAN = Mathf.PI / 180f;

    void Start()
    {
        keys.Front = KeyCode.W;
        keys.Back = KeyCode.S;
        keys.Left = KeyCode.A;
        keys.Right = KeyCode.D;
        keys.Up = KeyCode.Space;
        keys.Down = KeyCode.LeftShift;
    }

    void Update()
    {
        var mouse_move = new Vector2(Input.GetAxis("Mouse X") * mouse_sensitivity, Input.GetAxis("Mouse Y") * mouse_sensitivity);
        var set_display_angle = MainCamera.transform.eulerAngles;
        set_display_angle.x -= mouse_move.y;
        set_display_angle.y += mouse_move.x;
        MainCamera.transform.eulerAngles = set_display_angle;

        Vector3 force = Vector3.zero;

        //var set_display_position = transform.position;
        var move_distance = Time.deltaTime * move_sensitivity;
        var fb_move_position = new Vector2(
            Mathf.Sin(RADIAN * set_display_angle.y) * move_distance, 
            Mathf.Cos(RADIAN * set_display_angle.y) * move_distance
        );
        var lr_move_position = new Vector2(
            Mathf.Sin(RADIAN * (set_display_angle.y + 90f)) * move_distance,
            Mathf.Cos(RADIAN * (set_display_angle.y + 90f)) * move_distance
        );
        if (Input.GetKey(keys.Front))
        {
            force.x += fb_move_position.x;
            force.z += fb_move_position.y;
        }
        if (Input.GetKey(keys.Back))
        {
            force.x -= fb_move_position.x;
            force.z -= fb_move_position.y;
        }
        if (Input.GetKey(keys.Right))
        {
            force.x += lr_move_position.x;
            force.z += lr_move_position.y;
        }
        if (Input.GetKey(keys.Left))
        {
            force.x -= lr_move_position.x;
            force.z -= lr_move_position.y;
        }

        if (Input.GetKey(keys.Up))
        {
            force.y += move_sensitivity / 100f;
        }

        if (Input.GetKey(keys.Down))
        {
            force.y -= move_sensitivity / 100f;
        }

        //transform.position = set_display_position;
        rb.AddForce(force, ForceMode.Force);
    }
}
