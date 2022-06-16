using System.Collections;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public float Damage = 50;
    public float maxDamage = 1000;

    public void UpdateWeapon(float amount)
    {
        Damage += amount;
        Damage = Mathf.Min(Damage, maxDamage);

    }
    //Hồi phục khi nhặt máu

    public void Speed(float amount)
    {

    }

    public void OnHeal(float amount)
    {

    }

}