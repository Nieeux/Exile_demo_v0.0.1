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

    public float ShakeHDuration = 1;
    public float ShakeHStrenght = 1;
    public int punchHVibrato = 5;
    public float Hrandomness = 70;

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
        base.Invoke("ReturnShake", 0.1f);
    }

    public void HighShake()
    {
        //Súng gi?t
        Sequence s = DOTween.Sequence();
        s.Append(base.transform.DOShakeRotation(ShakeHDuration, ShakeHStrenght, punchHVibrato, Hrandomness, true));
        base.Invoke("ReturnShake", 0.1f);
    }
    private void ReturnShake()
    {
        base.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
}
