using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Handler : MonoBehaviour {

    [SerializeField]
    private GameObject enemyPrefab;

    private List<EnemyBehaviour> enemies;

    private void Awake() {
        enemies = new List<EnemyBehaviour>();

        for (int i = 0; i < 2; i++) {
            enemies.Add(Instantiate(enemyPrefab, this.transform).GetComponent<EnemyBehaviour>());
            enemies[i].transform.position = new Vector3(4 + (i * 2), 0, enemies[i].transform.position.z);
        }

        enemies[0].InitEnemy("Flower", 0.0f);
        enemies[1].InitEnemy("Masked Doctor", 4.0f);
    }

    private void Start() {
        
    }

    private void Update() {

    }

    private void SpawnEnemies() {

    }
}
