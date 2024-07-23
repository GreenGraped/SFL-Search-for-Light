using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid;
    private float moveDir;
    private bool onGround;
    public Vector2 playerDir;
    private int currentJumpCount;
    // 공격 쿨타임 만들기
    private bool onAttackCooldown;
    private Weapon currentWeapon;

    [SerializeField] private int maxJumpCount;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float inertia;
    public GameManager.Location currentLocation;
    

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bool isMoving = (moveDir != 0);
        if (moveDir < 0) playerDir = Vector2.left;
        else if (moveDir > 0) playerDir = Vector2.right;
        if (currentLocation == GameManager.Location.Cp1) {
            LanternFollowing();
        }

        RaycastHit2D ground = Physics2D.Raycast(rigid.position, Vector2.down, 1.3f, LayerMask.GetMask("Ground"));
        onGround = ground.collider != null;
        if (onGround)
        {
            currentJumpCount = 0;
        }
        HandleMovement(isMoving);
    }
    private void LanternFollowing() {
        GameManager.Instance.Lantern.transform.position = rigid.position;
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
            Collider2D col = interaction.collider;
            if (col.name == "Door_HTO") {
                rigid.position = new Vector2(-18, -1.76f);
                currentLocation = GameManager.Location.Ground;
                GameManager.Instance.MoveCamera(new Vector2(-7, 1));
            } else if (col.name == "Door_OTH") {
                rigid.position = new Vector2(-44, -1.76f);
                currentLocation = GameManager.Location.Home;
                GameManager.Instance.MoveCamera(new Vector2(-50, 1));
            } else if (col.name == "sword") {
                EquipWeapon(GameManager.Instance.sword);
                Destroy(col.gameObject);
            }
            Debug.Log(interaction.collider.name + " has detected!");
        }
    }

    void OnAttack()
    {
        if (currentWeapon != null) {
            if (!onAttackCooldown) {
                currentWeapon.Attack();
                StartCoroutine(attackCooldown(currentWeapon.cooldown));
            }
        }
        
    }

    private void EquipWeapon(Weapon newWeapon) {
        currentWeapon = newWeapon;
    }

    private void DropWeapon()
    {
        if (currentWeapon != null)
        {
            // Handle weapon dropping logic if needed
            currentWeapon = null;
        }
    }

    IEnumerator attackCooldown(float t) {
        onAttackCooldown = true;
        yield return new WaitForSeconds(t);
        onAttackCooldown = false;
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
