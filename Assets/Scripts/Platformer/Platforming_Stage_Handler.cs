using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforming_Stage_Handler : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private Transform playerTrans;
    private Vector3 respawnPlayerPos;
    private Rigidbody2D playerRB;
    private bool respawnSet;

    private CharacterController2D playerController;

    private float yLimit = -10f;

    private void Awake() {
        playerTrans = player.transform;
        respawnSet = false;
    }

    private void Start() {
        playerController = player.GetComponent<CharacterController2D>();
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckRespawn();
        if (respawnSet) {
            CheckPlayerFall();
        }
    }

    private void CheckPlayerFall() {
        if (playerTrans.position.y < yLimit) {
            playerTrans.position = respawnPlayerPos;
            playerRB.velocity = new Vector3(0f, 0f, 0f);
        }
    }

    private void CheckRespawn() {
        if (playerController.GetGrounded()) {
            respawnSet = false;
        } else {
            if (!respawnSet) {
                if (playerRB.velocity.y > 0.01f) {
                    respawnPlayerPos = playerTrans.position;
                    respawnSet = true;
                } else {
                    respawnPlayerPos = playerTrans.position;

                    if (playerController.GetFacingRight()) {
                        respawnPlayerPos -= new Vector3(1f, 0f, 0f);
                    } else {
                        respawnPlayerPos += new Vector3(1f, 0f, 0f);
                    }

                    respawnSet = true;
                }
            }
        }
    }
}
