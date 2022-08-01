using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    [Space]
    [Header("Velocity based sway")]
    public float amount = 0.1f;

    public float maxAmount = 0.3f;

    public float smoothAmount = 6f;

    public float scale = 0.25f;

    public float Weight { get; set; }

    [Header("Step based sway")]
    public float stepStrenght = 5f;

    public float noiseStrenght = 1f;

    public float stepMultiplier = 1f;

    [Header("Rotation Step sway")]
    public float rotSmoothAmount = 6f;


    public float stepRotMultiplier = 1f;

    public Vector3 initPos;

    public Quaternion initRot;

    public Vector3 finalPosToMove;

    public Vector3 stepPos = Vector3.zero;

    public Quaternion stepRot = Quaternion.identity;

    private float XAxis;

    private float ZAxis;

    private void Start()
    {
        this.Weight = 1f;
        this.initPos = base.transform.localPosition;
        this.initRot = base.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        this.InputAxis();
        if(finalPosToMove != Vector3.zero)
        {
            this.stepPos = Vector3.Lerp(this.stepPos, Vector3.zero, Time.deltaTime * this.smoothAmount);
            this.stepRot = Quaternion.Slerp(this.stepRot, Quaternion.identity, Time.deltaTime * this.rotSmoothAmount);
            Vector3 vector = this.finalPosToMove + this.stepPos;
            vector *= this.scale;
            vector *= this.Weight;
            Quaternion b = this.stepRot;
            base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, vector, Time.deltaTime * this.smoothAmount);
            base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, b, Time.deltaTime * this.rotSmoothAmount);
        }

    }
    private void InputAxis()
    {
        this.XAxis = -Input.GetAxis("Horizontal") * this.amount;
        this.ZAxis = -Input.GetAxis("Vertical") * this.amount;
        this.XAxis = Mathf.Clamp(this.XAxis, -this.maxAmount, this.maxAmount);
        this.ZAxis = Mathf.Clamp(this.ZAxis, -this.maxAmount, this.maxAmount);
        this.finalPosToMove = new Vector3(this.XAxis, 0f, this.ZAxis) + this.initPos;
    }
    public void OnStep()
    {
        Vector3 a = new Vector3(8f, 8f, 8f);
        float num = this.stepStrenght;
        float num2 = 10f;
        float d = Mathf.Cos(Time.timeSinceLevelLoad / 0.5f) * Mathf.PerlinNoise(Time.time, 0f) * this.noiseStrenght;
        a *= d;
        num += num / (1f - num2);
        num = Mathf.Clamp(num, 0f, 18f);
        a /= 2f;
        a.y = -num;
        this.stepPos += a * this.stepMultiplier / 10f;
        Vector3 a2 = new Vector3(0f, 0f, num) * this.stepRotMultiplier * (float)Random.Range(-1, 2);
        this.stepRot = Quaternion.Euler(a2 * this.Weight * this.scale) * this.initRot;
    }
}
