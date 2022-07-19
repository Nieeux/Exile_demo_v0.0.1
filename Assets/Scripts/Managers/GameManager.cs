using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static WorldSettings worldSettings { get; set; }
	public static GameManager instance;
	public GameObject resourceGen;

	void Start()
	{

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
		Application.targetFrameRate = 144;
	}
	public void Play()
	{
		SceneManager.LoadScene(1);
	}
	public void ExitGame()
	{
		Application.Quit(0);
	}

	public static int GetSeed()
	{
		return GameManager.worldSettings.Seed;
	}
}