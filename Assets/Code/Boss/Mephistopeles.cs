using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class Mephistopeles : MonoBehaviour
{
    private GameObject player;
    private Player playerSc;

    private Vector2 idlePosition = new Vector2(123, 5);
    private bool wait = false;

    void Start() {
        player = GameManager.Instance.player;
        playerSc = GameManager.Instance.playerSc;
    }

    public void BossStart()
    {
        Appear();
        //if (!wait)
        //{
        //    MoveToIdle();
        //}
        //if (!wait)
        //{
        //    StartCoroutine(Idle(3f));
        //}
        //if (!wait)
        //{
        //    Debug.Log("asdf");
        //}
        // 함수 끝나고 다음 함수 실행?
        
    }

    private void Appear()
    {
        transform.position = new Vector2(123, 12);
        StartCoroutine(AppearAnim());

    }
    private void MoveToIdle() {
        wait = true;
        StartCoroutine(MoveToIdleCo());
    }

    IEnumerator Idle(float t)
    {
        wait = true;
        yield return new WaitForSeconds(t);
        wait = false;
    }

    IEnumerator AppearAnim()
    {
        wait = true;
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
        wait = false;
        transform.position = idlePosition;
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
        wait = false;
    }

    private void Dash() {
        Vector2 toPlayerDir = (player.transform.position - transform.position).normalized;
    }


}
