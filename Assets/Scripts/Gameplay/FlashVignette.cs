using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashVignette : MonoBehaviour
{

    public Image FlashImage;

    public CanvasGroup FlashCanvasGroup;
    public CanvasGroup VignetteCanvasGroup;
    public CanvasGroup SleepCanvasGroup;

    public Color DamageFlashColor;
    public Color HealFlashColor;

    public float CriticaHealthVignetteMaxAlpha = .8f;
    public float PulsatingVignetteFrequency = 4f;
    public float DamageFlashDuration;

    public float DamageFlashMaxAlpha = 1f;

    bool m_FlashActive;
    float m_LastTimeFlashStarted = Mathf.NegativeInfinity;

    public PlayerStats player;

    private void Start()
    {
        if(player == null)
        {
            player = FindObjectOfType<PlayerStats>();
        }
       
        player.OnDamaged += OnTakeDamage;
    }

    void Update()
    {
        if (player.GetHealthRatio()<= 0.3f)
        {
            VignetteCanvasGroup.gameObject.SetActive(true);
            float vignetteAlpha =
                (1 - (player.GetHealthRatio() / 0.3f)) * CriticaHealthVignetteMaxAlpha;

            if (player.PlayerDead())
                VignetteCanvasGroup.alpha = vignetteAlpha;
            else
                VignetteCanvasGroup.alpha =((Mathf.Sin(Time.time * PulsatingVignetteFrequency) / 2) + 0.5f) * vignetteAlpha;
        }
        else
        {
            VignetteCanvasGroup.gameObject.SetActive(false);
        }

        if (player.GetSleepyRatio() <= 0.25f)
        {
            SleepCanvasGroup.gameObject.SetActive(true);
            float vignetteAlpha =
                (1 - (player.GetSleepyRatio() / 0.25f)) * CriticaHealthVignetteMaxAlpha;

            if (player.PlayerDead())
                SleepCanvasGroup.alpha = vignetteAlpha;
            else
                SleepCanvasGroup.alpha = ((Mathf.Sin(Time.time * PulsatingVignetteFrequency) / 2) + 0.5f) * vignetteAlpha;
        }
        else
        {
            SleepCanvasGroup.gameObject.SetActive(false);
        }

        if (m_FlashActive)
        {
            float normalizedTimeSinceDamage = (Time.time - m_LastTimeFlashStarted) / DamageFlashDuration;

            if (normalizedTimeSinceDamage < 1f)
            {
                float flashAmount = DamageFlashMaxAlpha * (1f - normalizedTimeSinceDamage);
                FlashCanvasGroup.alpha = flashAmount;
            }
            else
            {
                FlashCanvasGroup.gameObject.SetActive(false);
                m_FlashActive = false;
            }
        }
    }

    void ResetFlash()
    {
        m_LastTimeFlashStarted = Time.time;
        m_FlashActive = true;
        FlashCanvasGroup.alpha = 0f;
        FlashCanvasGroup.gameObject.SetActive(true);
    }

    void OnTakeDamage(float dmg)
    {
        ResetFlash();
        FlashImage.color = DamageFlashColor;
    }

    void OnHealed(float amount)
    {
        ResetFlash();
        FlashImage.color = HealFlashColor;
    }
}
