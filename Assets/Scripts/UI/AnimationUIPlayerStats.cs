using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationUIPlayerStats : MonoBehaviour
{
    private VerticalLayoutGroup layout;
    public GameObject bar;
    public float padBottom = 0;

    public void Awake()
    {
        this.layout = base.GetComponent<VerticalLayoutGroup>();
    }
    public void stats()
    {
        bar.SetActive(false);
    }

    void Update()
    {
        if (StatusUI.Instance.IsShowStatus == true)
        {
            bar.SetActive(true);
            this.padBottom = Mathf.Lerp(this.padBottom, 70, Time.deltaTime * 20f);
            RectOffset rectOffset = new RectOffset(this.layout.padding.left, this.layout.padding.right, this.layout.padding.top, this.layout.padding.bottom);
            rectOffset.bottom = (int)this.padBottom;
            this.layout.padding = rectOffset;
            this.layout.padding.bottom = (int)this.padBottom;
        }
        else
        {
            //Khong cho update thuong xuyen
            if (padBottom > 0)
            {
                this.padBottom = (int)Mathf.Lerp(this.padBottom, 0, Time.deltaTime * 20f);
                RectOffset rectOffset = new RectOffset(this.layout.padding.left, this.layout.padding.right, this.layout.padding.top, this.layout.padding.bottom);
                rectOffset.bottom = (int)this.padBottom;
                this.layout.padding = rectOffset;
                this.layout.padding.bottom = (int)this.padBottom;
                bar.SetActive(false);
            }
        }

    }

}
