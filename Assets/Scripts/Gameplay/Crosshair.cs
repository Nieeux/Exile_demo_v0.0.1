using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    GameObject Weapon;

    void Update()
    {
        transform.position = Weapon.transform.position;
        transform.rotation = Weapon.transform.rotation;
    }
}
