using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("移動速度")] public float speed;
    [Header("重力(正の値)")] public float gravity;
    [Header("ジャンプ速度")] public float jumpSpeed;
    [Header("ジャンプする高さ")] public float jumpHeight;
    [Header("ジャンプ制限時間")] public float jumpLimitTime;
    [Header("踏みつけ判定の高さの割合(%)")] public float StepOnRate;
    [Header("接地判定Obj")] public GroundCheck ground;
    [Header("頭をぶつけた判定Obj")] public GroundCheck head;
    [Header("ダッシュの速さ表現")] public AnimationCurve dashCurve;
    [Header("ジャンプの速さ表現")] public AnimationCurve jumpCurve;

    private Animator anim     = null;
    private Rigidbody2D rb    = null;
    private SpriteRenderer sr = null;
    private CapsuleCollider2D capcol = null;
    private bool isGround    = false;
    private bool isJump      = false;
    private bool isOtherJump = false;
    private bool isRun       = false;
    private bool isHead      = false;
    private bool isDown      = false;
    private bool isContinue  = false;
    private float jumpPos         = 0.0f;
    private float otherJumpHeight = 0.0f;
    private float continueTime    = 0.0f;
    private float blinkTime       = 0.0f;
    private float dashTime, jumpTime;
    private float beforeKey;

    private string enemyTag = "Enemy";

    void Start()
    {
        // コンポーネントのインスタンスを捕まえる
        anim   = GetComponent<Animator>();
        rb     = GetComponent<Rigidbody2D>();
        sr     = GetComponent<SpriteRenderer>();
        capcol = GetComponent<CapsuleCollider2D>();
    }

    private void Update(){
        if(isContinue){
            // 明滅させる
            if(blinkTime > 0.2f){
                sr.enabled = true;
                blinkTime  = 0.0f;
            }
            else if (blinkTime > 0.1f) {
                sr.enabled = false;
            }
            else {
                sr.enabled = true;
            }

            // 1秒経ったら明滅終わり
            if(continueTime > 1.0f){
                isContinue   = false;
                blinkTime    = 0.0f;
                continueTime = 0.0f;
                sr.enabled   = true;
            }
            else {
                blinkTime    += Time.deltaTime;
                continueTime += Time.deltaTime;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDown){
            // 設置判定を得る
            isGround = ground.IsGround();
            isHead   = head.IsGround();
            // 各種座標軸の速度を求める
            float xSpeed = GetXSpeed();
            float ySpeed = GetYSpeed();
            // アニメーションを適用
            SetAnimation();

            // 移動速度を設定
            rb.velocity = new Vector2(xSpeed, ySpeed);
        }
        else {
            // Downしている時は、動かないようにする
            rb.velocity = new Vector2(0, -gravity);
        }
    }

    /// <summary>
    /// X成分で必要な計算をし、速度を返す。
    /// </summary>
    /// <returns></returns>
    private float GetXSpeed(){
        // キー入力されたら行動する
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;

        if (horizontalKey > 0) {
            transform.localScale = new Vector3(1, 1, 1);
            isRun = true;
            dashTime += Time.deltaTime;
            //xSpeed = speed;
            //ゆっくり加速したい場合、
            xSpeed = Input.GetAxis("Horizontal") * speed;
        }
        else if (horizontalKey < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
            isRun = true;
            dashTime += Time.deltaTime;
            //xSpeed = -speed;
            //ゆっくり加速したい場合、
            xSpeed = Input.GetAxis("Horizontal") * speed;
        }
        else {
            dashTime  = 0.0f;
            isRun = false;
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

        return xSpeed;
    }

    /// <summary>
    /// Y成分で必要な計算をし、速度を返す。フラグでプレイヤーの状態を特定して、速度を決め、アニメーションカーブに適用する。
    /// </summary>
    /// <returns>Y軸の速さ</returns>
    private float GetYSpeed(){
        
        float verticalKey   = Input.GetAxis("Vertical");
        float ySpeed = -gravity;
        // 何かを踏んだ際のジャンプ
        if (isOtherJump){
            // 現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + otherJumpHeight > transform.position.y;
            // ジャンプ時間が長くなりすぎてないか
            bool canTime   = jumpLimitTime > jumpTime;

            if (canHeight && canTime && !isHead){
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else {
                isOtherJump = false;
                jumpTime = 0.0f;
            }
            
        }
        // 地面にいる時
        else if (isGround){
            if(verticalKey > 0) {
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; // ジャンプした位置を記録する
                isJump = true;
                jumpTime = 0.0f;
            } else {
                isJump = false;
            }
        }
        // ジャンプ中
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
        // アニメーションカーブを速度に適用
        if (isJump || isOtherJump){
            ySpeed *= jumpCurve.Evaluate(dashTime);
        }
        return ySpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.collider.tag == enemyTag){
            // 踏みつけ判定になる高さ
            float stepOnHeight = (capcol.size.y * (StepOnRate / 100f));
            // 踏みつけ判定のワールド座標
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;
            foreach (ContactPoint2D p in collision.contacts) {
                if(p.point.y < judgePos){
                    // もう一度跳ねる
                    ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
                    if (o!= null) {
                        otherJumpHeight = o.boundHeight; // 踏んづけたものから跳ねる高さを取得す
                        o.playerStepOn = true; // 踏んづけたものに対して踏んづけたことを通知する
                        jumpPos = transform.position.y; // ジャンプした位置を記録する
                        isOtherJump = true;
                        isJump = false;
                    jumpTime = 0.0f;
                    }
                    else {
                        Debug.Log("ObjectCollisionがついてないよ。");
                    }

                } else {
                    // ダウンする
                    anim.Play("player_down");
                    isDown = true;
                    break;
                }
            }
        }
    }
    /// <summary>
    /// アニメーションをセットする
    /// </summary>
    private void SetAnimation(){
        anim.SetBool("jump", isJump || isOtherJump);
        anim.SetBool("ground", isGround);
        anim.SetBool("run", isRun);
    }

    /// <summary>
    /// コンティニュー待機状態か
    /// </summary>
    /// <returns></returns>
    public bool IsContinueWaiting(){
        return IsDownAnimEnd();
    }

    // ダウンアニメーションが完了しているかどうか
    private bool IsDownAnimEnd(){
        if(isDown && anim != null){
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("player_down")){
                if(currentState.normalizedTime >= 1){
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// コンティニューする
    /// </summary>
    public void ContinuePlayer(){
        isDown = false;
        anim.Play("player_stand");
        isJump = false;
        isOtherJump = false;
        isRun = false;
        isContinue = true;
    }
}
