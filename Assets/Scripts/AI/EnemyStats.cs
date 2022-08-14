using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyStats : ScriptableObject
{
	public int id { get; set; }

	public new string name;

	public float health; 

	public bool ranged;

	public float speed;

	public int minMoneyReward = 5;

	public int maxMoneyReward = 10;

	public GameObject DeadPrefab;

	public GameObject EnemyPrefab;

	public void Getstats(EnemyStats enemy)
    {
		this.id = enemy.id;
		this.name = enemy.name;
		this.health = enemy.health;
		this.ranged = enemy.ranged;
		this.speed = enemy.speed;
		this.minMoneyReward = enemy.minMoneyReward;
		this.maxMoneyReward = enemy.maxMoneyReward;
		this.DeadPrefab = enemy.DeadPrefab;
		this.EnemyPrefab = enemy.EnemyPrefab;

	}
}
