using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour
{
	public Light sun;
	public static float totalTime { get; private set; }
	public float dayDuration = 1f;
	public float NightDuration = 0.25f;
	public float timeSpeed = 0.001f;
	public bool Day;
	public bool Night;

	[Range(0, 1)]
	public float currentTimeOfDay = 0.45f;
	public float timeMultiplier = 1f;
	[Space(10)]
	public Gradient nightDayColor;
	public float maxIntensity = 2f;
	public float minIntensity = 0f;
	public float minPoint = -0.2f;
	public float maxBounceIntensity = 1.0f;
	public float minBounceIntensity = 0.5f;
	public float maxAmbient = 1f;
	public float minAmbient = 0f;
	public float minAmbientPoint = -0.2f;
	public float fogScale = 1f;
	public float exposureMultiplier = 1f;
	public float dayAtmosphereThickness = 0.4f;
	public float nightAtmosphereThickness = 0.87f;
	Material skyMat;

	void Start()
	{
		sun = GetComponent<Light>();
		skyMat = RenderSettings.skybox;

	}
	void Update()
	{
		UpdatePosition();
		UpdateFX();

		float n = 1f * timeSpeed / dayDuration;
		if(currentTimeOfDay < 0.3f || currentTimeOfDay > 0.7f)
		{
			n /= NightDuration;
			Night = true;
			Day = false;
		}
		else
        {
			Night = false;
			Day = true;
		}

		float n2 = n * Time.deltaTime;
		currentTimeOfDay += n2;
		totalTime += n2;


		if (currentTimeOfDay >= 1)
		{
			currentTimeOfDay = 0;

		}
	}

	void UpdateCycle()
	{

	}

	void UpdatePosition()
	{
		sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);
	}

	void UpdateFX()
	{
		float tRange = 1 - minPoint;
		float dot = Mathf.Clamp01((Vector3.Dot(sun.transform.forward, Vector3.down) - minPoint) / tRange);
		float i = ((maxIntensity - minIntensity) * dot) + minIntensity;
		sun.intensity = i;

		i = ((maxBounceIntensity - minBounceIntensity) * dot) + minBounceIntensity;
		sun.bounceIntensity = i;

		tRange = 1 - minAmbientPoint;
		dot = Mathf.Clamp01((Vector3.Dot(sun.transform.forward, Vector3.down) - minAmbientPoint) / tRange);
		i = ((maxAmbient - minAmbient) * dot) + minAmbient;
		RenderSettings.ambientIntensity = i;

		sun.color = nightDayColor.Evaluate(dot);
		RenderSettings.ambientLight = sun.color;

		i = ((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
		skyMat.SetFloat("_AtmosphereThickness", i);
		skyMat.SetFloat("_Exposure", i * exposureMultiplier);
		
	}
}