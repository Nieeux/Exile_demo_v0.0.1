using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UISway : MonoBehaviour
{
    public static UISway Instance;
    public Vector2 amount;
    public float lerp = .5f;
    public Transform Model;

    public float punchStrenght = .1f;
    public int punchVibrato = 5;
    public float punchDuration = .1f;
    public float punchElasticity = .5f;

    public float Amount = 0.02f,
    maxAmount = 0.06f,
    smoothAmount = 6f;

    private Vector3 initialPosition;

    private void Start()
    {
        UISway.Instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        float moveX = Mathf.Clamp(x * Amount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(y * Amount, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(moveX, moveY, -144);

        this.transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
        this.transform.localEulerAngles = new Vector3(Mathf.LerpAngle(transform.localEulerAngles.x, y * amount.y, lerp), Mathf.LerpAngle(transform.localEulerAngles.y, x * amount.x, lerp), 1);

    }
    public void RecoilHUD()
    {
        //Súng gi?t
        Sequence s = DOTween.Sequence();
        s.Append(Model.DOPunchPosition(new Vector3(0, 0, -punchStrenght), punchDuration, punchVibrato, punchElasticity));
    }

}