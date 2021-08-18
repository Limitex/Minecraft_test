using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject MainCamera;

    private OperationKeys keys;

    private float mouse_sensitivity = 3f;
    private float move_sensitivity = 10f;

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

        
        var set_display_position = transform.position;
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
            set_display_position.x += fb_move_position.x;
            set_display_position.z += fb_move_position.y;
        }
        if (Input.GetKey(keys.Back))
        {
            set_display_position.x -= fb_move_position.x;
            set_display_position.z -= fb_move_position.y;
        }
        if (Input.GetKey(keys.Right))
        {
            set_display_position.x += lr_move_position.x;
            set_display_position.z += lr_move_position.y;
        }
        if (Input.GetKey(keys.Left))
        {
            set_display_position.x -= lr_move_position.x;
            set_display_position.z -= lr_move_position.y;
        }
        transform.position = set_display_position;
    }
}
