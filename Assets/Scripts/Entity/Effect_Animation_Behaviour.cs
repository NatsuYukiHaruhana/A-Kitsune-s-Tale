using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Animation_Behaviour : MonoBehaviour
{
    [SerializeField]
    private Animator effectAnimator;
    [SerializeField]
    private SpriteRenderer spriteRend;

    private bool animationFinished = false;

    public void FinishAnimation() {
        effectAnimator.enabled = false;
        spriteRend.sprite = null;
        animationFinished = true;
    }

    public bool GetAnimationFinishedAndReset() {
        if (animationFinished) {
            animationFinished = false;
            return true;
        }

        return false;
    }
}
