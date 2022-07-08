using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Doll : MonoBehaviour
{
    HitAble health;

    public UnityAction onDamaged;

    void Start()
    {
        health = GetComponent<HitAble>();
        health.OnDie += OnDie;
        health.OnDamaged += OnDamaged;

    }
    void OnDamaged(float damage)
    {
        onDamaged?.Invoke();
    }
    void OnDie()
    {

        Destroy(gameObject);
    }
    void Update()
    {
        
    }
}
