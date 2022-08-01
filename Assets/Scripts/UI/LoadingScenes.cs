using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScenes : MonoBehaviour
{
    public static LoadingScenes Instance;
    public CanvasGroup canvasGroup;
    public RawImage background;

    public bool Show;

    public float totalFadeTime { get; set; } = 1f;

    public float pad = 0;

    private void Awake()
    {
        LoadingScenes.Instance = this;
        this.canvasGroup.alpha = 0f;
        this.background.gameObject.SetActive(false);

    }

    void Start()
    {
        
    }
    void Update()
    {

        if(Show == true)
        {
            this.pad = Mathf.Lerp(this.pad, 1, Time.deltaTime * 20f);
            this.canvasGroup.alpha = pad;
            this.background.gameObject.SetActive(true);
        }
        else
        {
            this.pad = Mathf.Lerp(this.pad, 0, Time.deltaTime * 20f);
            this.canvasGroup.alpha = pad;
            if (pad == 0)
            {
                this.background.gameObject.SetActive(false);
            }

        }
    }

}
