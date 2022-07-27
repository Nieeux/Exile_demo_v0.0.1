using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        base.Invoke("StartFade", 5);
    }

    void StartFade()
    {
        GetComponent<Rigidbody>().drag = 5;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2);
    }

}
