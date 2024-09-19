using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 50;
    [SerializeField] private float damage = 50;
    [SerializeField] private GameObject ExplosionPrefab;

    public void SetDirection(Vector2 direction)
    {
        transform.up = direction.normalized;
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.velocity = direction.normalized * speed;

        Invoke("RemoveFromScene", 5);
    }

    private void RemoveFromScene()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(ExplosionPrefab,transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public float GetDamage()
    {
        return damage;
    }
}
