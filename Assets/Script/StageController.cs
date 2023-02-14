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
    [Header("ゲームオーバー時に鳴らすSE")] public AudioClip gameOverSE;
    [Header("リトライ時に鳴らすSE")] public AudioClip retrySE;
    [Header("ステージクリアSE")] public AudioClip stageClearSE;
    [Header("ステージクリア")] public GameObject stageClearObj;
    [Header("ステージクリア判定")] public PlayerTriggerCheck stageClearTrigger;

    private Player p;
    private int nextStageNum;
    // シーン切り替え時のフラグは、シーンが切り替わったら自動的に初期化されるので、初期化用のメソッドは必要ない
    private bool startFade     = false;
    private bool doGameOver    = false;
    private bool retryGame     = false;
    private bool doSceneChange = false;
    private bool doClear       = false;

    void Start()
    {
        if (isSetting()){
            gameOverObj.SetActive(false); // リトライ画面をオフに
            stageClearObj.SetActive(false); // クリア画面をオフに
            playerObj.transform.position = continuePoint[0].transform.position;
            p = playerObj.GetComponent<Player>(); // こっちはpublic 変数にアクセスする用
            if(p == null){
                Debug.Log("[DEBUG] player object isn't attached");
            }
        }
        else {
            Debug.Log("[ERROR] settings are not enough for continue points");
        }
    }

    void Update()
    {
        // ゲームオーバー時の処理
        if(GManager.instance.isGameOver && !doGameOver){
            gameOverObj.SetActive(true);
            GManager.instance.PlaySE(gameOverSE);
            doGameOver = true;
        }
        // Playerがやられた時の処理
        else if(p != null && p.IsContinueWaiting() && !doGameOver){
            if(continuePoint.Length > GManager.instance.continueNum){
                playerObj.transform.position = continuePoint[GManager.instance.continueNum].transform.position;
                p.ContinuePlayer();
            }
            else {
                Debug.Log("[ERROR] settings is not enough for continue points ");
            }
        }
        // ステージクリア時の処理
        else if (stageClearTrigger != null && stageClearTrigger.isOn && !doGameOver && !doClear){
            StageClear();
            doClear = true;                                                                                                             
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

    private bool isSetting() {
        return (playerObj != null && 
                continuePoint != null && 
                continuePoint.Length > 0 && 
                gameOverObj != null && 
                fade != null 
                && stageClearObj != null);
    }

    /// <summary>
    /// 最初から始める。// ボタンから呼ばれる
    /// </summary>
    public void Retry(){
        GManager.instance.PlaySE(retrySE);
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

    /// <summary>
    /// ステージをクリアしたときの演出をONにする
    /// </summary>
    public void StageClear(){
        GManager.instance.isStageClear = true;
        stageClearObj.SetActive(true);
        GManager.instance.PlaySE(stageClearSE);
    }
}
