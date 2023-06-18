using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform boundaryCheck = null;
    [SerializeField] 
    private LayerMask whatIsGround;         // A mask determining what is ground to the character
    const float groundedRadius = .2f;       // Radius of the overlap circle to determine if grounded

    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRend;
    private BoxCollider2D col2D;

    private bool fadeIn;
    private float fadeSpeed;

    [SerializeField]
    private float moveSpeed;
    [Range(0, .3f)][SerializeField] 
    private float movementSmoothing = .05f; // How much to smooth out the movement
    private Vector3 velocity = Vector3.zero;
    private bool facingRight = true;

    private string enemyName;

    private bool doRespawn;
    private long respawnTimer;

    private long invincibilityTimer;
    private bool isInvincible;

    private int indexInArray;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();

        fadeIn = true;
        fadeSpeed = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForFlip();
        DoMovement();

        if (doRespawn) {
            CheckForRespawn();
        }

        if (rigidbody2D.isKinematic) {
            InvincibilityFrames();
        }

        DoInvincibilityFrames();
    }

    public void InitEnemy(string newEnemyName, float newMoveSpeed, int index) {
        enemyName = newEnemyName;
        name = enemyName;

        moveSpeed = newMoveSpeed;
        indexInArray = index;

        if (moveSpeed == 0f) {
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        animator.runtimeAnimatorController = Resources.Load("Animations/" + enemyName + "/" + enemyName) as RuntimeAnimatorController;

        SpriteRenderer spriteRend = gameObject.GetComponent<SpriteRenderer>();

        boundaryCheck.transform.localPosition = new Vector3(boundaryCheck.transform.localPosition.x + moveSpeed * movementSmoothing / 2f,
                                                            -spriteRend.size.y / 2,
                                                            boundaryCheck.transform.localPosition.z);

        List<float> colliderData = Utils.LoadEnemyData(enemyName);

        col2D = gameObject.AddComponent<BoxCollider2D>();
        col2D.size = new Vector2(colliderData[0], colliderData[1]);
        col2D.offset = new Vector2(colliderData[2], colliderData[3]);
    }

    private void DoMovement() {
        float movementDirection = facingRight == true ? 1f : -1f;
        float move = moveSpeed * movementDirection;

        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move, rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
    }

    private void CheckForFlip() {
        bool isGrounded = false;

        // The enemy is grounded if a circlecast to the boundary check position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(boundaryCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                isGrounded = true;
            }
        }

        if (!isGrounded) {
            Flip();
        }
    }

    private void Flip() {
        // Switch the way the entity is labelled as facing.
        facingRight = !facingRight;

        // Multiply the entity's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public string GetEnemyName() {
        return enemyName;
    }

    public void SetRespawn() {
        doRespawn = true;
        respawnTimer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + 10000;
        invincibilityTimer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + 13000;

        animator.enabled = false;
        spriteRend.enabled = false;
        rigidbody2D.isKinematic = true;
        rigidbody2D.velocity = Vector2.zero;
        col2D.enabled = false;
        isInvincible = true;
    }

    private void CheckForRespawn() {
        if (respawnTimer < DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) {
            doRespawn = false;

            animator.enabled = true;
            spriteRend.enabled = true;
            col2D.enabled = true;
        }
    }

    private void ResetSpriteTransparency() {
        Color spriteColor = spriteRend.color;

        spriteColor.a = 1f;

        spriteRend.color = spriteColor;
    }

    private void InvincibilityFrames() {
        Color spriteColor = spriteRend.color;
        if (fadeIn) {
            spriteColor.a += fadeSpeed;

            if (spriteColor.a >= 1f) {
                fadeIn = false;
            }
        } else {
            spriteColor.a -= fadeSpeed;

            if (spriteColor.a <= 0f) {
                fadeIn = true;
            }
        }

        spriteRend.color = spriteColor;
    }

    private void DoInvincibilityFrames() {
        if (invincibilityTimer < DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) {
            rigidbody2D.WakeUp();
            rigidbody2D.isKinematic = false;
            ResetSpriteTransparency();
            isInvincible = false;
        }
    }

    public int GetIndex() {
        return indexInArray;
    }

    public bool GetIsInvincible() {
        return isInvincible;
    }
}
