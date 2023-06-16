using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Animation_Behaviour : MonoBehaviour {
    [SerializeField]
    private Animator spellAnimator;
    [SerializeField]
    private SpriteRenderer spriteRend;

    private Transform targetTransform = null;
    private float moveSpeed = 0f;

    private void Update() {
        if (targetTransform != null) {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, moveSpeed);
        }
    }

    public void FinishAnimation() {
        Destroy(this.gameObject);
    }

    public void SetTargetTransform(Transform targetTransform) {
        this.targetTransform = targetTransform;
    }

    public void SetMoveSpeed(float moveSpeed) {
        this.moveSpeed = moveSpeed;
    }
}
