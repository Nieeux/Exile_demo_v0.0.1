using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameSettings : MonoBehaviour
{
	public void ReturnToMenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(0);
	}
	public void Play()
	{

		base.Invoke("StartPlay", 1f);
		LoadingScenes.Instance.Show = true;

	}
	private void StartPlay()
	{
		SceneManager.LoadScene(1);

	}

	public void ExitGame()
	{
		Application.Quit(0);
	}
}
