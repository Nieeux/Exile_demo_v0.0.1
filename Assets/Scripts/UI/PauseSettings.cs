using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseSettings : MonoBehaviour
{
	public void ReturnToMenu()
	{
		SceneManager.LoadScene(0);
	}
}
