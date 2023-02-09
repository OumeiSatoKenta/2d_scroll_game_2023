using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Stage番号を更新するスクリプト
public class StageNum : MonoBehaviour
{
    private Text stageText = null;
    private int oldStageNum = 0;

    void Start()
    {
        stageText = GetComponent<Text>();
        if (GManager.instance != null){
            stageText.text = "Stage " + GManager.instance.stageNum;
        }
        else {
            Debug.Log("[ERROR] can't find GameManager Object");
            Destroy(this);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // 毎回UIを更新するのは負荷になるので、ステージ数が更新されたときだけUIのテキストを更新する
        if (oldStageNum != GManager.instance.stageNum){
            stageText.text = "Stage " + GManager.instance.stageNum;
            oldStageNum = GManager.instance.stageNum;
        }
        
    }
}
