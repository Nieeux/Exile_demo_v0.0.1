using System;
using UnityEngine;

// Token: 0x020000B4 RID: 180
[CreateAssetMenu]
public class Buff : ScriptableObject
{
	// Token: 0x060004A3 RID: 1187 RVA: 0x0001896C File Offset: 0x00016B6C
	public Color GetOutlineColor()
	{
		switch (this.tier)
		{
			case Buff.PowerTier.White:
				return Color.white;
			case Buff.PowerTier.Blue:
				return Color.cyan;
			case Buff.PowerTier.Orange:
				return Color.yellow;
			default:
				return Color.white;
		}
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x000189AC File Offset: 0x00016BAC
	public string GetColorName()
	{
		switch (this.tier)
		{
			case Buff.PowerTier.White:
				return "white";
			case Buff.PowerTier.Blue:
				return "#00C0FF";
			case Buff.PowerTier.Orange:
				return "orange";
			default:
				return "white";
		}
	}

	// Token: 0x0400047C RID: 1148
	public new string name;

	// Token: 0x0400047D RID: 1149
	public string description;

	// Token: 0x0400047E RID: 1150
	public int id;

	// Token: 0x0400047F RID: 1151
	public Buff.PowerTier tier;

	// Token: 0x04000480 RID: 1152
	public Mesh mesh;

	// Token: 0x04000481 RID: 1153
	public Material material;

	// Token: 0x04000482 RID: 1154
	public Sprite sprite;

	// Token: 0x02000176 RID: 374
	public enum PowerTier
	{
		// Token: 0x040009B4 RID: 2484
		White,
		// Token: 0x040009B5 RID: 2485
		Blue,
		// Token: 0x040009B6 RID: 2486
		Orange
	}
}
