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
    public GroundCheck ground;

    private Animator anim  = null;
    private Rigidbody2D rb = null;
    private bool isGround  = false;
    private bool isJump    = false;
    private float jumpPos  = 0.0f;

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
            } else {
                isJump = false;
            }
        }
        else if (isJump){ // ジャンプ中も押す間、かつ最大の高さを声ない間は上昇する. 
            if (verticalKey > 0 && jumpPos + jumpHeight > transform.position.y){
                ySpeed = jumpSpeed;
            }
            else {
                isJump = false;
            }
        }

        if (horizontalKey > 0) {
            transform.localScale = new Vector3(1, 1, 1);
            anim.SetBool("run", true);
            //xSpeed = speed;
            //ゆっくり加速したい場合、
            xSpeed = Input.GetAxis("Horizontal") * speed;
        }
        else if (horizontalKey < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
            anim.SetBool("run", true);
            xSpeed = -speed;
        }
        else {
            anim.SetBool("run", false);
            xSpeed = 0.0f;
        }
        rb.velocity = new Vector2(xSpeed, ySpeed);
    }
}
