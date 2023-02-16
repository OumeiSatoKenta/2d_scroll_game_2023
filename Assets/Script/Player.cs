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
    [Header("踏みつけ判定の高さの割合(%)")] public float stepOnRate;
    [Header("接地判定Obj")] public GroundCheck ground;
    [Header("頭をぶつけた判定Obj")] public GroundCheck head;
    [Header("ダッシュの速さ表現")] public AnimationCurve dashCurve;
    [Header("ジャンプの速さ表現")] public AnimationCurve jumpCurve;
    [Header("ジャンプするときに鳴らすSE")] public AudioClip jumpSE;
    [Header("やられたときに鳴らすSE")] public AudioClip downSE;
    [Header("コンティニュー時に鳴らすSE")] public AudioClip continueSE;

    private Animator anim      = null;
    private Rigidbody2D rb     = null;
    private SpriteRenderer sr  = null;
    private CapsuleCollider2D capcol = null;
    private MoveObject moveObj = null;
    private bool isGround      = false;
    private bool isJump        = false;
    private bool isOtherJump   = false;
    private bool isRun         = false;
    private bool isHead        = false;
    private bool isDown        = false;
    private bool isContinue    = false;
    private bool nonDownAnim   = false;
    private bool isClearMotion = false;
    private float jumpPos         = 0.0f;
    private float otherJumpHeight = 0.0f;
    private float continueTime    = 0.0f;
    private float blinkTime       = 0.0f;
    private float dashTime, jumpTime;
    private float beforeKey;

    private string enemyTag     = "Enemy";
    private string deadAreaTag  = "DeadArea";
    private string hitAreaTag   = "HitArea";
    private string moveFloorTag = "MoveFloor";
    private string fallFloorTag = "FallFloor";
    private string jumpStageTag  = "JumpStage";

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
        if (!isDown && !GManager.instance.isGameOver && !GManager.instance.isStageClear){
            // 設置判定を得る
            isGround = ground.IsGround();
            isHead   = head.IsGround();
            // 各種座標軸の速度を求める
            float xSpeed = GetXSpeed();
            float ySpeed = GetYSpeed();

            // 移動速度を設定
            Vector2 addVelocity = Vector2.zero;

            // アニメーションを適用
            SetAnimation();

            // 移動速度を設定
            if(moveObj != null){
                addVelocity = moveObj.GetVelocity();
            }
            rb.velocity = new Vector2(xSpeed, ySpeed) + addVelocity;
        }
        else {
            if (!isClearMotion && GManager.instance.isStageClear){
                anim.Play("player_clear");
                isClearMotion = true;
            }
            // Clear, Downしている時は、動かないようにする
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
                if(!isJump ){
                    GManager.instance.PlaySE(jumpSE);
                }
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

        bool enemy     = (collision.collider.tag == enemyTag);
        bool moveFloor = (collision.collider.tag == moveFloorTag);
        bool fallFloor = (collision.collider.tag == fallFloorTag);
        bool jumpStage = (collision.collider.tag == jumpStageTag);

        if(enemy || moveFloor || fallFloor || jumpStage){
            // 踏みつけ判定になる高さ
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));
            // 踏みつけ判定のワールド座標
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;
            foreach (ContactPoint2D p in collision.contacts) {
                if(p.point.y < judgePos){
                    // もう一度跳ねる
                    if(enemy || fallFloor || jumpStage){
                        ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
                        if (o!= null) {
                            if(enemy || jumpStage){
                                otherJumpHeight = o.boundHeight; // 踏んづけたものから跳ねる高さを取得す
                                o.playerStepOn = true; // 踏んづけたものに対して踏んづけたことを通知する
                                jumpPos = transform.position.y; // ジャンプした位置を記録する
                                isOtherJump = true;
                                isJump = false;
                                jumpTime = 0.0f;
                            }
                            else if (fallFloor) {
                                o.playerStepOn = true; // 踏んづけたものに対して踏んづけたことを通知する
                            }
                            
                        }
                        else {
                            Debug.Log("[ERROR] ObjectCollision is not attached in inspector settings");
                        }
                    }
                    else if (moveFloor){
                        moveObj = collision.gameObject.GetComponent<MoveObject>();
                    }
                } else {
                    ReceivedDamage(true);
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision){
        if(collision.collider.tag == moveFloorTag){
            // 動く床から離れた
            moveObj = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == deadAreaTag){
            ReceivedDamage(false); // ダウンモーションなし
        }
        else if(collision.tag == hitAreaTag){
            ReceivedDamage(true); // ダウンモーションあり
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
        if(GManager.instance.isGameOver){
            return false;
        }
        else {
            return IsDownAnimEnd() || nonDownAnim;
        }
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

    private void ReceivedDamage(bool downAnim){
        if (isDown){
            return;
        }
        else {
            if(downAnim){
                anim.Play("player_down");
            }
            else {
                nonDownAnim = true; // DeadAreaに落ちた時、コンティニュー処理で使う
            }
            GManager.instance.PlaySE(downSE);
            isDown = true;
            GManager.instance.SubHeartNum();
        }
    }

    /// <summary>
    /// コンティニューする
    /// </summary>
    public void ContinuePlayer(){
        GManager.instance.PlaySE(continueSE);
        anim.Play("player_stand");
        isDown      = false;
        isJump      = false;
        isOtherJump = false;
        isRun       = false;
        isContinue  = true;
        nonDownAnim = false;
    }
}
