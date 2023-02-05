using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string groundTag = "Ground";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;
    // 設置判定を返すメソッド
    // 物理判定の更新ごとに呼ぶ必要がある。
    public bool IsGround(){
        // 動く床のギミックでフラグを３種類必要。
        if(isGroundEnter || isGroundStay){
            isGround = true;
        }
        else if(isGroundExit){
            isGround = false;
        }
        // フラグをオフにしておく。
        isGroundEnter = false;
        isGroundStay  = false;
        isGroundExit  = false;
        return isGround;
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.tag == groundTag){
            isGroundEnter = true;
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision){
        if (collision.tag == groundTag){
            isGroundStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if (collision.tag == groundTag){
            isGroundExit = true;
        }
    }

}
