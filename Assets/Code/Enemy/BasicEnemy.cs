using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    protected override void Start()
    {
        moveSpeed = 2f;
        maxHealth = 100f;
        sightRange = 3f;
        damage = 20f;
        base.Start();
    }

    public override void Attack()
    {
        // none
    }

    protected override void Die()
    {
        base.Die();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            targetSc.TakeDamage(damage, true, 5f, transform.position);
        }
    }
}
