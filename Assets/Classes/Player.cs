using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerBase
{
    protected override void Update()
    {
        transform.up = (mousePosition - transform.position).normalized;

        Move(transform.up);

        if(Input.GetButtonDown("Shoot"))
        {
            ShootProjectile(LeftSpawnPoint.position, transform.up);
            ShootProjectile(RightSpawnPoint.position, transform.up);
        }

        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Astronaut"))
        {
            CollectAstronaut(collision.gameObject);
        }

        base.OnTriggerEnter2D(collision);
    }
}
