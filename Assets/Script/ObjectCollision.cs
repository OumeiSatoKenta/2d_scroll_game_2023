using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 何かを踏んだときに呼ばれるスクリプト
public class ObjectCollision : MonoBehaviour
{
    [Header("これを踏んだ時のプレイヤーが跳ねる高さ")] public float boundHeight;
    [Header("これを踏んだ時のプレイヤーが跳ねる速さ")] public float jumpSpeed;

    [HideInInspector] public bool playerStepOn;
}
