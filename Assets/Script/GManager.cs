using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲームマネージャー処理用のスクリプト
// シーン全体で1つに管理しておきたい情報を持つ
public class GManager : MonoBehaviour
{
    public static GManager instance = null; // static でメモリ確保
    
    [Header("スコア")] public int score;
    [Header("現在のステージ")] public int stageNum;
    [Header("現在の復帰位置")] public int continueNum;
    [Header("現在の残機")] public int heartNum;
    [Header("デフォルトの残機")] public int defaultHeartNum;
    [HideInInspector] public bool isGameOver = false;

    private AudioSource audioSource = null;

    private void Awake(){
        if(instance == null){
            instance = this;
            // シーン切り替え時にスクリプトが付いているゲームオブジェクトを破棄されない状態にする
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            // 開発で全てのシーンにGameManagerを置く（最初から再生しなくて便利）が、
            // タイトルから通してテストするときに2つ存在してしまい困るので、既に存在する時は破棄する
            Destroy(this.gameObject);
        }
    }

    private void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 残機を1つ減らす
    /// </summary>
    public void AddHeartNum(){
        if(heartNum <99){
            ++heartNum;
        }
    }

    public void SubHeartNum(){
        if(heartNum > 0){
            --heartNum;
        }
        else {
            isGameOver = true;
        }
    }

    /// <summary>
    /// 最初から始める時の処理
    /// </summary>
    public void RetryGame(){
        isGameOver  = false;
        heartNum    = defaultHeartNum;
        score       = 0;
        stageNum    = 1;
        continueNum = 0;
    }

    /// <summary>
    /// SEを鳴らす
    /// </summary>
    /// <param name="clip"></param>    
    public void PlaySE(AudioClip clip){
        if(audioSource != null){
            audioSource.PlayOneShot(clip);
        }
        else {
            Debug.Log("[DEBUG] Audio Source is not attached!");
        }
    }
}
