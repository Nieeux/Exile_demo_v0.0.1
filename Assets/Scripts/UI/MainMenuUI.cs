using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;



public class MainMenuUI : MonoBehaviour
{
	public GameObject MenuMain;
	public CanvasGroup canvasGroup;
	public GameManager gameManager;
	public bool Show;
	public float pad = 0;

	private void Awake()
    {
		

	}
    private void Start()
    {
		if (PlayerPrefs.GetInt("tutorialLanguage") == 0)
		{
			pad = 0;
			this.canvasGroup.alpha = 0f;
		}
        else
        {
			base.Invoke("fade", 1);
		}

		if (gameManager == null)
        {
			gameManager = FindObjectOfType<GameManager>();
		}
	}

    private void Update()
    {
		if (Show == true)
		{
			this.pad = Mathf.Lerp(this.pad, 1, Time.deltaTime * 5f);
			this.canvasGroup.alpha = pad;
			this.MenuMain.gameObject.SetActive(true);
			base.Invoke("fadeoff", 1);

		}
		else if (pad > 0)
		{

			this.pad = Mathf.Lerp(this.pad, 0, Time.deltaTime * 5f);
			this.canvasGroup.alpha = pad;
			if (pad <= 0.1f)
			{
				this.MenuMain.gameObject.SetActive(false);
				pad = 0;
			}

		}
	}
	void fade()
	{
		Show = true;
	}

	void fadeoff()
	{
		Destroy(this);
	}


}
