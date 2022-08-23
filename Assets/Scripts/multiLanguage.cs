using UnityEngine;

[CreateAssetMenu]
public class multiLanguage : ScriptableObject
{
	public string English;
	public string VietNamese;

	public string GetLanguage()
	{
        if (PlayerPrefs.GetInt("Language") == 1)
        {
			return this.VietNamese.ToString();
		}
		return this.English.ToString();
	}
}
