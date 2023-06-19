using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

public class Platforming_Stage_Handler : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Enemy_Handler enemyHandler;

    private Transform playerTrans;
    private Vector3 respawnPlayerPos;
    private Rigidbody2D playerRB;
    private bool respawnSet;

    long invincibilityTimer;

    private CharacterController2D playerController;

    private float yLimit = -10f;

    private void Awake() {
        playerTrans = player.transform;
        respawnSet = false;

        invincibilityTimer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + 3000;
    }

    private void Start() {
        playerController = player.GetComponent<CharacterController2D>();
        playerController.GetTriggerCollider().enabled = false;

        playerRB = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckRespawn();
        if (respawnSet) {
            CheckPlayerFall();
        }

        DoInvincibilityFrames();
    }

    private void CheckPlayerFall() {
        if (playerTrans.position.y < yLimit) {
            playerTrans.position = respawnPlayerPos;
            playerRB.velocity = new Vector3(0f, 0f, 0f);

            playerController.GetTriggerCollider().enabled = false;
            invincibilityTimer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + 3000;
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

    private void DoInvincibilityFrames() {
        if (invincibilityTimer < DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) {
            playerController.GetTriggerCollider().enabled = true;
            playerController.ResetSpriteTransparency();
        }
    }

    public void AddFirstBatchOfKana() {
        Utils.AddKanaData("あ");
        Utils.AddKanaData("い");
        Utils.AddKanaData("う");
        Utils.AddKanaData("え");
        Utils.AddKanaData("お");
        Utils.SaveKanaData();
    }
}
