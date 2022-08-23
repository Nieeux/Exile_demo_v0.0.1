using UnityEngine;
using UnityEngine.Events;

public class DaySystem : MonoBehaviour
{
	public static DaySystem Instance;
	public int day = 0;
	public int Tomorrow = 0;
	public bool isnight;
	public UnityAction NextDay;

	private void Awake()
	{
		if (DaySystem.Instance == null)
		{
			DaySystem.Instance = this;
		}
		else if (DaySystem.Instance != this)
		{
			Object.Destroy(this);
		}
		//Application.targetFrameRate = 144;
	}

    private void Start()
    {
		
	}

    private void Update()
    {
		DayCycle();

	}

	private void DayCycle()
    {
		int n = (int)DayNightCycle.Instance.GetCurrentTime();
		day = 1 + n;

		if (day > Tomorrow)
		{
			DayUI(day);
		}

	}

	private void DayUI(int day)
    {
		DateUI.Instance.ShowDay(day);
		if (NextDay != null)
		{
			NextDay.Invoke();
		}
		Tomorrow = day;
	}

}