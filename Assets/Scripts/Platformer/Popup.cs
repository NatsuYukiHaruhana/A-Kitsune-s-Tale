using UnityEngine;

public class Popup : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer spriteRend;
    [SerializeField]
    private AudioClip audioClip;

    private Sound_Manager voiceManager;

    private float moveSpeedY = 0.0005f;
    private float scaleSpeed = 0.01f;

    private float upperBoundY;
    private float lowerBoundY;

    private bool doMove = false;
    private bool doRaise = false;
    private bool doLower = false;

    private void Start() {
        upperBoundY = transform.position.y + 1f;
        lowerBoundY = transform.position.y - 0.2f;

        voiceManager = GameObject.Find("Voice Handler").GetComponent<Sound_Manager>();
    }

    private void Update() {
        if (doMove) {
            Move();
        } else if (doRaise) {
            Raise();
        } else if (doLower) {
            Lower();
        }
    }

    public void BeginRaise() {
        if (doLower) {
            doLower = false;
        }
        doRaise = true;
        spriteRend.enabled = true;
    }

    public void BeginLower() {
        doRaise = false;
        doLower = true;
        doMove = false;
    }

    private void Raise() {
        transform.localScale += new Vector3(0f, scaleSpeed, 0f);

        if (Utils.ApproximatelyEqual(transform.localScale.y, 1f, 0.01f)) {
            doRaise = false;
            //doMove = true;

            if (audioClip != null) {
                voiceManager.PlaySound(audioClip);
            }
        }
    }

    private void Lower() {
        transform.localScale -= new Vector3(0f, scaleSpeed, 0f);

        if (Utils.ApproximatelyEqual(transform.localScale.y, 0f, 0.01f)) {
            doLower = false;
            spriteRend.enabled = false;
        }
    }

    private void Move() {
        if (Utils.ApproximatelyEqual(transform.position.y, lowerBoundY, 0.02f)) {
            moveSpeedY *= -1;
        } else if (Utils.ApproximatelyEqual(transform.position.y, upperBoundY, 0.02f)) {
            moveSpeedY *= -1;
        }

        transform.position += new Vector3(0f, moveSpeedY, 0f);
    }
}
