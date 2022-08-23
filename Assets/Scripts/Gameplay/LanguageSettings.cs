using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageSettings : MonoBehaviour
{
    public multiLanguage language;
    TextMeshProUGUI Text;
    MenuSetting changelanguage;
    void Start()
    {
        changelanguage = FindObjectOfType<MenuSetting>();
        changelanguage.ChangeLanguage += change;

        Text = GetComponent<TextMeshProUGUI>();
        Text.text = language.GetLanguage();
    }
    private void change()
    {
        Text.text = language.GetLanguage();
    }
}
