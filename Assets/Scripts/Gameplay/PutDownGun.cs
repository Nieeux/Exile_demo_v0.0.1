using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDownGun : MonoBehaviour
{
    [Header("Reference Points")]
    public LayerMask wall;

    public Transform rotationPoint;

    public float rotationalRecoilSpeed = 8f;

    public Vector3 Rotation = new Vector3(90, 0, 0);
    public Vector3 defuRotation = new Vector3(0, 0, 0);

    Vector3 rotationalRecoil;
    Vector3 Rot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit Hit;
        if (Physics.Raycast(rotationPoint.transform.position, rotationPoint.transform.forward, out Hit, 1f, wall))
        {
            PutDownWeapons();
        }
        else
        {
            Rot = Vector3.Slerp(Rot, defuRotation, rotationalRecoilSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(Rot);
        }
    }
    private void PutDownWeapons()
    {
        Rot = Vector3.Slerp(Rot, Rotation, rotationalRecoilSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Rot);
    }
}
