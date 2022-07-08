using System;
using UnityEngine;

// Token: 0x020000E4 RID: 228
internal static class HitEffectExtension
{
	// Token: 0x060006F1 RID: 1777 RVA: 0x00024B7C File Offset: 0x00022D7C
	public static Color GetColor(HitEffect effect)
	{
		switch (effect)
		{
			case HitEffect.AmmoNormal:
				return Color.yellow;
			case HitEffect.Crit:
				return Color.red;
			case HitEffect.AmmoPiercing:
				return Color.cyan;
			default:
				return Color.white;
		}
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00024BC8 File Offset: 0x00022DC8
	public static string GetColorName(HitEffect effect)
	{
		switch (effect)
		{
			case HitEffect.AmmoNormal:
				return "#" + ColorUtility.ToHtmlStringRGB(Color.yellow);
			case HitEffect.Crit:
				return "red";
			case HitEffect.AmmoPiercing:
				return "#" + ColorUtility.ToHtmlStringRGB(Color.cyan);
			default:
				return "white";
		}
	}
}
