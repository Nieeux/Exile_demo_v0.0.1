using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ItemStatsGenerator : MonoBehaviour
{
    public static ItemStatsGenerator Instance;

    public ItemStats item;

    public ItemStats itemChange;

	private Random random;

	public float Original;

	public float Upgrade;

	public float Advanced;

	public float randomly;

	private void Awake()
	{
		ItemStatsGenerator.Instance = this;
		this.random = new Random();

	}
	void Start()
    {


		GetRandomStatsWeapon(Original, Upgrade, Advanced);
		GetRandomStatsWeapon();

	}

	public float GetRandomStatsWeapon(float Original, float Upgrade, float Advanced)
	{
		float num = Original + Upgrade + Advanced;
		float num2 = (float)random.NextDouble();
		if (num2 < Original / num)
		{
			return randomly = 1;
		}
		if (num2 < (Original + Upgrade) / num)
		{
			return randomly = 2;

		}
			return randomly = 3;
	}
	public void GetRandomStatsWeapon()
    {
		ItemStats inventoryItem = ScriptableObject.CreateInstance<ItemStats>();
        if (randomly == 1)
        {
			inventoryItem.GetweaponOriginal(this.item);
		}
		if (randomly == 2)
		{
			inventoryItem.GetweaponUpgrade(this.item);
		}
		if (randomly == 3)
		{
			inventoryItem.GetweaponAdvanced(this.item);
		}
		itemChange = inventoryItem;
	}
}
