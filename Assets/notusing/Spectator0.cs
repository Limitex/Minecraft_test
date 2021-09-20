using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OperationKeys
{
    public KeyCode Front;
    public KeyCode Back;
    public KeyCode Left;
    public KeyCode Right;
    public KeyCode Up;
    public KeyCode Down;
}

public static class ExpansionClass
{
    //動く距離のバッファ
    private static Vector3 set_display_position_buffer;

    /// <summary>
    /// マウスでカメラを操作して、カメラの向きに移動する
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="keys"></param>
    /// <param name="mouse_sensitivity"></param>
    /// <param name="move_sensitivity"></param>
    /// <param name="start_move"></param>
    /// <param name="stop_move"></param>
    public static void KeyToAngle(this Transform transform, OperationKeys keys, float mouse_sensitivity, float move_sensitivity, float start_move, float stop_move)
    {
        //マウスの移動速度
        var mouse_move_x = Input.GetAxis("Mouse X") * mouse_sensitivity;
        var mouse_move_y = Input.GetAxis("Mouse Y") * mouse_sensitivity;

        //現在の位置と向きを保存
        var set_display_position = transform.position;
        var set_display_angle = transform.eulerAngles;

        //計算のためのラジアンのやつ
        var RADIAN = Mathf.PI / 180f;

        //向きを上からに直す
        var fix_angle = set_display_angle.x > 180f ? set_display_angle.x - 270f : set_display_angle.x + 90f;

        //普通に動く距離と向きによって動く距離
        var move_distance = Time.deltaTime * move_sensitivity;
        var move_position_distance = Mathf.Sin(RADIAN * fix_angle) * move_distance;

        //上下左右の移動距離
        var fb_move_position_x = Mathf.Sin(RADIAN * set_display_angle.y) * move_position_distance;
        var fb_move_position_z = Mathf.Cos(RADIAN * set_display_angle.y) * move_position_distance;
        var lr_move_position_x = Mathf.Sin(RADIAN * (set_display_angle.y + 90f)) * move_distance;
        var lr_move_position_z = Mathf.Cos(RADIAN * (set_display_angle.y + 90f)) * move_distance;
        var move_position_y = Mathf.Cos(RADIAN * fix_angle) * move_distance;

        //向きを設定する
        set_display_angle.x -= mouse_move_y;
        set_display_angle.y += mouse_move_x;

        //キーによって移動距離を設定する
        if (Input.GetKey(keys.Front))
        {
            set_display_position.x += fb_move_position_x;
            set_display_position.z += fb_move_position_z;
            set_display_position.y += move_position_y;
        }
        if (Input.GetKey(keys.Back))
        {
            set_display_position.x -= fb_move_position_x;
            set_display_position.z -= fb_move_position_z;
            set_display_position.y -= move_position_y;
        }
        if (Input.GetKey(keys.Right))
        {
            set_display_position.x += lr_move_position_x;
            set_display_position.z += lr_move_position_z;
        }
        if (Input.GetKey(keys.Left))
        {
            set_display_position.x -= lr_move_position_x;
            set_display_position.z -= lr_move_position_z;
        }
        if (Input.GetKey(keys.Up))
        {
            set_display_position.y += move_distance;
        }
        if (Input.GetKey(keys.Down))
        {
            set_display_position.y -= move_distance;
        }

        //動き出しと止まる時のムーブ
        if (keycheck(keys))
            set_display_position_buffer = Vector3.Lerp(set_display_position_buffer, set_display_position - transform.position, start_move);
        else
            set_display_position_buffer = Vector3.Lerp(set_display_position_buffer, Vector3.zero, stop_move);

        //計算結果を反映する
        transform.position += set_display_position_buffer;
        transform.eulerAngles = set_display_angle;
    }

    /// <summary>
    /// 全てのキーの状態をチェックする
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    private static bool keycheck(OperationKeys keys) =>
        Input.GetKey(keys.Front) || Input.GetKey(keys.Back) ||
        Input.GetKey(keys.Right) || Input.GetKey(keys.Left) ||
        Input.GetKey(keys.Up) || Input.GetKey(keys.Down);
}