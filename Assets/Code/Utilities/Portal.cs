using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Vector2 playerPos;
    void OnTriggerEnter2D(Collider2D other) {
        GameManager.Instance.sceneManagement.LoadScene(sceneName);
        GameManager.Instance.player.transform.position = playerPos;
        GameManager.Instance.cameraCon.MoveCamera(playerPos);
    }
}
