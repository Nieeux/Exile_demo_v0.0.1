using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
	public static WorldSettings worldSettings { get; set; }
	public static GameManager Instance;
	public DayNightCycle dayNight;
	public int day = 0;
	public int Tomorrow = 0;
	public UnityAction<int> changeDay;

	private void Awake()
	{
		if (GameManager.Instance == null)
		{
			GameManager.Instance = this;
		}
		else if (GameManager.Instance != this)
		{
			Object.Destroy(this);
		}
		//Application.targetFrameRate = 144;
	}

	public static int GetSeed()
	{
		return GameManager.worldSettings.Seed;
	}

    private void Start()
    {
		
	}

    private void Update()
    {
		DayCycle();

	}

	private void DayCycle()
    {
		int n = (int)DayNightCycle.totalTime;
		day = 1 + n;
		

		if (day > Tomorrow)
		{
			DayUI(day);
			
		}
		

	}

	private void DayUI(int day)
    {
		DateUI.Instance.ShowDay(day);
		Tomorrow = day;
		Debug.Log("UpdateDay");
	}

	public void Relife()
    {
		NavMesh.RemoveAllNavMeshData();
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}
}