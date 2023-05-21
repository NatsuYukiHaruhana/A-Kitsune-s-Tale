using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour {
	[SerializeField] private float jumpForce = 400f;                         // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;         // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f; // How much to smooth out the movement
	[SerializeField] private bool airControl = false;                        // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask whatIsGround;                         // A mask determining what is ground to the character
	[SerializeField] private Transform groundCheck;                          // A position marking where to check if the player is grounded.
	[SerializeField] private Transform ceilingCheck;                         // A position marking where to check for ceilings
	[SerializeField] private Collider2D crouchDisableCollider;               // A collider that will be disabled when crouching
	/*[SerializeField] private Button_Functionality moveLeftButton;			 // Button used for touch control to move left 
	[SerializeField] private Button_Functionality moveRightbutton;			 // Button used for touch control to move right
	[SerializeField] private Button_Functionality jumpButton;				 // Button used for touch control to jump
	[SerializeField] private Button_Functionality sprintButton;              // Button used for touch control to sprint
	[SerializeField] private Button_Functionality crouchButton;              // Button used for touch control to crouch
	[SerializeField] private Button_Functionality attackButton;              // Button used for touch control to attack
	*/[SerializeField] private Button_Functionality menuButton;                // Button used for activating the menu screen

	const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool grounded;            // Whether or not the player is grounded.
	const float ceilingRadius = .2f;  // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D rigidbody2D;
	private bool facingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
	private bool wasCrouching = false;

	private void Awake() {
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate() {
		grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				grounded = true;
			}
		}

		Move(Input.GetAxisRaw("Horizontal"), Input.GetButton("Crouch"), Input.GetButton("Jump"));
		if (PressMenuButton()) {
			Debug.Log("Menu pop-up");
		}
	}


	public void Move(float move, bool crouch, bool jump) {
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
		}
		// If the player should jump...
		if (grounded && jump) {
			// Add a vertical force to the player.
			grounded = false;
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
		}
	}

	private float InputMoveHorizontal() {
#if UNITY_EDITOR
		return Input.GetAxisRaw("Horizontal");
#elif UNITY_ANDROID
		
#else
		return Input.GetAxisRaw("Horizontal");
#endif
	}

	private bool PressMenuButton() {
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Return)) {
			return true;
		}
#endif
		return menuButton.IsButtonPressed();
	}

	private void Flip() {
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


}