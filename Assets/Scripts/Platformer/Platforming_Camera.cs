using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Platforming_Camera : MonoBehaviour
{
    [SerializeField]
    private Transform playerTrans;

    [SerializeField]
    private CharacterController2D charController;
    private bool playerFacingRight;

    [SerializeField]
    private float minY;

    [SerializeField]
    private float maxY;

    [SerializeField]
    private float xOffset;

    [SerializeField]
    private float moveSpeedX;

    [SerializeField]
    private float moveSpeedY;

    private Camera playerCam;

    private void Awake() {
        playerCam = Camera.main;
        //playerFacingRight = true;
        //playerCam.transform.position = new Vector3(-Mathf.Abs(xOffset), playerCam.transform.position.y, playerCam.transform.position.z);
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer() {
        playerCam.transform.position = new Vector3(playerTrans.position.x, playerTrans.position.y, playerCam.transform.position.z);
    }

    /*
    private void FollowPlayer() {
        Vector3 playerPos = playerTrans.position;
        Vector3 camVel = new Vector3(0f, 0f, 0f);

        float xOffsetNew = 0f;
        float yOffset = 0f;

        Debug.Log("Player: " + playerPos.x);
        Debug.Log("Camera: " + (playerCam.transform.position.x - Mathf.Abs(xOffset)));
        if (playerFacingRight && playerPos.x > playerCam.transform.position.x - Mathf.Abs(xOffset)) {
            camVel += new Vector3(moveSpeedX, 0f, 0f);

            xOffsetNew = -Mathf.Abs(Mathf.Abs(xOffset) - Mathf.Abs(playerPos.x));
        } else if (!playerFacingRight && playerPos.x < playerCam.transform.position.x + Mathf.Abs(xOffset)) {
            camVel += new Vector3(-moveSpeedX, 0f, 0f);

            xOffsetNew = Mathf.Abs(Mathf.Abs(xOffset) - Mathf.Abs(playerPos.x));
        }

        
        if (playerPos.y < playerCam.transform.position.y - Mathf.Abs(minY)) {
            camVel += new Vector3(0f, -moveSpeedY, 0f);

            yOffset = -Mathf.Abs(minY);
        }
        else if (playerPos.y > playerCam.transform.position.y + Mathf.Abs(maxY)) {
            camVel += new Vector3(0f, moveSpeedY, 0f);

            yOffset = Mathf.Abs(maxY);
        }

        playerCam.transform.position = Vector3.MoveTowards(playerCam.transform.position,
            new Vector3(playerCam.transform.position.x + xOffsetNew, playerCam.transform.position.y, playerCam.transform.position.z),
            moveSpeedX);
        playerCam.transform.position = Vector3.MoveTowards(playerCam.transform.position,
            new Vector3(playerCam.transform.position.x, playerCam.transform.position.y + yOffset, playerCam.transform.position.z),
            moveSpeedY);

        playerCam.transform.position = Vector3.SmoothDamp(playerCam.transform.position,
                new Vector3(playerCam.transform.position.x + Mathf.Abs(xOffsetNew - playerPos.x), playerCam.transform.position.y + yOffset, playerCam.transform.position.z),
                ref camVel,
                1f);
    }
    */
    /*
    private void Flip() {
        if (charController.GetFacingRight() != playerFacingRight) {
            playerFacingRight = !playerFacingRight;

            xOffset *= -1f;
        }
    }*/
}
