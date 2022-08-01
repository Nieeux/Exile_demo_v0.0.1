using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    [Header("Reference Points")]
    public LayerMask wall;

    public Transform rotationPoint;

    public float rotationalRecoilSpeed = 8f;
    public float pad = 0;

    public Vector3 DownPosition = new Vector3(0.124f, -0.12f, 0);
    public Vector3 DownRotation = new Vector3(-70f, 0, -30f);

    public Vector3 RunPosition = new Vector3(0, 0, 0);
    public Vector3 RunRotation = new Vector3(0, 0, 0);

    public Vector3 defuPotation = new Vector3(0.124f, -0.12f, 0.451f);
    public Vector3 defuRotation = new Vector3(0, 0, 0);

    Vector3 Rot;

    [Header("WeaponBob")]
    public float walkingBobbingSpeed = 14f;

    public float RunningBobbingSpeed = 14f;

    public float WalkbobbingAmount = 0.05f;

    public float RunbobbingAmount = 0.05f;

    private float defaultPosY;

    private float timer;

    public static CloseWeapon Instance;

    private void Awake()
    {
        CloseWeapon.Instance = this;
    }
    private void Start()
    {
        this.defaultPosY = base.transform.localPosition.y;
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.Instance.isRunning)
        {
            RunningWeapons();
        }
        else
        {
            RaycastHit Hit;
            if (Physics.Raycast(rotationPoint.transform.position, rotationPoint.transform.forward, out Hit, 1f, wall))
            {
                PutDownWeapons();
            }
            else
            {
                if (pad > 0 && !PlayerMovement.Instance.isRunning)
                {
                    defuTransform();
                }
            }
        }




    }
    private void PutDownWeapons()
    {
        this.pad = Mathf.Lerp(this.pad, 300, Time.deltaTime * 10f);
        Rot = Vector3.Slerp(Rot, DownRotation, rotationalRecoilSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Rot);
        transform.localPosition = Vector3.Slerp(transform.localPosition, DownPosition, rotationalRecoilSpeed * Time.deltaTime);
    }
    public bool defuTransform()
    {
        this.pad = (int)Mathf.Lerp(this.pad, 0, Time.deltaTime * 10f);
        Rot = Vector3.Slerp(Rot, defuRotation, rotationalRecoilSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Rot);
        transform.localPosition = Vector3.Slerp(transform.localPosition, defuPotation, rotationalRecoilSpeed * Time.deltaTime);
        return false;
    }
    private void RunningWeapons()
    {
        this.pad = Mathf.Lerp(this.pad, 300, Time.deltaTime * 10f);
        Rot = Vector3.Slerp(Rot, RunRotation, rotationalRecoilSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Rot);
        transform.localPosition = Vector3.Slerp(transform.localPosition, RunPosition, rotationalRecoilSpeed * Time.deltaTime);
    }

    public void WeaponBob()
    {
        if (PlayerMovement.Instance.isRunning)
        {
            this.timer += Time.deltaTime * this.RunningBobbingSpeed;
            this.transform.localPosition = new Vector3(base.transform.localPosition.x, this.defaultPosY + Mathf.Sin(this.timer) * this.RunbobbingAmount, base.transform.localPosition.z);
            return;
        }
        this.timer += Time.deltaTime * this.walkingBobbingSpeed; 
        this.transform.localPosition = new Vector3(base.transform.localPosition.x, this.defaultPosY + Mathf.Sin(this.timer) * this.WalkbobbingAmount, base.transform.localPosition.z);
    }



}
