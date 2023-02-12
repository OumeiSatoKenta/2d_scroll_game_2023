using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 床や天井、動く床の接地判定
// 例）Player の頭と足でそれぞれ判定を作る=> HeadChecker, GroundChecker
public class GroundCheck : MonoBehaviour
{
    [Header("エフェクトがついた床を判定するかどうか")] public bool checkPlatformGround = true;

    private string groundTag    = "Ground";
    private string platformTag  = "GroundPlatform";
    private string moveFloorTag = "MoveFloor";
    private string fallFloorTag = "FallFloor";
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

    private bool isFloorTag (Collider2D collision) {
        return (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag);
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.tag == groundTag){
            isGroundEnter = true;
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag)){
            isGroundEnter = true;
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision){
        if (collision.tag == groundTag){
            isGroundStay = true;
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag)){
            isGroundStay = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision){
        if (collision.tag == groundTag){
            isGroundExit = true;
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag)){
            isGroundExit = true;
        }
    }

}
