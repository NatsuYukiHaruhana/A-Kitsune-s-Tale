using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforming_Stage_Handler : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private Transform playerTrans;
    private Vector3 respawnPlayerPos;
    private bool respawnSet;

    private CharacterController2D playerController;

    private float yLimit = -10f;

    private void Awake() {
        playerTrans = player.transform;
        respawnSet = false;
    }

    private void Start() {
        playerController = player.GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        CheckPlayerFall();
        CheckPitAhead();
    }

    private void CheckPlayerFall() {
        if (playerTrans.position.y < yLimit) {
            playerTrans.position = respawnPlayerPos;
        }
    }

    private void CheckPitAhead() {
        if (playerController.GetGrounded()) {
            if (playerController.GetPitAhead()) {
                respawnPlayerPos = playerTrans.position;
                respawnSet = true;

                if (playerController.GetFacingRight()) {
                    respawnPlayerPos -= new Vector3(1f, 0f, 0f);
                } else {
                    respawnPlayerPos += new Vector3(1f, 0f, 0f);
                }
            } else {
                respawnSet = false;
            }
        } else {
            if (!respawnSet) {
                respawnPlayerPos = playerTrans.position;
                respawnSet = true;
            }
        }
    }
}
