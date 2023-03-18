using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 落ちる床
public class FallDownFloor : MonoBehaviour
{
    [Header("スプライトがあるオブジェクト")] public GameObject spriteObj;
    [Header("振動幅")]   public float vibrationWidth          = 0.05f;
    [Header("振動速度")] public float vibrationSpeed          = 30.0f;
    [Header("落ちるまでの時間")] public float fallTime         = 1.0f;
    [Header("落ちていく速度")] public float fallSpeed          = 10.0f;
    [Header("落ちてから戻ってくる時間")] public float returnTime = 5.0f;

    private bool isOn;
    private bool isFall;
    private bool isReturn;
    private Vector3 spriteDefaultPos;
    private Vector3 floorDefaultPos;
    private Vector2 fallVelocity;
    private BoxCollider2D col;
    private Rigidbody2D rb;
    private ObjectCollision oc;
    private SpriteRenderer sr;
    private float timer = 0.0f;
    private float fallingTimer = 0.0f;
    private float returnTimer  = 0.0f;
    private float blinkTimer   = 0.0f;
    void Start()
    {
        // 初期設定
        col = GetComponent<BoxCollider2D>();
        rb  = GetComponent<Rigidbody2D>();
        oc  = GetComponent<ObjectCollision>();
        if (spriteObj != null && oc != null && col != null && rb != null) {
            spriteDefaultPos = spriteObj.transform.position;
            fallVelocity     = new Vector2(0, -fallSpeed);
            floorDefaultPos = gameObject.transform.position;
            sr = spriteObj.GetComponent<SpriteRenderer>();
            if (sr = null) {
                Debug.Log("[ERROR] fallDownFloor: SpriteRenderer is not attached at the inspector");
                Destroy(this);
            }
        } else {
            Debug.Log("[ERROR] fallDownFloor: some inspector settings are not attached");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーが一回でも乗ったらフラグをオンに
        if (oc.playerStepOn){
            isOn = true;
            oc.playerStepOn = false;
        }

        // プレイヤーが乗ってから落ちるまでの間
        if (isOn && !isFall){
            // 振動する
            Vector3 vibVector3 = new Vector3(Mathf.Sin(timer * vibrationSpeed) * vibrationWidth, 0, 0);
            spriteObj.transform.position = spriteDefaultPos + vibVector3;

            // 一定時間経ったら落ちる
            if (timer > fallTime){
                isFall = true;
            }
            timer += Time.deltaTime;
        }

        // 一定時間経つと明滅して戻ってくる
        if (isReturn){
            sr = spriteObj.GetComponent<SpriteRenderer>();
            // 明滅しているときに戻る
            if (blinkTimer > 0.2f){
                sr.enabled = true;
                blinkTimer = 0.0f;
            }
            // 明滅消えているとき
            else {
                sr.enabled = false;
            }
            // 1秒経ったら明滅終わり
            if (returnTimer > 1.0f){
                isReturn    = false;
                blinkTimer  = 0f;
                returnTimer = 0f;
                sr.enabled  = true;
            }
            else {
                blinkTimer  += Time.deltaTime;
                returnTimer += Time.deltaTime;
            }
        }
    }
    
    private void FixedUpdate(){
        // 落下中
        if(isFall){
            rb.velocity = fallVelocity;

            // 一定時間経つと元のいちに戻る
            if (fallingTimer > returnTime){
                isReturn           = true;
                transform.position = floorDefaultPos;
                rb.velocity        = Vector2.zero;
                isFall             = false;
                timer              = 0.0f;
                fallingTimer       = 0.0f;
            }
            else {
                fallingTimer += Time.deltaTime;
                isOn         = false;
            }
        }
    }
}
