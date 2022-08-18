using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;



public class MainMenuUI : MonoBehaviour
{
	private float currentSens;
	public GameObject MenuMain;
	public GameObject MenuSetting;
	public GameObject MenuAboutMe;
	GameManager gameManager;
	private Resolution[] resolutions;

    private void Awake()
    {
		

	}
    private void Start()
    {
		gameManager = FindObjectOfType<GameManager>();
		MenuSetting.SetActive(false);

		if(MenuAboutMe != null)
        {
			MenuAboutMe.SetActive(false);
		}

	}



	public void Play()
	{

		base.Invoke("Startplay", 3f);
		LoadingScenes.Instance.Show = true;

	}

	public void OpenFacebook()
    {
		Application.OpenURL("https://www.facebook.com/Lieeux");
    }
	public void OpenYoutube()
	{
		Application.OpenURL("https://www.youtube.com/c/salix_lieeux");
	}
	public void OpenTwitter()
	{
		Application.OpenURL("https://twitter.com/Salix_Lieeux");
	}

	public void SetSens(float sens)
	{
		if (!this.MenuSetting)
		{
			PlayerMovement.Instance.Sensitivity = sens;
		}
		this.currentSens = sens;
		PlayerPrefs.SetFloat("Sensitivity", this.currentSens);
	}

	public void SetFullscreen(bool isFullscreen)
	{
		if (isFullscreen)
		{
			Resolution[] array = Screen.resolutions;
			Resolution resolution = array[array.Length - 1];
			Screen.SetResolution(resolution.width, resolution.height, true);
		}
		else
		{
			Resolution[] array2 = Screen.resolutions;
			Resolution resolution2 = array2[array2.Length - 1];
			Screen.SetResolution(resolution2.width, resolution2.height, false);
		}
		PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = this.resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}
	private void Startplay()
	{

		gameManager.StartPlay();

	}

	public void ExitGame()
	{
		Application.Quit(0);
	}
}
