using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // インスペクターで設定する
    public float speed;
    public float gravity;
    public float jumpSpeed;
    public float jumpHeight;
    public float jumpLimitTime;
    public GroundCheck ground;
    public GroundCheck head;
    public AnimationCurve dashCurve;
    public AnimationCurve jumpCurve;

    private Animator anim  = null;
    private Rigidbody2D rb = null;
    private bool isGround  = false;
    private bool isJump    = false;
    private bool isHead    = false;
    private float jumpPos  = 0.0f;
    private float dashTime, jumpTime;
    private float beforeKey;

    void Start()
    {
        // コンポーネントのインスタンスを捕まえる
        anim = GetComponent<Animator>();
        rb   = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 設置判定を得る
        isGround = ground.IsGround();
        isHead   = head.IsGround();

        // キー入力されたら行動する
        float horizontalKey = Input.GetAxis("Horizontal");
        float verticalKey   = Input.GetAxis("Vertical");
        float xSpeed = 0.0f;
        float ySpeed = -gravity;

        if (isGround){
            if(verticalKey > 0) {
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; // ジャンプした位置を記録する
                isJump = true;
                jumpTime = 0.0f;
            } else {
                isJump = false;
            }
        }
        else if (isJump){ // ジャンプ中も押す間、かつ最大の高さを声ない間は上昇する.
            // 上方向を押しているか
            bool pushUpKey = verticalKey > 0;
            // 現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + jumpHeight > transform.position.y;
            // ジャンプ時間が長くなりすぎてないか
            bool canTime   = jumpLimitTime > jumpTime;
 
            if (pushUpKey && canHeight && canTime && !isHead){
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else {
                isJump = false;
                jumpTime = 0.0f;
            }
        }

        if (horizontalKey > 0) {
            transform.localScale = new Vector3(1, 1, 1);
            anim.SetBool("run", true);
            dashTime += Time.deltaTime;
            //xSpeed = speed;
            //ゆっくり加速したい場合、
            xSpeed = Input.GetAxis("Horizontal") * speed;
        }
        else if (horizontalKey < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
            anim.SetBool("run", true);
            dashTime += Time.deltaTime;
            //xSpeed = -speed;
            //ゆっくり加速したい場合、
            xSpeed = Input.GetAxis("Horizontal") * speed;
        }
        else {
            dashTime  = 0.0f;
            anim.SetBool("run", false);
            xSpeed = 0.0f;
        }

        // 前回の入力からダッシュの斑点を判断して速度を変える
        if (horizontalKey > 0 && beforeKey < 0){
            dashTime = 0.0f;
        }
        else if (horizontalKey > 0 && beforeKey < 0){
            dashTime = 0.0f;
        }
        beforeKey = horizontalKey;

        // アニメーションカーブを速度に適用
        xSpeed *= dashCurve.Evaluate(dashTime);
        if (isJump){
            ySpeed *= jumpCurve.Evaluate(dashTime);
        }
        anim.SetBool("jump", isJump);
        anim.SetBool("ground", isGround);
        rb.velocity = new Vector2(xSpeed, ySpeed);
    }
}
