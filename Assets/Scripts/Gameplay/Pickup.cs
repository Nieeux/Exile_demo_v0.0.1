using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Pickup : MonoBehaviour
{

    public Rigidbody PickupRigidbody { get; private set; }

    Collider m_Collider;

    protected virtual void Start()
    {
        PickupRigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
    }

    void Update()
    {
    }

    //Trigger vào
    void OnTriggerEnter(Collider other)
    {
        Player pickingPlayer = other.GetComponent<Player>();

        if (pickingPlayer != null)
        {
            OnPicked(pickingPlayer);
        }
    }

    protected virtual void OnPicked(Player Controller)
    {
        PlayPickupFeedback();
    }

    public void PlayPickupFeedback()
    {

    }
}
