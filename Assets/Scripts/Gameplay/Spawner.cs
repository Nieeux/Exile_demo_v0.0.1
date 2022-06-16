using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
	// Token: 0x0600010B RID: 267 RVA: 0x0000A418 File Offset: 0x00008818
	private void Start()
	{
		if (this.Mountains)
		{
			this.Objects = Resources.LoadAll<UnityEngine.Object>("Cube");
			this.ammount = 1;
			this.RadiusX = 50f;
			this.Y = 0.1839f;
			this.RadiusZ = 50f;
			this.RotX = -90f;
			this.RandomRotZ = true;
			Debug.Log("Spawn");
		}
		base.StartCoroutine(this.Check());
		base.StartCoroutine(this.Spawning());
	}

	private IEnumerator Check()
	{
		yield return new WaitForSeconds(3f);
		if (base.gameObject.transform.parent.GetComponent<MeshCollider>().sharedMesh == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		yield break;
	}

	private IEnumerator Spawning()
	{
		yield return new WaitForSeconds(1f);
		YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
		while (this.i < this.ammount)
		{
			if (this.RandomRotY)
			{
				this.RotY = (float)UnityEngine.Random.Range(0, 360);
			}
			if (this.RandomRotZ)
			{
				this.RotZ = (float)UnityEngine.Random.Range(0, 360);
			}
			this.Object = (GameObject)UnityEngine.Object.Instantiate(this.Objects[UnityEngine.Random.Range(0, this.Objects.Length)], new Vector3(base.transform.position.x + UnityEngine.Random.insideUnitCircle.x * this.RadiusX, base.transform.position.y + this.Y, base.transform.position.z + UnityEngine.Random.insideUnitCircle.x * this.RadiusZ), Quaternion.Euler(this.RotX, this.RotY, this.RotZ), base.transform);
			this.Object.isStatic = true;
			this.Object.tag = this.Tag;
			this.i++;
			if (this.i % 10 == 0)
			{
				yield return waitForFixedUpdate;
			}
		}
		yield break;
	}

	private void Update()
	{
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.position, 45f);
	}

	public UnityEngine.Object[] Objects;

	private GameObject Object;

	public bool Mountains;

	public bool Trees;

	private int ammount;

	private float RadiusX;

	private float Y;

	private float RadiusZ;

	private float RotX;

	private float RotY;

	private float RotZ;

	private bool RandomRotY;

	private bool RandomRotZ;

	private string Tag;

	private int i;
}
