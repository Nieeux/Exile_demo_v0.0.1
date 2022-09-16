using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoroutineManager : MonoBehaviour
{

	private void Awake()
	{
		CoroutineManager.instance = this;
	}

	public static Coroutine Start(IEnumerator routine)
	{
		return CoroutineManager.instance.StartCoroutine(routine);
	}

	public static void Stop(Coroutine routine)
	{
		if (routine != null)
		{
			CoroutineManager.instance.StopCoroutine(routine);
		}
	}

	public static Coroutine Delay(Action action, float duration)
	{
		return CoroutineManager.instance.StartCoroutine(CoroutineManager.DelaySequence(action, duration));
	}

	private static IEnumerator DelaySequence(Action action, float duration)
	{
		yield return new WaitForSeconds(duration);
		if (action != null)
		{
			action();
		}
		yield break;
	}

	public static IEnumerator SmoothAction(Action<float, float> action, float duration, bool unscaled = false)
	{
		float progress = 0f;
		float timer = 0f;
		while (progress < 1f)
		{
			action(progress, timer);
			timer += (unscaled ? Time.unscaledDeltaTime : Time.deltaTime);
			progress = timer / duration;
			yield return null;
		}
		action(1f, duration);
		yield break;
	}

	private static CoroutineManager instance;
}
