using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    [SerializeField] protected float detectionDistance = 5;
    private Transform playerTransform;
    protected bool isChasing;
    protected Vector2 directionToPlayer;
    [SerializeField] protected Projectile ProjectilePrefab;
    [SerializeField] protected Transform ProjectileSpawnPoint;
    [SerializeField] protected float shootTimer = 2;
    private LineRenderer playerLine;

    protected override void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerLine = GetComponent<LineRenderer>();

        shootTimer = Random.Range(shootTimer * 0.5f, shootTimer * 1.5f);
        InvokeRepeating("ShootProjectile", shootTimer, shootTimer);

        base.Start();
    }

    protected override void Update()
    {
        if (Vector2.Distance(playerTransform.position, transform.position) <= detectionDistance)
        {
            isChasing = true;
        }

        if(isChasing) 
        {
            playerLine.SetPosition(0, transform.position);
            playerLine.SetPosition(1, playerTransform.position);

            directionToPlayer = (playerTransform.position - transform.position).normalized;
            transform.up = directionToPlayer;
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if(isChasing) 
        {
            Move(directionToPlayer);
        }
    }

    public void ShootProjectile()
    {
        if (ProjectilePrefab != null && isChasing)
        {
            Projectile instance = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity);
            instance.SetDirection(transform.up);
        }
    }

    protected override void OnCharacterDeath()
    {
        Destroy(gameObject);
        base.OnCharacterDeath();
    }
}
