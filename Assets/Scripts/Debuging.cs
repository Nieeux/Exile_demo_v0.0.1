using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;


public class Debuging : MonoBehaviour
{
	private void Start()
	{
		Debuging.Instance = this;
	}

	private void Update()
	{
		string text = "";
		this.deltaTime += (Time.unscaledDeltaTime - this.deltaTime) * 0.1f;
		float num = this.deltaTime * 1000f;
		float num2 = 1f / this.deltaTime;
		text += string.Format("{0:0.0} ms | {1:0.} fps", num, num2);
		this.fps.text = text;
	}



	public TextMeshProUGUI fps;

	private float deltaTime;

	public static List<string> r = new List<string>();

	public static Debuging Instance;

}
