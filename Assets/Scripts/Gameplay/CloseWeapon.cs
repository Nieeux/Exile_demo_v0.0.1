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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (PlayerInput.Instance.GetRunningInputDown())
        {

            Rot = Vector3.Slerp(Rot, RunRotation, rotationalRecoilSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(Rot);
            transform.localPosition = Vector3.Slerp(transform.localPosition, RunPosition, rotationalRecoilSpeed * Time.deltaTime);
        }
        */

        RaycastHit Hit;
        if (Physics.Raycast(rotationPoint.transform.position, rotationPoint.transform.forward, out Hit, 1f, wall))
        {
            PutDownWeapons();
        }
        else
        {
            if (pad > 0)
            {
                Debug.Log("CloseWeapon");
                defuTransform();
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

}
