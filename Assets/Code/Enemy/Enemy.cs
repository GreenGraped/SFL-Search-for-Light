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
    protected SpriteRenderer sprite;
    protected bool isAggro = false;
    protected bool isAggroOffing = false;
    protected int moveDir = 1;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        target = GameManager.Instance.player;
        targetRigid = target.GetComponent<Rigidbody2D>();
        rigid = GetComponent<Rigidbody2D>();
        targetSc = target.GetComponent<Player>();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void ManageAggro() {
        if (PlayerFound() && !isStunned)
        {
            if (Mathf.Sign((transform.position - target.transform.position).x) * -1 == moveDir) {
                isAggro = true;
            }
        }
        else if (!PlayerFound()) {
            if (isAggro && !isAggroOffing) {
                StartCoroutine(aggroOff());
            }
        }
    }

    protected IEnumerator aggroOff() {
        isAggroOffing = true;
        yield return new WaitForSeconds(5f);
        isAggroOffing = false;
        isAggro = false;
    }

    public abstract void Attack();

    public virtual void Move()
    {
        if (Physics2D.Raycast(rigid.position, new Vector2(moveDir, 0), 1f, LayerMask.GetMask("Ground"))
        || !Physics2D.OverlapPoint(rigid.position + new Vector2(moveDir, -1), LayerMask.GetMask("Ground"))) {
            moveDir *= -1;
        }
        if (moveDir == 1) {
            rigid.velocity = Vector2.right * moveSpeed;
        } else {
            rigid.velocity = Vector2.left * moveSpeed;
        }
        // 좌우로 이동하며 벽에 닿거나 낭떠러지가 있을 때 방향을 바꾸는 기능
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