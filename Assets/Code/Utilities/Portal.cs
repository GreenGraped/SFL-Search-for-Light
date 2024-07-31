using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private bool isSceneMove;
    [SerializeField] private string sceneName;
    [SerializeField] private Vector2 playerPos;
    void OnTriggerEnter2D(Collider2D other) {
        if (isSceneMove) {
            
            SceneLoader.Instance.LoadScene(sceneName);
        }
        else {
            GameManager.Instance.player.transform.position = playerPos;
            GameManager.Instance.cameraCon.MoveCamera(playerPos);
        }
        
    }
}
