using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ジャンプ台のアニメーション処理など
public class JumpObject : MonoBehaviour
{
    private ObjectCollision oc;
    private Animator anim;

    void Start()
    {
        oc   = GetComponent<ObjectCollision>();
        anim = GetComponent<Animator>();
        if(oc == null || anim == null){
            Debug.Log("[ERROR] settings for jump stage is not enough");
            Destroy(this);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if(oc.playerStepOn){
            anim.SetTrigger("on");
            oc.playerStepOn = false;
        }
        
    }
}
