using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public GameObject All;

    [Header("Move")]
    public GameObject Move;
    public Image moveicon;
    public TextMeshProUGUI dichuyen;
    public TextMeshProUGUI nhay;
    public TextMeshProUGUI chay;
    public TextMeshProUGUI ngoi;

    [Header("Interact")]
    public GameObject Interact;
    public Image InteractIcon;
    public Image SwitchIcon;
    public TextMeshProUGUI nemvukhi;
    public TextMeshProUGUI tuongtac;
    public TextMeshProUGUI ruongdo;
    public TextMeshProUGUI doivukhi;

    void Start()
    {
        Move.SetActive(false);
        Interact.SetActive(false);

        if (PlayerPrefs.GetInt("tutorial") == 0)
        {
            PlayerPrefs.SetInt("tutorial", 1);

            base.Invoke("Enable", 4f);

        }
        else
        {
            Destroy(All); 
            Destroy(this);
        }
    }
    private void Enable()
    {
        Move.SetActive(true);
        base.Invoke("fade", 5f);
    }
    private void fade()
    {
        this.moveicon.CrossFadeAlpha(0f, 1f, true);
        this.dichuyen.CrossFadeAlpha(0f, 1f, true);
        this.nhay.CrossFadeAlpha(0f, 1f, true);
        this.chay.CrossFadeAlpha(0f, 1f, true);
        this.ngoi.CrossFadeAlpha(0f, 1f, true);
        base.Invoke("interact", 1f);
    }
    private void interact()
    {
        Interact.SetActive(true);
        base.Invoke("fadeinter", 5f);
    }
    private void fadeinter()
    {
        this.InteractIcon.CrossFadeAlpha(0f, 1f, true);
        this.SwitchIcon.CrossFadeAlpha(0f, 1f, true);
        this.tuongtac.CrossFadeAlpha(0f, 1f, true);
        this.ruongdo.CrossFadeAlpha(0f, 1f, true);
        this.nemvukhi.CrossFadeAlpha(0f, 1f, true);
        this.doivukhi.CrossFadeAlpha(0f, 1f, true);

    }
}
