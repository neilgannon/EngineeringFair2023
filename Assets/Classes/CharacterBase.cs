using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class CharacterBase : MonoBehaviour
{
    protected float currentHealth = 100;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float maxSpeed = 100;
    protected float currentSpeed;
    protected Rigidbody2D body;
    [SerializeField] private float boostPercentage = 1.5f;
    [SerializeField] private float maxBodyMagnitude = 5;
    [HideInInspector] public UnityEvent<float, float> HealthUpdated;
    [SerializeField] private float healthRegenerationAmount = 2;
    [SerializeField] private bool doesHealthRegenerate = false;

    protected virtual void Start()
    {
        currentSpeed = maxSpeed;
        currentHealth = maxHealth;
        body = GetComponent<Rigidbody2D>();
    }

    protected void StartBoost()
    {
        currentSpeed = maxSpeed * boostPercentage;
    }

    protected void StopBoost()
    {
        currentSpeed = maxSpeed;
    }

    protected virtual void Move(Vector2 direction)
    {
        body.AddForce(direction * currentSpeed * Time.deltaTime);

        if (body.velocity.magnitude > maxBodyMagnitude)
        {
            body.velocity = Vector2.ClampMagnitude(body.velocity, maxBodyMagnitude);
        }
    }

    public void AddHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if(HealthUpdated != null) 
        {
            HealthUpdated.Invoke(currentHealth, maxHealth);
        }
    }

    public void SubtractHeallth(float amount)
    {
        currentHealth -= amount;

        if (HealthUpdated != null)
        {
            HealthUpdated.Invoke(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            OnCharacterDeath();
        }
    }

    protected virtual void OnCharacterDeath() { }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Projectile instance = collision.gameObject.GetComponent<Projectile>();
            SubtractHeallth(instance.GetDamage());
        }
    }

    protected virtual void Update() 
    {
        if(doesHealthRegenerate && currentHealth < maxHealth)
        {
            AddHealth(healthRegenerationAmount * Time.deltaTime);
        }
    }
}
