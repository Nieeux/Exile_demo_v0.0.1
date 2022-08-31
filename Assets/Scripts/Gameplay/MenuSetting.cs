using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Events;
using TMPro;

public class MenuSetting : MonoBehaviour
{
	public static MenuSetting Instance;
	public bool targetFrame;

	[SerializeField]
	private AudioMixer audioMixer;
	private float currentVolume;
	[SerializeField]
	private Slider volumeSlider;

	private int GraphicIndex;

	[SerializeField]
	private Toggle fullscreenToggle;

	[SerializeField]
	private Toggle vsynctoggle;
	public TMP_Dropdown GraphicQuality;

	public TMP_Dropdown resolutionDropdown;

	private float mouseSensitivity;
	[SerializeField]
	private Slider SensSlider;

	[SerializeField]
	private Toggle LanguageToggle;
	public UnityAction ChangeLanguage;

	private Resolution[] resolutions;

	private void Awake()
    {
		MenuSetting.Instance = this;
		if(targetFrame == true)
        {
			Application.targetFrameRate = 60;
		}

	}
    private void Start()
    {
		//Volume
		this.audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("VolumePreference"));

		if (PlayerPrefs.HasKey("VolumePreference"))
		{
			this.volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
		}
		else
		{
			this.volumeSlider.value = -10f;
		}
		//language
		bool isOn = PlayerPrefs.GetInt("fullscreen", 1) == 1;
		this.fullscreenToggle.isOn = isOn;
		//vsyn
		//bool isVsync = PlayerPrefs.GetInt("Vsync") == 1;
		//this.vsynctoggle.isOn = isVsync;
		//FullScreen
		bool isViet = PlayerPrefs.GetInt("Language", 1) == 1;
		this.LanguageToggle.isOn = isViet;
		//Graphics
		this.GraphicIndex = QualitySettings.GetQualityLevel();
		int graphic = PlayerPrefs.GetInt("Graphics");
		SetQuality(graphic);
		this.GraphicQuality.value = graphic;
		//Resolutions
		this.resolutions = Screen.resolutions;
		this.resolutionDropdown.ClearOptions();

		if (PlayerPrefs.HasKey("MouseSensitivity"))
		{
			this.SensSlider.value = PlayerPrefs.GetFloat("MouseSensitivity");
		}
		else
		{
			this.SensSlider.value = 2f;
			SetMouseSensitivity(2f);
		}

		List<string> list = new List<string>();
		int value = 0;
		for (int i = 0; i < this.resolutions.Length; i++)
		{
			string item = string.Concat(new string[]
			{
				this.resolutions[i].width.ToString(),
				" x ",
				this.resolutions[i].height.ToString(),
				" ",
				this.resolutions[i].refreshRate.ToString(),
				"Hz"
			});
			list.Add(item);
			if (this.resolutions[i].width == Screen.width && this.resolutions[i].height == Screen.height)
			{
				value = i;
			}
		}

		this.resolutionDropdown.AddOptions(list);
		this.resolutionDropdown.value = value;
		this.resolutionDropdown.RefreshShownValue();
	}
	/*
	public void Vsync(bool vsync)
	{
		if (vsync)
		{
			QualitySettings.vSyncCount = 1;
		}
		else
		{
			QualitySettings.vSyncCount = 0;
		}
		PlayerPrefs.SetInt("Vsync", vsync ? 1 : 0);
	}
	*/
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

	public void SetQuality (int QualityIndex)
    {
        if (QualitySettings.GetQualityLevel() == QualityIndex)
        {
			return;
        }
        QualitySettings.SetQualityLevel(QualityIndex);
		GraphicIndex = QualityIndex;
		PlayerPrefs.SetInt("Graphics", QualityIndex);
	}

	public void SetLanguage(bool Viet)
	{
		if (Viet)
		{
			PlayerPrefs.SetInt("Language", 1);
			if (ChangeLanguage != null)
			{
				ChangeLanguage.Invoke();
			}
		}
		else
		{
			PlayerPrefs.SetInt("Language", 0);
			if (ChangeLanguage != null)
			{
				ChangeLanguage.Invoke();
			}
		}
	}
	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = this.resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}
	public void SetVolume(float volume)
	{
		this.audioMixer.SetFloat("volume", volume);

		this.currentVolume = volume;
		PlayerPrefs.SetFloat("VolumePreference", this.currentVolume);
	}
	public void SetMouseSensitivity(float value)
	{
		this.mouseSensitivity = value;
		if(PlayerMovement.Instance != null)
        {
			PlayerMovement.Instance.SetMouseSensitivity(this.mouseSensitivity);
		}
		PlayerPrefs.SetFloat("MouseSensitivity", this.mouseSensitivity);
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

	private void Startplay()
	{
		GameManager.Instance.StartPlay();
	}
	public void ExitGame()
	{
		Application.Quit(0);
	}
}
