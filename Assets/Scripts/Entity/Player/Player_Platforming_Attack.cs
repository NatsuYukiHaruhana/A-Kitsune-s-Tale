using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Platforming_Attack : MonoBehaviour
{
    [SerializeField]
    private Animator attackAnimator;
    [SerializeField]
    private Collider2D attackCollider;
    [SerializeField]
    private SpriteRenderer spriteRend;

    public void FinishAttack() {
        attackCollider.enabled = false;
        attackAnimator.enabled = false;
        spriteRend.sprite = null;
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision != null) {
            if (collision.gameObject.CompareTag("Enemy")) {
                if (attackCollider.enabled == true) {
                    Battle_Handler.playerStrikeFirst = true;
                }

                SceneManager.LoadScene("Battle Scene");
            }
        }
    }
}
