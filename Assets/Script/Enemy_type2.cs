using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// タイプ２の敵用のスクリプト
// 例) 花
public class Enemy_type2 : MonoBehaviour
{
    [Header("攻撃オブジェクト")] public GameObject attackObj; //攻撃エフェクトのオブジェクト
    [Header("攻撃間隔")] public float interval;

    private Animator anim;
    private float timer;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null || attackObj == null){
            Debug.Log("[ERROR] inspector settings are not enough");
            Destroy(this.gameObject);
        }
        else {
            attackObj.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);

        // 通常の状態
        if (currentState.IsName("flower_idle")){
            if(timer > interval){
                anim.SetTrigger("attack");
                timer = 0.0f;
            }
            else {
                timer += Time.deltaTime;
            }
        }
    }

    public void Attack(){
        GameObject g = Instantiate(attackObj);
        g.transform.SetParent(transform);
        g.transform.position = attackObj.transform.position;
        g.transform.rotation = attackObj.transform.rotation;
        g.SetActive(true);
    }
}
