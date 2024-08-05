using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Enemy
{
    protected override void Start()
    {
        moveSpeed = 0f;
        maxHealth = 500f;
        sightRange = 30f;
        damage = 0f;
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
        
    }
}
