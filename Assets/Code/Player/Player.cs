using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid;
    private float moveDir;
    private bool onGround;
    private int currentJumpCount;
    private bool onAttackCooldown;
    private Weapon currentWeapon;
    private bool hasDash;
    private int dashCount;
    private bool hasDashCool = false;
    private Vector2 dashDir;
    [HideInInspector] public Vector2 playerDir;
    [HideInInspector] public bool hasLantern;
    [HideInInspector] public bool isAction;
    [HideInInspector] public bool isTalking;
    [HideInInspector] public bool talkingInProgress;
    [HideInInspector] public float maxHealth;
    [HideInInspector] public float health;
    [HideInInspector] public bool isStunned;
    
    [SerializeField] private int maxJumpCount;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float inertia;
    [SerializeField] private float dashCool;
    public GameManager.Location currentLocation;
    public int bossFighting = -1; // not fighting
    


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Init();
        Reset();
    }

    private void Init() {
        maxHealth = 100f;
        maxJumpCount = 1;
        speed = 40f;
        maxSpeed = 7f;
        jumpPower = 8f;
        inertia = 0f; // 추후 수정 가능성 있음
        dashCool = 0.1f;
    }

    private void Reset() {
        health = maxHealth;
    }



    private void Update()
    {
        bool isMoving = (moveDir != 0);
        if (moveDir < 0) playerDir = Vector2.left;
        else if (moveDir > 0) playerDir = Vector2.right;
        if (hasLantern) { // if (currentLocation == GameManager.Location.Cp1)
            LanternFollowing();
        }

        RaycastHit2D ground = Physics2D.Raycast(rigid.position, Vector2.down, 1.3f, LayerMask.GetMask("Ground"));
        onGround = ground.collider != null;

        if (onGround)
        {
            currentJumpCount = 0;
            dashCount = 1;
            
        }
        if (!isAction && !isStunned) {
            HandleMovement(isMoving);
        }
        
    }

    private void LanternFollowing() {
        GameManager.Instance.Lantern.transform.position = rigid.position + new Vector2(.7f, 0) * playerDir;
    }
    private void HandleMovement(bool isMoving)
    {
        if (isMoving)
        {
            // 이동 방향으로 힘을 추가
            rigid.AddForce(Vector2.right * moveDir * speed * Time.deltaTime, ForceMode2D.Impulse);

            // 최대 속도 제한
            if (Mathf.Abs(rigid.velocity.x) > maxSpeed)
            {
                rigid.velocity = new Vector2(Mathf.Sign(rigid.velocity.x) * maxSpeed, rigid.velocity.y);
            }
        }
        else
        {
            if (onGround)
            {
                // 착지 시 관성을 적용하여 속도 감소를 부드럽게 처리
                rigid.velocity = new Vector2(rigid.velocity.x * inertia, rigid.velocity.y);
            }
            else
            {
                // 공중에서는 속도를 유지
                rigid.velocity = new Vector2(rigid.velocity.x * 0.99f, rigid.velocity.y);
            }
        }
    }


    void OnMove(InputValue value)
    {
        // float input = value.Get<float>();
        Vector2 input = value.Get<Vector2>();
        moveDir = input.x;
        if (input != Vector2.zero) {
            if (input.x < 0) dashDir = Vector2.left;
            else dashDir = Vector2.right;
        }
    }

    void OnJump()
    {
        if (!isAction) {
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
    }

    private void Jump(float jumpPower)
    {
        rigid.velocity = new Vector2(rigid.velocity.x, 0); // 점프 전에 수직 속도를 초기화
        rigid.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        currentJumpCount++;
    }

    private void Dash()
    {
        if (dashDir == Vector2.zero) dashDir = Vector2.right;

        // 플레이어가 대시할 목표 위치를 계산
        Vector2 targetPosition = rigid.position + dashDir.normalized * 5f;

        // 대시 방향으로 레이캐스트를 쏴서 충돌 검사
        RaycastHit2D dashPoint = Physics2D.Raycast(rigid.position, dashDir.normalized, 5f, LayerMask.GetMask("Ground"));
        
        if (dashPoint.collider == null)
        {
            // 충돌하지 않으면 목표 위치로 이동
            rigid.position = targetPosition;
            rigid.velocity = Vector2.zero;
        }
        else
        {
            // 충돌한 경우, 충돌 지점 바로 앞에서 멈추도록 목표 위치를 조정
            CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
            float capsuleRadius = capsuleCollider.size.x / 2f;

            // 충돌 지점에서 플레이어의 캡슐 반지름만큼 뒤로 물러남
            targetPosition = dashPoint.point - dashDir.normalized * capsuleRadius;

            // 플레이어 위치를 조정된 위치로 설정
            rigid.position = targetPosition;
            rigid.velocity = Vector2.zero;
        }
    }


    void OnInteraction()
    {
        if (isTalking) {
            // talking method...
            if (talkingInProgress) {
                GameManager.Instance.dialogueManager.dialogInterval = 0.005f;
            }
            else {
                GameManager.Instance.dialogueManager.dialogInterval = 0.05f;
                GameManager.Instance.dialogueManager.talk(GameManager.Instance.currentDialogId);
            }
        }
        else {
            RaycastHit2D interaction = Physics2D.Raycast(rigid.position, playerDir, 1f, LayerMask.GetMask("Object"));
            if (interaction.collider != null)
            {
                Collider2D col = interaction.collider;
                if (col.name == "Door_HTO") {
                    rigid.position = new Vector2(-18, -1.76f);
                    currentLocation = GameManager.Location.Ground;
                    GameManager.Instance.cameraCon.MoveCamera(new Vector2(-7, 1));
                } else if (col.name == "Door_OTH") {
                    rigid.position = new Vector2(-44, -1.76f);
                    currentLocation = GameManager.Location.Home;
                    GameManager.Instance.cameraCon.MoveCamera(new Vector2(-50, 1));
                } else if (col.name == "sword") {
                    EquipWeapon(GameManager.Instance.weaponManager.sword);
                    Destroy(col.gameObject);
                } else if (col.name == "Lantern") {
                    hasLantern = true;
                    CapsuleCollider2D lanternCol = GameManager.Instance.Lantern.GetComponent<CapsuleCollider2D>();
                    lanternCol.enabled = false;
                    GameManager.Instance.Lantern.layer = 0;
                } else if (col.name == "Dash Skill") {
                    // get skill animation / tutorial
                    hasDash = true;
                }
            }
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

    IEnumerator attackCooldown(float t) {
        onAttackCooldown = true;
        yield return new WaitForSeconds(t);
        onAttackCooldown = false;
    }

    void OnDash()
    {
        if (hasDash && dashCount > 0 && !hasDashCool) {
            Dash();
            dashCount--;
            StartCoroutine(dashCooldown(dashCool));
        }
        
    }

    IEnumerator dashCooldown(float t) {
        hasDashCool = true;
        yield return new WaitForSeconds(t);
        hasDashCool = false;
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
                    enemySc.TakeDamage(damage);
                    enemySc.TakeKnockback(3f, rigid.position, 2f);
                }
            }
        }
    }

    public void switchAction(bool action, string type) {
        if (action) {
            rigid.velocity = Vector2.zero;
            GameManager.Instance.dialogueManager.canvas.SetActive(true);
            isAction = true;
            if (type == "talk") {
                isTalking = true;
            }
        } 
        else {
            isAction = false;
            isTalking = false;
            GameManager.Instance.dialogueManager.resetCanvas();
            GameManager.Instance.dialogueManager.canvas.SetActive(false);
        }
    }

    public void TakeDamage(float damage, bool knockback, float strength, Vector2 attackPoint) {
        health -= damage;
        Debug.Log("attacked. current health is " + health);
        if (knockback) {
            TakeKnockback(strength, attackPoint, 0.2f);
        }
        if (health <= 0) {
            Die();
        }
    }

    public void TakeKnockback(float strength, Vector2 point, float stunTime)
    {
        Vector2 kbDir = (rigid.position - point - new Vector2(0, -0.5f)).normalized * strength;
        if (kbDir == Vector2.zero) kbDir = Vector2.up * strength;
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

    private void Die() {
        Debug.Log("you died");
        Reset();
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.name == "Boss Start") {
            if (currentLocation == GameManager.Location.Cp1) { // 챕터 1일때
                col.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                GameManager.Instance.cameraCon.isCameraMoving = false;
                StartCoroutine(FadeOutMep(0.5f));
            }
        }
    }

    IEnumerator FadeIn(float fadeDuration) {
        UnityEngine.UI.Image fade = GameObject.Find("Fade").GetComponent<UnityEngine.UI.Image>();
        
        float elapsedTime = 0f;
        Color color = fade.color;
        color.a = 1f;  // 처음에 alpha를 1로 설정
        fade.color = color;

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);  // alpha 값을 감소
            fade.color = color;
            yield return null;
        }

        color.a = 0f;
        fade.color = color;

    }

    IEnumerator FadeOut(float fadeDuration) {
        UnityEngine.UI.Image fade = GameObject.Find("Fade").GetComponent<UnityEngine.UI.Image>();
        
        float elapsedTime = 0f;
        Color color = fade.color;
        color.a = 1f;  // 처음에 alpha를 1로 설정
        fade.color = color;

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);  // alpha 값을 감소
            fade.color = color;
            yield return null;
        }

        color.a = 1f;
        fade.color = color;

    }

    IEnumerator FadeOutMep(float fadeDuration) {
        UnityEngine.UI.Image fade = GameObject.Find("Fade").GetComponent<UnityEngine.UI.Image>();
        
        float elapsedTime = 0f;
        Color color = fade.color;
        color.a = 1f;  // 처음에 alpha를 1로 설정
        fade.color = color;

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);  // alpha 값을 감소
            fade.color = color;
            yield return null;
        }

        color.a = 1f;
        fade.color = color;
        GameManager.Instance.cameraCon.MoveCamera(new Vector2(115f, 3.5f));
        rigid.position = new Vector2(105, -1.5f);
        GameManager.Instance.cameraCon.changeCameraSize(7f);
        GameManager.Instance.Mep.BossStart();
        bossFighting = 1;
        StartCoroutine(FadeIn(fadeDuration));
    }

}
