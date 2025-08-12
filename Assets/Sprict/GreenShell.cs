// GreenShell.cs
using UnityEngine;

public class GreenShell : MonoBehaviour
{
    public float speed = 50f;
    public int maxBounces = 7;
    private int currentBounces = 0;
    private Rigidbody rb;
    private KartController owner;

    public void Initialize(KartController owner, Vector3 direction)
    {
        this.owner = owner;
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction * speed;
        Destroy(gameObject, 15f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        KartController otherKart = collision.gameObject.GetComponent<KartController>();
        if (otherKart != null && otherKart != owner)
        {
            otherKart.GetHit(1f);
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.tag != "Player")
        {
            currentBounces++;
            if (currentBounces >= maxBounces)
            {
                Destroy(gameObject);
            }
        }
    }
}