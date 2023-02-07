using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵が壁などに当たったときに折り返す処理
public class EnemyCollisionCheck : MonoBehaviour
{
    [HideInInspector] public bool isOn = false;

    private string groundTag = "Ground";
    private string enemyTag  = "Enemy";

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.tag == groundTag || collision.tag == enemyTag){
            isOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if (collision.tag == groundTag || collision.tag == enemyTag){
            isOn = false;
        }
    }
}
