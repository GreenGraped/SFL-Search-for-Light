using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    [SerializeField] private float str;
    protected override void Start()
    {
        moveSpeed = 0f;
        maxHealth = 40f;
        sightRange = 10f;
        damage = 40f;
        attackCool = 3f;
        base.Start();
    }

    public override void Attack()
    {
        if (isAttackReady) {
            var arrow = ObjectPool.GetObject();
            arrow.damage = 40f;
            var dir = (target.transform.position - transform.position + new Vector3(0, 0.8f)).normalized;
            arrow.transform.position = transform.position + dir.normalized;
            arrow.Shoot(dir, str);
            StartCoroutine(startAttackCooldown(attackCool));
        }
    }
    protected override void Die()
    {
        base.Die();
    }

    protected override void Update()
    {
        if (PlayerFound()) {
            Attack();
        }
    }
}
