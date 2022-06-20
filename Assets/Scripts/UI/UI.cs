using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //public Texture crosshairTexture;

    public Image HealthBar;
    public Image HurtBar;

    private float HurtSpeed = 0.002f;
    public Transform playerCam;
    public Health m_PlayerHealth;

    void Start()
    {

    }
    void Update()
    {
        HealthBar.fillAmount = m_PlayerHealth.CurrentHealth / m_PlayerHealth.MaxHealth;
        if (HurtBar.fillAmount > HealthBar.fillAmount)
        {
            HurtBar.fillAmount -= HurtSpeed;
        }
        else
        {
            HurtBar.fillAmount = HealthBar.fillAmount;
        }
    }

    void OnGUI()
    {
        GUI.color = Color.red;
        //GUI.DrawTexture(new Rect(Screen.width / 2 - 3, Screen.height / 2 - 3, 6, 6), crosshairTexture);

        if (m_PlayerHealth.CurrentHealth <= 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 3, Screen.height / 2 - 3, 250, 25), "Game Over");
        }
        else
        {
            //GUI.DrawTexture(new Rect(Screen.width / 2 - 3, Screen.height / 2 - 3, 6, 6), crosshairTexture);
        }
    }
}

