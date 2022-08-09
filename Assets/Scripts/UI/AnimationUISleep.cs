using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationUISleep : MonoBehaviour
{
    private HorizontalLayoutGroup layout;
    public GameObject bar;
    public float padleft = 0;
    public void Awake()
    {
        this.layout = base.GetComponent<HorizontalLayoutGroup>();
    }

    void Start()
    {
        bar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (StatusUI.Instance.IsShowStatus == true)
        {
            bar.SetActive(true);
            this.padleft = Mathf.Lerp(this.padleft, -80, Time.deltaTime * 20f);
            RectOffset rectOffset = new RectOffset(this.layout.padding.left, this.layout.padding.right, this.layout.padding.top, this.layout.padding.bottom);
            rectOffset.left = (int)this.padleft;
            this.layout.padding = rectOffset;
            this.layout.padding.left = (int)this.padleft;
        }
        else if (padleft < 0)
        {
            this.padleft = (int)Mathf.Lerp(this.padleft, 0, Time.deltaTime * 20f);
            RectOffset rectOffset = new RectOffset(this.layout.padding.left, this.layout.padding.right, this.layout.padding.top, this.layout.padding.bottom);
            rectOffset.left = (int)this.padleft;
            this.layout.padding = rectOffset;
            this.layout.padding.left = (int)this.padleft;
            base.Invoke("TurnOff", 0.1f);
        }
    }
    void TurnOff()
    {
        bar.SetActive(false);
    }
}
