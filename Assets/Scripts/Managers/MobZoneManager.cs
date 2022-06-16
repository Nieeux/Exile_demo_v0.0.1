using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobZoneManager : MonoBehaviour
{
	/*
	private void Awake()
	{
		MobZoneManager.zoneId = 0;
		MobZoneManager.Instance = this;
		this.zones = new Dictionary<int, TriggerArea>();
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00010A78 File Offset: 0x0000EC78
	public void AddZones(List<TriggerArea> zones)
	{
		foreach (TriggerArea spawnZone in zones)
		{
			this.AddZone(spawnZone, spawnZone.id);
		}
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00010ACC File Offset: 0x0000ECCC
	public void AddZone(TriggerArea mz, int id)
	{
		mz.SetId(id);
		this.zones.Add(id, mz);
		if (this.attatchDebug)
		{
			Object.Instantiate<GameObject>(this.debug, mz.transform).GetComponentInChildren<DebugObject>().text = "id" + id;
		}
	}

	public int GetNextId()
	{
		return MobZoneManager.zoneId++;
	}

	public void RemoveZone(int mobId)
	{
		this.zones.Remove(mobId);
	}

	public Dictionary<int, TriggerArea> zones;

	private static int zoneId;

	public bool attatchDebug;

	public GameObject debug;

	public static MobZoneManager Instance;
	*/
}