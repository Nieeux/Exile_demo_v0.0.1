using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
	public static WorldSettings worldSettings { get; set; }
	public static GameManager Instance;
	public GameObject resourceGen;

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
	public void Relife()
    {
		NavMesh.RemoveAllNavMeshData();
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}
}