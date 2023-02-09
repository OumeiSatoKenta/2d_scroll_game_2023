using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Header("プレイヤーゲームオブジェクト")] public GameObject playerObj;
    [Header("コンティニュー位置")] public GameObject[] continuePoint;

    private Player p;

    void Start()
    {
        if (playerObj != null && continuePoint != null && continuePoint.Length > 0){
            playerObj.transform.position = continuePoint[0].transform.position;
            p = playerObj.GetComponent<Player>(); // こっちはpublic 変数にアクセスする用
            if(p == null){
                Debug.Log("[DEBUG] player object isn't attached");
            }
        }
        else {
            Debug.Log("[ERROR] settings is not enough for continue points");
        }
    }

    void Update()
    {
        if(p != null && p.IsContinueWaiting()){
            if(continuePoint.Length > GManager.instance.continueNum){
                playerObj.transform.position = continuePoint[GManager.instance.continueNum].transform.position;
                p.ContinuePlayer();
            }
            else {
                Debug.Log("[ERROR] settings is not enough for continue points ");
            }
        }
    }
}
