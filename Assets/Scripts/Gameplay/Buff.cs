using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Buff : ScriptableObject
{
	public int id;
	public new string name;
	public string nameViet;
	public string description;
	[Header("Visuals")]
	public Sprite sprite;
}
