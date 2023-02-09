using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スコアアイテムを取得した時のスクリプト
public class ScoreItem : MonoBehaviour
{
    [Header("加算するスコア")] public int myScore;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;

    void Update()
    {
        // Playerが判定ないに入ったら
        if (playerCheck.isOn){
            if(GManager.instance != null){
                GManager.instance.score += myScore;
                Destroy(this.gameObject);
            }
        }        
    }
}
