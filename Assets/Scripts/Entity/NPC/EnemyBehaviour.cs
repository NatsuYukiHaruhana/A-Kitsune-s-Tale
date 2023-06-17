using System.Collections.Generic;
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

    [SerializeField]
    private float moveSpeed;
    [Range(0, .3f)][SerializeField] 
    private float movementSmoothing = .05f; // How much to smooth out the movement
    private Vector3 velocity = Vector3.zero;
    private bool facingRight = true;

    private string enemyName;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForFlip();
        DoMovement();
    }

    public void InitEnemy(string newEnemyName, float newMoveSpeed) {
        enemyName = newEnemyName;
        moveSpeed = newMoveSpeed;

        if (moveSpeed == 0f) {
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        animator.runtimeAnimatorController = Resources.Load("Animations/" + enemyName + "/" + enemyName) as RuntimeAnimatorController;

        SpriteRenderer spriteRend = gameObject.GetComponent<SpriteRenderer>();

        boundaryCheck.transform.localPosition = new Vector3(boundaryCheck.transform.localPosition.x + moveSpeed * movementSmoothing / 2f,
                                                            -spriteRend.size.y / 2,
                                                            boundaryCheck.transform.localPosition.z);

        List<float> colliderData = Utils.LoadEnemyData(enemyName);

        BoxCollider2D col2D = gameObject.AddComponent<BoxCollider2D>();
        col2D.size = new Vector2(colliderData[0], colliderData[1]);
        col2D.offset = new Vector2(colliderData[2], colliderData[3]);
        BoxCollider2D col2Dtrigger = gameObject.AddComponent<BoxCollider2D>();
        col2Dtrigger.isTrigger = true;
        col2Dtrigger.size = new Vector2(colliderData[0], colliderData[1]);
        col2Dtrigger.offset = new Vector2(colliderData[2], colliderData[3]);
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
}
