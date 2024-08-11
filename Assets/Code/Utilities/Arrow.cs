using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rigid;
    private Vector2 dir;
    public float damage;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage) {

    }

    public void Shoot(Vector2 dir, float power) {
        this.dir = dir;
        rigid.AddForce(dir, ForceMode2D.Impulse);
        Invoke("DestroyBullet", 5f);
    }

    public void DestroyBullet() {
        ObjectPool.ReturnObject(this);
    }

    void Update() {
        Collider2D touchedPlayer = Physics2D.OverlapCircle(rigid.position, transform.localScale.x, LayerMask.GetMask("Player"));
        Collider2D touchedGround = Physics2D.OverlapCircle(rigid.position, transform.localScale.x, LayerMask.GetMask("Ground"));
        if (touchedPlayer) {
            GameManager.Instance.playerSc.TakeDamage(damage, true, 0.1f, rigid.position);
            DestroyBullet();
        }
        if (touchedGround) {
            DestroyBullet();
        }
    }
}
