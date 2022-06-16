using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static WorldSettings worldSettings { get; set; }
	public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
	public static GameManager instance;
	public GameObject resourceGen;
	//private float Delay = 2;

	void Start()
	{
		//base.StartCoroutine(this.GenerateWorldRoutine());
	}

	private void Awake()
	{
		if (GameManager.instance == null)
		{
			GameManager.instance = this;
		}
		else if (GameManager.instance != this)
		{
			Object.Destroy(this);
		}
	}

	public static int GetSeed()
	{
		return GameManager.worldSettings.Seed;
	}

	//private IEnumerator GenerateWorldRoutine()
	//{
		/*
		yield return 3f;
		Debug.Log("Generating World");
		this.resourceGen.SetActive(true);
		yield return new WaitForSeconds(this.Delay);
		Debug.Log("Generating resources");
		this.resourceGen.SetActive(true);
		yield return 60;
		Debug.Log("Finished loading");
		yield break;
		*/
	//}
	//void Update()
	//{

	//}
}