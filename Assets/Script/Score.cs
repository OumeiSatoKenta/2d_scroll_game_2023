using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ゲーム内スコア点数の操作を行うスクリプト
public class Score : MonoBehaviour
{
    private Text scoreText = null;
    private int oldScore = 0;
    void Start()
    {
        scoreText = GetComponent<Text>();
        if(GManager.instance != null){
            scoreText.text = "Score " + GManager.instance.score;
        }
        else {
            Debug.Log("[ERROR] can't find GameManager Object");
            Destroy(this);
        }
    }

    void Update()
    {       
        // 毎回アップデートせずに古いスコアと違っていたら更新する
        if(oldScore != GManager.instance.score){
            scoreText.text = "Score " + GManager.instance.score;
            oldScore = GManager.instance.score;
        }
        
    }
}
