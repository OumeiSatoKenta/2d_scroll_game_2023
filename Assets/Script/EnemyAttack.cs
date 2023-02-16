using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("スピード")] public float speed = 3.0f;
    [Header("最大移動距離")] public float maxDistance = 100.0f;

    private Rigidbody2D rb;
    private Vector3 defaultPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null){
            Debug.Log("[ERROR inspector settings are not enough");
            Destroy(this.gameObject);
        }
        defaultPos = transform.position;
    }

    void FixedUpdate()
    {
        float d = Vector3.Distance(transform.position, defaultPos);

        // 最大移動距離を超えている
        if (d > maxDistance){
            Destroy(this.gameObject);
        }
        else {
            rb.MovePosition(transform.position += transform.up * Time.deltaTime * speed);
        }
    }

    private void OnCollisionEnter2D (Collision2D collision){
        Destroy(this.gameObject);
    }
}
