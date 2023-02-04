using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // インスペクターで設定する
    public float speed;
    public GroundCheck ground;

    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;

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
        float xSpeed = 0.0f;

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
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
    }
}
