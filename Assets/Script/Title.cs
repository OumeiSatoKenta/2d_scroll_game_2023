using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [Header("フェード")] public FadeImage fade;

    private bool firstPush = false;
    private bool goNextScene = false;
    
    public void PressStart()
    {
        Debug.Log("Press Start!");
        if (!firstPush)
        {
            Debug.Log("Go Next Scene!");
            // ここに次のシーンへ行く命令を書く
            fade.StartFadeOut();
            firstPush = true;
        }
    }
    private void Update(){
        if(!goNextScene && fade.IsFadeOutComplete()){
            SceneManager.LoadScene("1st_stage");
            goNextScene = true;
        }
    }
}
