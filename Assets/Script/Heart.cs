using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    private Text heartText = null;
    private int oldHeartNum = 0;

    void Start()
    {
        heartText = GetComponent<Text>();
        if(GManager.instance != null){
            heartText.text = "× " + GManager.instance.heartNum;
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
        if(oldHeartNum != GManager.instance.heartNum){
            heartText.text = "× " + GManager.instance.heartNum;
            oldHeartNum    = GManager.instance.heartNum;
        }
    }
}
