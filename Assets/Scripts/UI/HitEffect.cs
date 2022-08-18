using TMPro;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
	private TextMeshProUGUI text;
	private float speed = 10f;
	public Color Critical;
	public Color LowDmg;
	public Color NormalDmg;
	public Color HighDmg;

	private Vector3 defaultScale;
	private Vector3 dir;
	private Vector3 hitDir;

	private void Awake()
	{
		base.Invoke("StartFade", 1f);
		this.defaultScale = base.transform.localScale * 0.5f;
		this.text = base.GetComponentInChildren<TextMeshProUGUI>();
		float num = 0.5f;
		this.dir = new Vector3(Random.Range(-num, num), Random.Range(1f, 2f), Random.Range(-num, num));
	}

	private void Update()
	{
		this.speed = Mathf.Lerp(this.speed, 0.1f, Time.deltaTime * 20f);
		base.transform.position += (this.dir + this.hitDir) * Time.deltaTime * this.speed;
		base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.defaultScale * 0.5f, Time.deltaTime * 0.3f);
	}

	public void SetTextAndDir(float damage, Vector3 dir, HitType hitEffect)
	{
		this.hitDir = -dir;
		this.text.color = GetColor(hitEffect);
		this.text.text = damage.ToString("0");
	}

	private Color GetColor(HitType effect)
	{
		switch (effect)
		{
			case HitType.Low:
				return LowDmg;
			case HitType.Normal:
				return NormalDmg;
			case HitType.High:
				return HighDmg;
			case HitType.Critical:
				return Critical;
			default:
				return Color.white;
		}
	}

	private void StartFade()
	{
		this.text.CrossFadeAlpha(0f, 1f, true);
		base.Invoke("DestroySelf", 1f);
	}

	private void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}

	public enum HitType
	{
		Null,

		Low,

		Normal,

		High,

		Critical,
	}
}