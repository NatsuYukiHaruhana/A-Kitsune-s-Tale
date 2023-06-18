using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Handler : MonoBehaviour {

    [SerializeField]
    private GameObject enemyPrefab;

    private List<EnemyBehaviour> enemies;

    private void Awake() {
        enemies = new List<EnemyBehaviour>();

        SpawnEnemies();
    }

    private void SpawnEnemies() {
        for (int i = 0; i < Utils.enemyNames.Count; i++) {
            enemies.Add(Instantiate(enemyPrefab, this.transform).GetComponent<EnemyBehaviour>());

            float moveSpeed = Utils.enemyNames[i] == "Flower" ? 0f : 5f;
            enemies[i].InitEnemy(Utils.enemyNames[i], moveSpeed, i);
            enemies[i].transform.position = Utils.enemyPos[i];
        }

        if (Utils.enemyToBattleIndex != -1) {
            enemies[Utils.enemyToBattleIndex].SetRespawn();
        }
    }
}
