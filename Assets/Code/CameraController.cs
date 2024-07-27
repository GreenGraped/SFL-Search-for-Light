using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float smoothing = 0.2f;
    [SerializeField] GameObject player;
    private Camera mainCamera;
    private Player playerSc;
    
    void Start() {
        mainCamera = Camera.main;
        mainCamera.backgroundColor = Color.black;
        playerSc = player.GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        if (playerSc.currentLocation == GameManager.Location.Home) {
            CameraFix(new Vector2(-50, 1));
        }
        else {
            cameraFollowing();
        }
    }
    private void CameraFix(Vector2 pos) {
        transform.position = new Vector3(pos.x, pos.y, -10);
    } // 집 안에서 밖으로 나갈 때 카메라가 부드럽게 따라감
    private void cameraFollowing() {
        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }
    public void MoveCamera(Vector2 pos) {
        mainCamera.transform.position = new Vector3(pos.x, pos.y, -10);
    }
}
