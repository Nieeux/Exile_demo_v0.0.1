using System;
using UnityEngine;


public class Billboard : MonoBehaviour
{

	void Start()
	{

	}

	void Update()
	{

		transform.localEulerAngles = new Vector3(0f, PlayerMovement.Instance.playerCamera.transform.eulerAngles.y, 0f);

	}


}