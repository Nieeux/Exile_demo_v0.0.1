using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour
{
	public static DayNightCycle Instance;
	public Light sun;
	public float CurrentTime;

	public float dayDuration = 1f;
	public float NightDuration = 0.25f;

	[SerializeField]
	private float timeSpeed = 0.01f;
	[SerializeField]
	private bool Day;

    [Range(0,1)]
	public float currentTimeOfDay = 0.3f;
	public float timeMultiplier = 1f;

	[Header("fog")]
	public Gradient nightDayFogColor;
	public AnimationCurve fogDensityCurve;

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

	public float ingameTime { get; private set; }

	[SerializeField]
	private float lengthOfDayrealtime = 4;
	float passedTime; 

    private void Awake()
    {
		DayNightCycle.Instance = this;
    }
	private void Start()
	{
		sun = GetComponent<Light>();
		skyMat = RenderSettings.skybox;

	}
	private void Update()
	{
		UpdatePosition();
		UpdateFX();

		float n = 1f * timeSpeed / dayDuration;
		if(currentTimeOfDay < 0.3f || currentTimeOfDay > 0.7f)
		{
			n /= NightDuration;
			Day = false;
		}
		else
        {
			Day = true;
		}

		float n2 = n * Time.deltaTime;
		currentTimeOfDay += n2;
		CurrentTime += n2;


		if (currentTimeOfDay >= 1)
		{
			currentTimeOfDay = 0;

		}
	}

	public void NewDay()
	{
        if (Day)
        {
			int n = (int)(CurrentTime + 0.5f);
			currentTimeOfDay = 0.7f;
			CurrentTime = 0.4f + n;
			return;
		}
        else
        {
			int n = (int)(CurrentTime + 0.7f);
			currentTimeOfDay = 0.3f;
			CurrentTime = n;
			return;
		}
	}

	public float GetCurrentTime()
	{
		return CurrentTime;
	}
	public float GetcurrentTimeOfDay()
	{
		return currentTimeOfDay;
	}
	public bool IsDay()
	{
		if(Day == true)
        {
			return true;
		}
		return false;
	}

	void realtime()
    {
		passedTime = (passedTime + Time.deltaTime) % lengthOfDayrealtime;
		float t = Mathf.InverseLerp(0f, lengthOfDayrealtime, passedTime);
		ingameTime = Mathf.Lerp(0f, 24, t);

	}

	private void UpdatePosition()
	{
		sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);
	}

	private void UpdateFX()
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

		RenderSettings.fogColor = nightDayFogColor.Evaluate(dot);
		RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;

		i = ((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
		skyMat.SetFloat("_AtmosphereThickness", i);
		skyMat.SetFloat("_Exposure", i * exposureMultiplier);
		
	}

}