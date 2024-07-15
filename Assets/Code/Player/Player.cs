using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid;
    private float moveDir;
    private bool onGround;
    private Vector2 playerDir;
    private int currentJumpCount;
    private LineRenderer lineRenderer;

    [SerializeField] private int maxJumpCount;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float inertia;
    [SerializeField] private int weapon;
    [SerializeField] private float atkRange;
    [SerializeField] private float damage;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();


        // LineRenderer 설정
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.sortingOrder = -1;  // 필요한 경우 sorting order 설정
    }

    private void Update()
    {
        bool isMoving = (moveDir != 0);
        if (moveDir < 0) playerDir = Vector2.left;
        else if (moveDir > 0) playerDir = Vector2.right;

        RaycastHit2D ground = Physics2D.Raycast(rigid.position, Vector2.down, 1.3f, LayerMask.GetMask("Ground"));
        onGround = ground.collider != null;
        if (onGround)
        {
            currentJumpCount = 0;
        }
        HandleMovement(isMoving);
    }

    private void HandleMovement(bool isMoving)
    {
        if (isMoving)
        {
            rigid.AddForce(Vector2.right * moveDir * speed * Time.deltaTime, ForceMode2D.Impulse);
            if (rigid.velocity.x > maxSpeed)
            {
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            }
            else if (rigid.velocity.x < -maxSpeed)
            {
                rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);
            }
        }
        else
        {
            rigid.velocity = new Vector2(rigid.velocity.x * inertia, rigid.velocity.y);
        }
    }


    void OnMove(InputValue value)
    {
        float input = value.Get<float>();
        moveDir = input;
    }

    void OnJump()
    {
        if (maxJumpCount <= 1)
        {
            if (onGround && currentJumpCount == 0)
            {
                Jump(jumpPower);
            }
        }
        else
        {
            if (currentJumpCount < maxJumpCount - 1)
            {
                Jump(jumpPower);
            }
        }
    }

    private void Jump(float jumpPower)
    {
        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        rigid.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        currentJumpCount++;
    }

    void OnInteraction()
    {
        RaycastHit2D interaction = Physics2D.Raycast(rigid.position, playerDir, 1f, LayerMask.GetMask("Object"));
        if (interaction.collider != null)
        {
            Debug.Log(interaction.collider.name + " has detected!");
        }
    }

    void OnAttack()
    {
        RaycastHit2D attackTarget = Physics2D.Raycast(rigid.position, playerDir, atkRange, LayerMask.GetMask("Enemy"));
        if (attackTarget.collider != null)
        {
            GameObject enemy = attackTarget.collider.gameObject;
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.takeDamage(30);
            Debug.Log("attacked");
        }
    }

    void OnStrongAttack()
    {
        
    }

    private void RangeAttack(float damage)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(rigid.position, 1f, LayerMask.GetMask("Enemy"));
        if (targets != null)
        {
            foreach (Collider2D target in targets)
            {
                Enemy enemySc = target.gameObject.GetComponent<Enemy>();
                if (enemySc != null)
                {
                    enemySc.takeDamage(damage);
                    enemySc.takeKnockback(3f, rigid.position, 2f);
                }
            }
        }
    }
}
