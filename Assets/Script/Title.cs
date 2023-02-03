using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    private bool firstPush = false;
    
    public void Start()
    {
        Debug.Log("Press Start!");
        if (!firstPush)
        {
            Debug.Log("Go Next Scene!");
            // ここに次のシーンへ行く命令を書く
        }
        firstPush = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
