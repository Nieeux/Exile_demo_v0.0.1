using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public static UseItem Instance;

    private void Awake()
    {
        UseItem.Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void useItem(ItemStats item)
    {
        if (item.name == "Supply")
        {
            OpenSupply pickup = Instantiate(item.prefab, transform.position 
                + new Vector3(Random.Range(-10f, 10f) * 2f, 200f, Random.Range(-10f, 10f) * 2f), transform.rotation).GetComponent<OpenSupply>();
        }
    }
}
