using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    [Header("プレイヤーゲームオブジェクト")] public GameObject playerObj;
    [Header("コンティニュー位置")] public GameObject[] continuePoint;
    [Header("ゲームオーバー")] public GameObject gameOverObj;
    [Header("フェード")] public FadeImage fade;

    private Player p;
    private int nextStageNum;
    // シーン切り替え時のフラグは、シーンが切り替わったら自動的に初期化されるので、初期化用のメソッドは必要ない
    private bool startFade     = false;
    private bool doGameOver    = false;
    private bool retryGame     = false;
    private bool doSceneChange = false;

    void Start()
    {
        if (playerObj != null && continuePoint != null && continuePoint.Length > 0){
            gameOverObj.SetActive(false); // リトライ画面をオフに
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
        // ゲームオーバー時の処理
        if(GManager.instance.isGameOver && !doGameOver){
            gameOverObj.SetActive(true);
            doGameOver = true;
        }
        // Playerがやられた時の処理
        if(p != null && p.IsContinueWaiting()){
            if(continuePoint.Length > GManager.instance.continueNum){
                playerObj.transform.position = continuePoint[GManager.instance.continueNum].transform.position;
                p.ContinuePlayer();
            }
            else {
                Debug.Log("[ERROR] settings is not enough for continue points ");
            }
        }
        // ステージを切り替える
        if (fade !=null && startFade && !doSceneChange){
            if (fade.IsFadeOutComplete()){
                // ゲームリトライの場合
                if(retryGame){
                    GManager.instance.RetryGame(); // スコアのリセット
                }
                // 次のステージ
                else {
                    GManager.instance.stageNum = nextStageNum;
                }
                SceneManager.LoadScene("stage" + nextStageNum);
                doSceneChange = true;
            }
        }
    }
    /// <summary>
    /// 最初から始める。// ボタンから呼ばれる
    /// </summary>
    public void Retry(){
        ChangeScene(1); // 最初のステージに戻るので
        retryGame = true;
    }
    public void ChangeScene(int num){
        if(fade != null){
            nextStageNum = num;
            fade.StartFadeOut();
            startFade = true;
        }
    }
}
