// Banana.cs
using UnityEngine;

public class Banana : MonoBehaviour
{
    private KartController owner;

    public void Initialize(KartController owner)
    {
        this.owner = owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        KartController otherKart = other.GetComponent<KartController>();
        if (otherKart != null && otherKart != owner)
        {
            otherKart.SpinAndStun(1f);
            Destroy(gameObject);
        }
    }
}