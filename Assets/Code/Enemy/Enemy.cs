using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBehavior {
    void Attack();
    void Move();
}

public abstract class Enemy : MonoBehaviour, IEnemyBehavior
{
    public float moveSpeed;
    public float maxHealth;
    public float sightRange;
    public float damage;
    public float attackRange;
    public float attackCool;

    protected float currentHealth;
    protected bool isStunned;
    protected bool isAttackReady = true;
    protected GameObject target;
    protected Rigidbody2D targetRigid;
    protected Player targetSc;
    protected Rigidbody2D rigid;
    protected Vector2 dir;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        target = GameManager.Instance.player;
        targetRigid = target.GetComponent<Rigidbody2D>();
        rigid = GetComponent<Rigidbody2D>();
        targetSc = target.GetComponent<Player>();
    }

    protected virtual void Update()
    {
        // 
        // if (PlayerFound() && !isStunned)
        // {
        //     Move();
        // }
    }

    public abstract void Attack();

    public virtual void Move()
    {
        // rigid.MovePosition(rigid.position + new Vector2(dir.x * moveSpeed * Time.deltaTime, 0));
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeKnockback(float strength, Vector2 point, float stunTime)
    {
        Vector2 kbDir = (rigid.position - point).normalized * strength;
        if (kbDir == Vector2.zero) kbDir = Vector2.up * strength;
        kbDir += Vector2.up * 0.3f * strength;
        GetStunned(stunTime);
        rigid.velocity = kbDir;
    }

    public void GetStunned(float time)
    {
        StartCoroutine(Stun(time));
    }

    private IEnumerator Stun(float time)
    {
        isStunned = true;
        yield return new WaitForSeconds(time);
        isStunned = false;
    }

    protected bool PlayerFound()
    {
        int layerMask = ~LayerMask.GetMask("Enemy");
        dir = (targetRigid.position - rigid.position).normalized;
        RaycastHit2D sight = Physics2D.Raycast(rigid.position, dir, sightRange, layerMask);
        return sight.collider != null && sight.collider.gameObject == target;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    protected IEnumerator startAttackCooldown(float time) {
        isAttackReady = false;
        yield return new WaitForSeconds(time);
        isAttackReady = true;
        Debug.Log("attack ready");
    }
}