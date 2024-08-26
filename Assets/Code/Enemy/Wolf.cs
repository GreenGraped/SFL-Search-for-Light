using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Enemy
{


    protected override void Start()
    {
        moveSpeed = 3f;
        maxHealth = 50f;
        sightRange = 7f;
        attackRange = 2f;
        damage = 20f;
        attackCool = 0.6f;
        base.Start();
    }

    public override void Attack()
    {
        if (isAttackReady) {
            if (Mathf.Abs((transform.position - target.transform.position).y) < 0.6f) {
                targetSc.TakeDamage(damage, true, 5f, rigid.position);
                StartCoroutine(startAttackCooldown(attackCool));
            }
        }
    }

    protected override void Die()
    {
        base.Die();
    }

    protected override void Update()
    {
        // base.Update();
        ManageAggro();
        if (isAggro) {
            FollowingPlayer();
            if (Physics2D.OverlapCircle(rigid.position, attackRange, LayerMask.GetMask("Player"))) {
                Attack();
            }
        } else {
            Move();
        }
    }

    public void FollowingPlayer() {
        float direction = Mathf.Sign((target.transform.position - transform.position).normalized.x);
        moveDir = (int) direction;
        Vector2 targetPosition = new Vector2(rigid.position.x + direction * moveSpeed * Time.fixedDeltaTime, rigid.position.y);
        rigid.velocity = new Vector2(direction * moveSpeed, rigid.velocity.y);
    }
}
