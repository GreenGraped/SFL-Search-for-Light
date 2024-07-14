using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public float moveSpeed = 0f;
    [HideInInspector] public float maxHealth = 0f;
    [HideInInspector] public float sightRange = 0f;
    private float currentHealth;
    private bool isStunned;
    private GameObject target;
    private Rigidbody2D targetRigid;
    private Rigidbody2D rigid;
    private Vector2 dir;

    void Start()
    {
        currentHealth = maxHealth;
        target = GameManager.Instance.player;
        targetRigid = target.GetComponent<Rigidbody2D>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dir = (targetRigid.position - rigid.position).normalized;
        if (playerFound() && !isStunned) {
            moveToPlayer();
        }
    }

    public void moveToPlayer() {
        rigid.MovePosition(rigid.position + new Vector2(dir.x * moveSpeed * Time.deltaTime, 0));
    }

    public void takeDamage(float damage) {
        currentHealth -= damage;
        Debug.Log("Enemy : Damage taken. current health : " + currentHealth);
        // 데미지 애니메이션 재생
        if (currentHealth <= 0) {
            Die();
        }
    }

    public void takeKnockback(float strength, Vector2 point, float stunTime) {
        Vector2 kbDir = (rigid.position - point).normalized * strength;
        if (kbDir == Vector2.zero) kbDir = Vector2.up * strength;
        kbDir += Vector2.up * 0.3f * strength;
        getStunned(stunTime);
        rigid.AddForce(kbDir, ForceMode2D.Impulse);
    }

    public void getStunned(float time) {
        StartCoroutine(Stun(time));
    }

    IEnumerator Stun(float time) {
        isStunned = true;
        yield return new WaitForSeconds(time);
        isStunned = false;
    }

    private bool playerFound()
    {
        // 'Enemy' 레이어를 제외한 모든 레이어에 대해서만 레이캐스트
        int layerMask = ~LayerMask.GetMask("Enemy");

        RaycastHit2D sight = Physics2D.Raycast(rigid.position, dir, sightRange, layerMask);
        if (sight.collider != null && sight.collider.gameObject == target)
        {
            return true;
        }
        return false;
    }

    private void Die() {
        // Death Animation
        Destroy(gameObject);
    }
}
