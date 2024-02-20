using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject explosionEffect;

    private void Start()
    {
        var rigidBody = GetComponent<Rigidbody>();

        rigidBody.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision col)
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

