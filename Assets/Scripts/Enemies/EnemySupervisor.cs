using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySupervisor : MonoBehaviour {
    public List<GameObject> spawnPoints;
    public GameObject enemyPrefab;
    private List<GameObject> enemies;
    private int enemiesAlive = 0;

    public delegate void  EnemyCountHandler();
    public static EnemyCountHandler EnemiesDead;
	
	void Start () {
        enemies = new List<GameObject>();
        foreach (GameObject spawnpoint in spawnPoints)
        {
            GameObject enemy = GameObject.Instantiate(enemyPrefab, spawnpoint.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.SetParent(this.transform);
            enemies.Add(enemy);
            enemy.GetComponent<ChargeHolder>().Overcharged += OnEnemyDeath;
            enemiesAlive++;
        }
	}
	
	private void OnEnemyDeath()
    {
        enemiesAlive--;
        if (enemiesAlive == 0)
        {
            EnemiesDead();
        }
    }

    void Update () {
	
	}
}
