using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuePoint : MonoBehaviour
{
    [Header("コンティニュー番号")] public int continueNum;
    [Header("音")] public AudioClip se;
    [Header("プレイヤー判定")] public PlayerTriggerCheck trigger;
    [Header("スピード")] public float speed   = 3.0f;
    [Header("動く幅")] public float moveDis  = 3.0f;

    private bool on     = false;
    private float angle = 0.0f;
    private Vector3 defaultPos;

    void Start()
    {
        // 初期化
        if(trigger == null){
            Debug.Log("[ERROR]inspector settings is not enough!");
            Destroy(this);
        }
        defaultPos = transform.position;
    }

    void Update()
    {
        // プレイヤーが範囲内に入った。
        if (trigger.isOn && !on){
            GManager.instance.continueNum = continueNum;
            GManager.instance.PlaySE(se);
            on = true;
        }

        if (on){
            if(angle < 180.0f){
                // sinカーブで振動させる。
                Vector3 vibration  = Vector3.up * moveDis * Mathf.Sin(angle * Mathf.Deg2Rad);
                transform.position = defaultPos + vibration;

                //途中からサイズが小さくなる
                if (angle > 90.0f){
                    transform.localScale = Vector3.one * (1 - ((angle - 90.0f)/90.0f));
                }
                angle += 180.0f * Time.deltaTime * speed;
            }
            else {
                gameObject.SetActive(false);
                on = false;
            }
        }
    }
}
