using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid;
    private float moveDir;
    private bool onGround;
    private int currentJumpCount;
    [SerializeField] private int maxJumpCount;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float inertia;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bool isMoving = (moveDir != 0);
        RaycastHit2D ground = Physics2D.Raycast(rigid.position, Vector2.down, 1.3f, LayerMask.GetMask("Ground"));
        RaycastHit2D testRay = Physics2D.Raycast(rigid.position, Vector2.right, 1f, LayerMask.GetMask("Object"));
        Debug.DrawRay(rigid.position, Vector2.right, Color.green);
        onGround = ground.collider != null;
        if (testRay.collider != null)
        {
            Debug.Log(testRay.collider.name + " has detected");
        }
        if (onGround)
        {
            currentJumpCount = 0;
        }
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
        if (maxJumpCount <= 1) {
            if (onGround)
            {
                if (currentJumpCount == 0)
                {
                    Jump(jumpPower);
                }
            }
        } else {
            if (currentJumpCount < maxJumpCount - 1) {
                Jump(jumpPower);
            }

        }
        
    }

    void Jump(float jumpPower) {
        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        rigid.AddForce(new Vector2(0, jumpPower));
        currentJumpCount++;
    }
}
