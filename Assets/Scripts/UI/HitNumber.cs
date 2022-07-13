using TMPro;
using UnityEngine;

public class HitNumber : MonoBehaviour
{

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

	public void SetTextAndDir(float damage, Vector3 dir, HitEffect hitEffect)
	{
		this.hitDir = -dir;
		string colorName = HitEffectExtension.GetColorName(hitEffect);
		this.text.text = string.Concat(new object[]
		{
			"<color=",
			colorName,
			">",
			damage
		});
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

	private TextMeshProUGUI text;

	// Token: 0x04000206 RID: 518
	private float speed = 10f;

	// Token: 0x04000207 RID: 519
	private Vector3 defaultScale;

	// Token: 0x04000208 RID: 520
	private Vector3 dir;

	// Token: 0x04000209 RID: 521
	private Vector3 hitDir;
}