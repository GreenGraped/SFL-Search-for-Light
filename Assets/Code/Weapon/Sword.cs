using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private GameObject player;
    private Player playerSc;

    private Sword() {
        damage = 50f;
        cooldown = 0.4f;
        attackRange = 2f;
    }

    private void Start() {
        player = GameManager.Instance.player;
        playerSc = player.GetComponent<Player>();
    }
    public override void Attack()
    {
        RaycastHit2D attackTarget = Physics2D.Raycast(player.transform.position, playerSc.playerDir, attackRange, LayerMask.GetMask("Enemy"));
        if (attackTarget.collider != null)
        {
            GameObject enemy = attackTarget.collider.gameObject;
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.TakeDamage(damage);
        }
    }
}
