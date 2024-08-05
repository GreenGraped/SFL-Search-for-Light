using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Enemy
{

    private int moveDir = 1;
    private float moveD = 0;

    protected override void Start()
    {
        moveSpeed = 3f;
        maxHealth = 50f;
        sightRange = 7f;
        attackRange = 2f;
        damage = 20f;
        attackCool = 1f;
        base.Start();
    }

    public override void Attack()
    {
        if (isAttackReady) {
            targetSc.TakeDamage(damage, true, 0, rigid.position);
            StartCoroutine(startAttackCooldown(attackCool));
        }
    }

    protected override void Die()
    {
        base.Die();
    }

    protected override void Update()
    {
        Move();
        if (PlayerFound()) {
            // FIXME - 몬스터 뒤로 플레이어가 이동해도 몬스터가 플레이어를 공격함.
            if (Physics2D.OverlapCircle(rigid.position, attackRange, LayerMask.GetMask("Player"))) {
                Attack();
            }
        }
    }

    public override void Move() {
        if (moveD >= 5 || Physics2D.Raycast(rigid.position, new Vector2(moveDir, 0), 1f, LayerMask.GetMask("Ground"))
        || !Physics2D.OverlapPoint(rigid.position + new Vector2(moveDir, -1), LayerMask.GetMask("Ground"))) {
            moveDir *= -1;
            moveD = 0;
            Debug.Log("방향 전환");
        }
        if (moveDir == 1) {
            rigid.velocity = Vector2.right * moveSpeed;
        } else {
            rigid.velocity = Vector2.left * moveSpeed;
        }
        moveD += 0.05f;
        // 좌우로 이동하며 벽에 닿거나 낭떠러지가 있을 때 방향을 바꾸는 기능
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
