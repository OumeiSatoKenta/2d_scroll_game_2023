using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーが判定内に入ったかどうか調べるスクリプト
// スコアUPのライフUPのアイテムなどで使われるが、判定用の処理は汎用的に使うのでそれらのスクリプトとは別に書いておく。
public class PlayerTriggerCheck : MonoBehaviour
{
    // 判定ないにプレイヤーがいる
    [HideInInspector] public bool isOn = false;
    private string playerTag = "Player";

    // 接触判定
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == playerTag){
            isOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.tag == playerTag){
            isOn = false;
        }
    }
}
