using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Buff : ScriptableObject
{
	public int id;
	public new string name;
	public string nameViet;
	public string descriptionViet;

	public string English;
	public string description;
	[Header("Visuals")]
	public Sprite sprite;


	public string GetDescription()
	{
		if (PlayerPrefs.GetInt("Language") == 1)
		{
			return this.nameViet + "\n<size=70%>" + this.descriptionViet;
		}
		return this.English + "\n<size=70%>" + this.description;
	}
}
