using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    private bool doMovementX = false;
    private bool doMovementY = false;
    private bool moveBackwards = false;
    private float minPosX;
    private float minPosY;
    private float maxPosX;
    private float maxPosY;

    [SerializeField]
    private float xPosModifier = 0.3f;
    [SerializeField]
    private float yPosModifier = 0.3f;
    [SerializeField]
    private float xPosOffset = 1f;
    [SerializeField]
    private float yPosOffset = 0f;

    private GameObject target = null;

    [SerializeField]
    private float moveSpeed = 0.01f;

    private void Awake() {
        minPosX = transform.position.x;
        maxPosX = transform.position.x + xPosModifier;
        minPosY = transform.position.y;
        maxPosY = transform.position.y + yPosModifier;
    }

    private void FixedUpdate()
    {
        DoMovementOnAxis(doMovementX, minPosX, maxPosX, true);
        DoMovementOnAxis(doMovementY, minPosY, maxPosY, false);
    }

    private void DoMovementOnAxis(bool doMovementOnAxis, float minPos, float maxPos, bool isOnXAxis) {
        if (doMovementOnAxis) {
            Vector3 delta = isOnXAxis ? Vector3.right : Vector3.up;
            if (moveBackwards) {
                delta *= -1;
            }
            
            transform.position += delta * moveSpeed;
            float axis = isOnXAxis ? transform.position.x : transform.position.y;
            
            if (Utils.ApproximatelyEqual(axis, minPos, 0.02f)) {
                moveBackwards = false;
                transform.position = transform.position + delta * (axis - minPos);
            } else if (Utils.ApproximatelyEqual(axis, maxPos, 0.02f)) {
                moveBackwards = true;
                transform.position = transform.position + delta * (axis - maxPos);
            }
        }
    }

    public void SetTarget(GameObject arrowTarget) {
        if (arrowTarget == null) {
            Debug.Log("arrowTarget is set to null!");
            return;
        }
        target = arrowTarget;

        float targetXPos = target.transform.position.x + xPosOffset;
        float targetYPos = target.transform.position.y + yPosOffset;
        transform.position = new Vector2(targetXPos, targetYPos);

        minPosX = targetXPos;
        maxPosX = targetXPos + xPosModifier;
        minPosY = targetYPos;
        maxPosY = targetYPos + yPosModifier;
    }

    public void SetMovementX(bool newMovementValue) {
        doMovementX = newMovementValue;
    }

    public void SetMovementY(bool newMovementValue) {
        doMovementY = newMovementValue;
    }

    public void SetVisible(bool visible, bool moveOnX = false, bool moveOnY = false) {
        gameObject.GetComponent<SpriteRenderer>().enabled = visible;
        
        SetMovementX(moveOnX);
        SetMovementY(moveOnY);
    }

    public void SetOffsetX(float newOffsetX) {
        xPosOffset = newOffsetX;
    }

    public void SetOffsetY(float newOffsetY) {
        yPosOffset = newOffsetY;
    }

    // Arrow always faces to the left. Rotate to 0 on the z axis to make it face to the right instead.
    public void RotateArrow(float rotate) {
        Quaternion newRotation = transform.rotation;
        newRotation.z = rotate;
        transform.rotation = newRotation;
    }
}
