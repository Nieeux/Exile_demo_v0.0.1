using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBumble : MonoBehaviour
{
	[SerializeField]
	private Vector3 positionStrength;

	[SerializeField]
	private Vector3 rotationStrength;

	[SerializeField]
	private float rumbleSpeed;

	private Vector3 defuLocalPosition;

	private Quaternion defuLocalRotation;

	private float perlinX;

	private float perlinY;

	private void Start()
	{
		this.defuLocalPosition = base.transform.localPosition;
		this.defuLocalRotation = base.transform.localRotation;
		this.perlinX = Random.Range(-10000f, 10000f);
		this.perlinY = Random.Range(-10000f, 10000f);
	}


	private void Update()
	{
		this.perlinX += Time.deltaTime * this.rumbleSpeed;
		this.perlinY += Time.deltaTime * this.rumbleSpeed;
		Vector3 zero = Vector3.zero;
		zero.x = (Mathf.PerlinNoise(this.perlinX, 0f) * 2f - 1f) * this.positionStrength.x;
		zero.y = (Mathf.PerlinNoise(0f, this.perlinY) * 2f - 1f) * this.positionStrength.y;
		zero.z = (Mathf.PerlinNoise(this.perlinY, 0f) * 2f - 1f) * this.positionStrength.z;
		base.transform.localPosition = this.defuLocalPosition + zero;
		Vector3 zero2 = Vector3.zero;
		zero2.x = (Mathf.PerlinNoise(this.perlinY, 0f) * 2f - 1f) * this.rotationStrength.x;
		zero2.y = (Mathf.PerlinNoise(0f, this.perlinX) * 2f - 1f) * this.rotationStrength.y;
		zero2.z = (Mathf.PerlinNoise(this.perlinX, 0f) * 2f - 1f) * this.rotationStrength.z;
		base.transform.localRotation = Quaternion.Euler(this.defuLocalRotation.eulerAngles + zero2);
	}

}
