using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class TutorialLanguage : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject background;

    public GameObject mainmenu;

    public bool Show;
    public bool Showtutorial;

    public bool CanSelect;
    public float pad = 0;

    void Start()
    {

        if (PlayerPrefs.GetInt("tutorialLanguage") == 0)
        {
            PlayerPrefs.SetInt("tutorialLanguage", 1);

            Cursor.lockState = CursorLockMode.Locked;
            this.canvasGroup.alpha = 1f;
            Show = true;
            base.Invoke("canSelect", 1f);
            this.mainmenu.SetActive(false);

        }
        else
        {
            pad = 0;
            this.canvasGroup.alpha = 0f;
            this.background.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            this.mainmenu.SetActive(true);
        }

    }

    void Update()
    {
        if (CanSelect == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                MenuSetting.Instance.SetLanguage(false);
                CanSelect = false;
                Cursor.lockState = CursorLockMode.None;
                base.Invoke("fade", 1);
            }
            if (Input.GetMouseButtonDown(0))
            {
                MenuSetting.Instance.SetLanguage(true);
                CanSelect = false;
                Cursor.lockState = CursorLockMode.None;
                base.Invoke("fade", 1);
            }
        }


        if (Show == true)
        {
            this.pad = Mathf.Lerp(this.pad, 1, Time.deltaTime * 5f);
            this.canvasGroup.alpha = pad;
            this.background.gameObject.SetActive(true);
        }
        else if (pad > 0)
        {

            this.pad = Mathf.Lerp(this.pad, 0, Time.deltaTime * 5f);
            this.canvasGroup.alpha = pad;
            if (pad <= 0.1f)
            {
                this.background.gameObject.SetActive(false);
                pad = 0;
            }

        }
    }
    void canSelect()
    {
        CanSelect = true;

    }
    void fade()
    {
        Show = false;
        base.Invoke("fadeopen", 0.5f);
    }
    void fadeopen()
    {
        this.mainmenu.SetActive(true);
    }
}
