using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy Type1の挙動に関するスクリプト
/// </summary>
public class Enemy_type1 : MonoBehaviour
{
    [Header("加算スコア")] public int myScore;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("接触判定")] public EnemyCollisionCheck checkCollision;

    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    private bool rightTleftF = false;
    private bool isDead = false;
    void Start()
    {
        rb   = GetComponent<Rigidbody2D>();
        sr   = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        oc   = GetComponent<ObjectCollision>();
        col  = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(!oc.playerStepOn){
            if (sr.isVisible || nonVisibleAct){
                // 行動する
                if (checkCollision.isOn){ // 接触判定スクリプトで判定したフラグ
                    rightTleftF = !rightTleftF;
                }
                int xVector = -1; //デフォルトは左方向
                if (rightTleftF){
                    xVector = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                } 
                else {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                rb.velocity = new Vector2(xVector * speed, -gravity);
            }
            else {
                rb.Sleep();
            }
        }
        else {
            if(!isDead){
                if (GManager.instance != null){
                    //スコア加算
                    GManager.instance.score += myScore;
                }
                anim.Play("enemy_type1_down");
                rb.velocity = new Vector2(0, -gravity);
                isDead = true;
                col.enabled = false;
                Destroy(gameObject, 3f);
            }
            else {
                transform.Rotate(new Vector3(0,0,2));
            }
        }
        
    }
}
