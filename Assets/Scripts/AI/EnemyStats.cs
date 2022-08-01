using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyStats : ScriptableObject
{
	public int id { get; set; }

	public new string name;

	public bool ranged;

	public float speed;

	public int minMoneyReward = 5;

	public int maxMoneyReward = 10;

	public GameObject DeadPrefab;

	public GameObject EnemyPrefab;
}
