using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Scripting;

public class Mephistopeles : MonoBehaviour
{
    private GameObject player;
    private Player playerSc;
    private LineRenderer lineRenderer;
    private Rigidbody2D rigid;

    private Vector2 idlePosition = new Vector2(158, 5);
    private Vector2 defenselessPos = new Vector2(150, 1);
    private bool invincibility = true;
    private Color startColor;
    private Color endColor;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    private float maxHealth = 500;
    private float currentHealth;
    private float dashDamage = 40f;
    private bool isDashing = false;
    private bool isLiving = true;

    void Start() {
        player = GameManager.Instance.player;
        playerSc = GameManager.Instance.playerSc;

        lineRenderer = GetComponent<LineRenderer>();
        startColor = new Color(1f, 0f, 0f, 0f);
        endColor = new Color(1f, 0f, 0f, 1f);
        Material lineMaterial = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material = lineMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 2f;
        lineRenderer.endWidth = 2f;

        rigid = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        
    }

    public void BossStart()
    {
        transform.position = new Vector2(158, 12);
        StartCoroutine(Boss());
        
    }

    IEnumerator Boss() {
        yield return StartCoroutine(AppearAnim());
        StartCoroutine(turnOffTheLight());
        yield return StartCoroutine(Idle(3f));
        while (isLiving) {
            for (int i = 0; i < 3; i++) {
                yield return StartCoroutine(Dash());
            }
            yield return StartCoroutine(Idle(0.5f));
            yield return StartCoroutine(MoveToIdleCo(defenselessPos));
            invincibility = false;
            yield return new WaitForSeconds(3f);
            invincibility = true;
            yield return StartCoroutine(MoveToIdleCo(idlePosition));
        }
        
        

    }

    public void TakeDamage(float damage) {
        if (!invincibility) {
            currentHealth -= damage;
        }
        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        isLiving = false;
        StopAllCoroutines();
    }

    IEnumerator turnOffTheLight() {
        Light2D bossRoomLight = GameObject.Find("Boss Room Lantern").GetComponent<Light2D>();
        while (bossRoomLight.shapeLightFalloffSize > 5) {
            bossRoomLight.shapeLightFalloffSize -= 0.1f;
            yield return null;
        }
    }
    private void MoveToIdle() {
        StartCoroutine(MoveToIdleCo(idlePosition));
    }

    IEnumerator Idle(float t)
    {
        yield return new WaitForSeconds(t);
    }

    IEnumerator AppearAnim()
    {
        for (int i = 0; i < 60; i++)
        {
            transform.position -= new Vector3(0, 0.05f);
            yield return null;
        }
        yield return new WaitForSeconds(0.7f);

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 40; j++)
            {
                transform.position -= new Vector3(0, 0.05f);
                yield return null;
            }
            yield return new WaitForSeconds(0.7f);
        }
        transform.position = idlePosition;
    }

    IEnumerator MoveToIdleCo(Vector2 targetPos) {
        Vector2 startPosition = transform.position;
        float t = 0;
        float elapsedTime = 0;
            while (t < 1) {
                elapsedTime += Time.deltaTime;
                t = Mathf.Clamp01(elapsedTime / 1f); // 0.7f is duration
                transform.position = Vector3.Lerp(startPosition, targetPos, t);
                yield return null;
            }
    }

    IEnumerator Dash() {
        Vector3 toPlayerDir = (player.transform.position - transform.position).normalized;
        RaycastHit2D[] dashPoint = Physics2D.RaycastAll(transform.position, toPlayerDir, 50, LayerMask.GetMask("Ground"));
        Vector3 targetPos = Vector3.zero;
        foreach (RaycastHit2D hit in dashPoint) {
            if (Vector2.Distance(hit.point, transform.position) < 30 && Vector2.Distance(hit.point, transform.position) > 2) {
                if (!hit.collider.gameObject.name.Contains("Platform")) {
                    targetPos = hit.point - new Vector2(toPlayerDir.x, toPlayerDir.y) * 2;
                }
            }
        }
        if (targetPos == Vector3.zero) {
            targetPos = transform.position + toPlayerDir * 30;
        }
        

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + toPlayerDir * 30);

        lineRenderer.startColor = startColor;
        lineRenderer.startColor = startColor;

        float elapsedTime = 0f;
        float fadeDuration = 2f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            Color currentColor = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            lineRenderer.startColor = currentColor;
            lineRenderer.endColor = currentColor;
            yield return null;
        }

        lineRenderer.startColor = startColor;
        lineRenderer.endColor = startColor;
        isDashing = true;
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 30 * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isDashing = false;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            if (isDashing) {
                playerSc.TakeDamage(dashDamage, false, 0, transform.position);
            }
        }
    }
}
