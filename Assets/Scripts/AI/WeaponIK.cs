using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIK : MonoBehaviour
{
	[SerializeField]
	private Transform targetTransform;

	[SerializeField]
	private Transform aimTransform;

	[SerializeField]
	private Transform bone;

	public int iterations = 10;

	public float angleLimit = 90f;

	public float distanceLimit = 1.5f;

	public Vector3 targetOffset;

	void Start()
    {
        
    }

    void LateUpdate()
    {

		if (this.aimTransform == null)
		{
			return;
		}
		if (this.targetTransform == null)
		{
			return;
		}
		Vector3 targetPosition = this.GetTargetPosition();
		for (int i = 0; i < this.iterations; i++)
		{
			this.AimAtTarget(bone, targetPosition, 1);
		}
	}

	private Vector3 GetTargetPosition()
	{
		Vector3 vector = this.targetTransform.position + this.targetOffset - this.aimTransform.position;
		Vector3 forward = this.aimTransform.forward;
		float num = 0f;
		float num2 = Vector3.Angle(vector, forward);
		if (num2 > this.angleLimit)
		{
			num += (num2 - this.angleLimit) / 50f;
		}
		float magnitude = vector.magnitude;
		if (magnitude < this.distanceLimit)
		{
			num += this.distanceLimit - magnitude;
		}
		Vector3 b = Vector3.Slerp(vector, forward, num);
		return this.aimTransform.position + b;
	}

	private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
	{
		Vector3 forward = this.aimTransform.forward;
		Vector3 toDirection = targetPosition - this.aimTransform.position;
		Quaternion b = Quaternion.FromToRotation(forward, toDirection);
		Quaternion lhs = Quaternion.Slerp(Quaternion.identity, b, weight);
		bone.rotation = lhs * bone.rotation;
	}

	public void SetTargetTransform(Transform target)
	{
		this.targetTransform = target;
	}

	public void SetAimTransform(Transform aim)
	{
		this.aimTransform = aim;
	}

}
