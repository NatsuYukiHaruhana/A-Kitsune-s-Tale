using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Animation_Behaviour : MonoBehaviour
{
    [SerializeField]
    private Animator effectAnimator;
    [SerializeField]
    private SpriteRenderer spriteRend;

    public void FinishAnimation() {
        effectAnimator.enabled = false;
        spriteRend.sprite = null;
    }
}
