using System;
using PCGSharp;
using UnityEngine;

public class Randomizer
{
	public Randomizer()
	{
		this.SetSeed(1234);
	}

	public Randomizer(int seed)
	{
		this.SetSeed(seed);
	}

	public void SetSeed(int seed)
	{
		this.seed = seed;
		this.pcg = new PcgExtended(seed);
	}

	public int GetInt()
	{
		return this.pcg.Next();
	}

	public int GetInt(int minIncusive, int maxExclusive)
	{
		return this.pcg.Next(minIncusive, maxExclusive);
	}

	public float GetFloat()
	{
		return this.pcg.NextFloat();
	}

	public float GetFloat(float minInclusive, float maxInclusive)
	{
		return this.pcg.NextFloat(minInclusive, maxInclusive);
	}

	public Vector3 GetPositionInsideSphere()
	{
		Vector3 result = default(Vector3);
		do
		{
			result.x = this.GetFloat(-1f, 1f);
			result.y = this.GetFloat(-1f, 1f);
			result.z = this.GetFloat(-1f, 1f);
		}
		while (result.sqrMagnitude > 1f);
		return result;
	}

	public Vector3 GetPositionOnSphere()
	{
		return this.GetPositionInsideSphere().normalized;
	}

	public Vector2 GetPositionInsideCircle()
	{
		Vector2 result;
		do
		{
			result.x = this.GetFloat(-1f, 1f);
			result.y = this.GetFloat(-1f, 1f);
		}
		while (result.sqrMagnitude > 1f);
		return result;
	}

	public Vector2 GetPositionInsideUnitCircle()
	{
		return this.GetPositionInsideCircle().normalized;
	}

	public bool GetBool(float probability)
	{
		probability = Mathf.Clamp01(probability);
		return this.GetFloat(0f, 1f) < probability;
	}

	public Vector3 GetDirectionInCone(Vector3 axis, float maxAngle)
	{
		float d = Mathf.Tan(0.017453292f * maxAngle / 2f);
		Vector2 vector = this.GetPositionInsideUnitCircle() * d;
		return (axis + Quaternion.FromToRotation(Vector3.forward, axis) * new Vector3(vector.x, vector.y, 0f)).normalized;
	}

	public Quaternion GetRotation()
	{
		return Quaternion.AngleAxis(this.GetFloat(0f, 360f), new Vector3(this.GetFloat(-1f, 1f), this.GetFloat(-1f, 1f), this.GetFloat(-1f, 1f)).normalized);
	}

	public T GetEnum<T>()
	{
		return (T)((object)this.GetInt(0, Enum.GetValues(typeof(T)).Length));
	}

	protected int seed;

	protected PcgExtended pcg;
}
