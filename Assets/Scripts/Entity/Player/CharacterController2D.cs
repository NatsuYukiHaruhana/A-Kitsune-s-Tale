using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour {
	[SerializeField] private float jumpForce = 400f;                         // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;         // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f; // How much to smooth out the movement
	[SerializeField] private bool airControl = false;                        // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask whatIsGround;                         // A mask determining what is ground to the character
	[SerializeField] private Transform groundCheck;                          // A position marking where to check if the player is grounded.
	[SerializeField] private Transform ceilingCheck;                         // A position marking where to check for ceilings
    [SerializeField] private Collider2D crouchDisableCollider;               // A collider that will be disabled when crouching
	[SerializeField] private Button_Functionality moveLeftButton;			 // Button used for touch control to move left 
	[SerializeField] private Button_Functionality moveRightButton;			 // Button used for touch control to move right
	[SerializeField] private Button_Functionality jumpButton;				 // Button used for touch control to jump
	[SerializeField] private Button_Functionality crouchButton;              // Button used for touch control to crouch
	[SerializeField] private Button_Functionality attackButton;              // Button used for touch control to attack
	
	const float groundedRadius = .4f; // Radius of the overlap circle to determine if grounded
	private bool grounded;            // Whether or not the player is grounded.
	const float ceilingRadius = .4f;  // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D rigidbody2D;
	private bool facingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
	private bool wasCrouching = false;
	private SpriteRenderer spriteRend;

    private bool fadeIn;
    private float fadeSpeed;

    private Animator animator;
    [SerializeField]
    private Animator attackAnimator;
    [SerializeField]
    private Collider2D attackCollider;
    [SerializeField]
    private Collider2D triggerCollider;

	private bool readingPopup;


    private void Awake() {
		rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRend = GetComponent<SpriteRenderer>();

        fadeIn = true;
        fadeSpeed = 0.3f;

        animator.SetBool("isCrouching", false);
        animator.SetBool("isGrounded", false);
        animator.SetFloat("moveSpeed", 0.0f);

		transform.position = Utils.LoadPlayerPosition();

		readingPopup = false;
#if UNITY_ANDROID
		moveLeftButton.gameObject.SetActive(true);
		moveRightButton.gameObject.SetActive(true);
		jumpButton.gameObject.SetActive(true);
		crouchButton.gameObject.SetActive(true);
		attackButton.gameObject.SetActive(true);
#endif
    }

    private void FixedUpdate() {
		grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		Collider2D[] collidersGround = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < collidersGround.Length; i++) {
			if (collidersGround[i].gameObject != gameObject) {
				grounded = true;
				break;
			}
		}

        Move(InputMoveHorizontal(), PressCrouchButton(), PressJumpButton());

		if (PressAttackButton()) {
			Attack();
		}

		if (!triggerCollider.enabled) {
			InvincibilityFrames();
		}
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

	public void ResetSpriteTransparency() {
        Color spriteColor = spriteRend.color;

        spriteColor.a = 1f;

        spriteRend.color = spriteColor;
    }

	private void Attack() {
		attackCollider.enabled = true;
		attackAnimator.enabled = true;
	}

	private void Move(float move, bool crouch, bool jump) {
		// If crouching, check to see if the character can stand up
		if (!crouch) {
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround)) {
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (grounded || airControl) {

			// If crouching
			if (crouch && grounded) {
				if (!wasCrouching) {
					wasCrouching = true;
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= crouchSpeed;

				// Disable one of the colliders when crouching
				if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = false;
			} else {
				// Enable the collider when not crouching
				if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = true;

				if (wasCrouching) {
					wasCrouching = false;
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !facingRight) {
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && facingRight) {
				// ... flip the player.
				Flip();
            }

            animator.SetFloat("moveSpeed", Mathf.Floor(Mathf.Abs(targetVelocity.x)));
        }
		// If the player should jump...
		if (grounded && jump) {
			// Add a vertical force to the player.
			grounded = false;
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
		}

		animator.SetBool("isCrouching", crouch);
		animator.SetBool("isGrounded", grounded);
	}

    public void OnTriggerStay2D(Collider2D collision) {
		if (collision != null) {
            if (collision.gameObject.CompareTag("Enemy")) {
                if (!triggerCollider.enabled) {
                    return;
                }
                if (collision.gameObject.GetComponent<EnemyBehaviour>().GetIsInvincible()) {
					return;
				}

                Transform enemyTransform = collision.gameObject.transform;
                if (enemyTransform.position.x > transform.position.x && !facingRight) {
                    Battle_Handler.enemyStrikeFirst = true;
                } else if (enemyTransform.position.x < transform.position.x && facingRight) {
                    Battle_Handler.enemyStrikeFirst = true;
                }

                Utils.SavePlayerPosition(transform.position);
                Utils.enemyToBattle = enemyTransform.gameObject.GetComponent<EnemyBehaviour>().GetEnemyName();
				Utils.enemyToBattleIndex = enemyTransform.gameObject.GetComponent<EnemyBehaviour>().GetIndex();
                SceneManager.LoadScene("Battle Scene");
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision != null) {
			if (collision.gameObject.CompareTag("Popup")) {
				if (!readingPopup) {
					collision.gameObject.GetComponentInChildren<Popup>().BeginRaise();
					readingPopup = true;
				}
            } else if (collision.gameObject.CompareTag("Trigger")) {
				string triggerName = collision.gameObject.name;

				string gameObjectToFind = triggerName.Substring(0, triggerName.IndexOf(':'));
				string message = triggerName.Substring(triggerName.IndexOf(':') + 2);

				GameObject receiver = GameObject.Find(gameObjectToFind);
				if (receiver != null) {
					receiver.SendMessage(message);
				}
			}
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision != null) {
            if (collision.gameObject.CompareTag("Popup")) {
                collision.gameObject.GetComponentInChildren<Popup>().BeginLower();
				readingPopup = false;
            }
        }
    }

    private float InputMoveHorizontal() {
#if UNITY_EDITOR
        return Input.GetAxisRaw("Horizontal");
#elif UNITY_ANDROID
		if (moveLeftButton.IsButtonPressed()) {
			return -1;
		}
		
		if (moveRightButton.IsButtonPressed()) {
			return 1;
		}

		return 0;
#else
		return Input.GetAxisRaw("Horizontal");
#endif
    }

    private bool PressAttackButton() {
#if UNITY_EDITOR || UNITY_WIN
        if (Input.GetKey(KeyCode.Z)) {
            return true;
        }
#endif
        return attackButton.IsButtonPressed();
    }

    private bool PressCrouchButton() {
#if UNITY_EDITOR || UNITY_WIN
        if (Input.GetButton("Crouch")) {
            return true;
        }
#endif
        return crouchButton.IsButtonPressed();
    }

    private bool PressJumpButton() {
#if UNITY_EDITOR || UNITY_WIN
        if (Input.GetButton("Jump")) {
            return true;
        }
#endif
        return jumpButton.IsButtonPressed();
    }


    private void Flip() {
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public bool GetFacingRight() {
		return facingRight;
	}

	public bool GetGrounded() {
		return grounded;
	}

	public Collider2D GetTriggerCollider() {
		return triggerCollider;
	}
}