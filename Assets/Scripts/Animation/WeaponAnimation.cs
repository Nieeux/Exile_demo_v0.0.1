using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class WeaponAnimation : MonoBehaviour
{
    public static WeaponAnimation Instance;
    public float amount = 0.02f,
        maxAmount = 0.06f,
        smoothAmount = 6f;

    public float rotationAmount = 4f,
        maxRotationAmount = 5f,
        smoothRotation = 12f;

    private bool rotationX = true, rotationY = true, rotationZ = true;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private float InputX, InputY;
    public Transform Model;

    public float punchStrenght = .2f;
    public int punchVibrato = 5;
    public float punchDuration = .3f;
    public float punchElasticity = .5f;

    //public Animator animator;


    private void Start()
    {
        WeaponAnimation.Instance = this;
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("Punch", true);
            StartCoroutine(idle());

        }
        */
        CalculateSway();
        MoveSway();
        TiltSway();
    }
    /*
    public IEnumerator idle()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("Punch", false);
    }
    */
    public void RecoilAni()
    {
        //Súng gi?t
        Sequence s = DOTween.Sequence();
        s.Append(Model.DOPunchPosition(new Vector3(0, 0, -punchStrenght), punchDuration, punchVibrato, punchElasticity));
    }

    private void CalculateSway()
    {
        InputX = Input.GetAxis("Mouse X");
        InputY = Input.GetAxis("Mouse Y");
    }

    private void MoveSway()
    {
        float moveX = Mathf.Clamp(InputX * amount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(InputY * amount, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(moveX, moveY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
    }

    private void TiltSway()
    {
        float tiltY = Mathf.Clamp(InputX * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(InputY * rotationAmount, -maxRotationAmount, maxRotationAmount);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? -tiltX : 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f));
            
        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, smoothRotation * Time.deltaTime);
    }
}