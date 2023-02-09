using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲームマネージャー処理用のスクリプト
// シーン全体で1つに管理しておきたい情報を持つ
public class GManager : MonoBehaviour
{
    public static GManager instance = null; // static でメモリ確保
    public int score;
    public int stageNum;
    public int continueNum;
    public int heartNum;

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
}
