using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class Mephistopeles : MonoBehaviour
{
    private GameObject player;
    private Player playerSc;

    private Vector2 idlePosition = new Vector2(123, 5);

    void Start() {
        player = GameManager.Instance.player;
        playerSc = GameManager.Instance.playerSc;
    }

    public void BossStart()
    {
        transform.position = new Vector2(123, 12);
        MoveToIdle();
    }


    private void MoveToIdle() {
        StartCoroutine(MoveToIdleCo());
    }

    IEnumerator MoveToIdleCo() {
        Vector2 startPosition = transform.position;
        float t = 0;
        float elapsedTime = 0;
            while (t < 1) {
                elapsedTime += Time.deltaTime;
                t = Mathf.Clamp01(elapsedTime / 1f); // 0.7f is duration
                transform.position = Vector3.Lerp(startPosition, idlePosition, t);
                yield return null;
        }
    }

    private void Dash() {
        Vector2 toPlayerDir = (player.transform.position - transform.position).normalized;
    }


}
