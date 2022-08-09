using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    public float ShakeDuration = 1;
    public float ShakeStrenght = 1;
    public int punchVibrato = 5;
    public float randomness = 70;

    private void Awake()
    {
        CameraShake.Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shake()
    {
        //Súng gi?t
        Sequence s = DOTween.Sequence();
        s.Append(base.transform.DOShakeRotation(ShakeDuration, ShakeStrenght, punchVibrato, randomness, true));
    }
}
