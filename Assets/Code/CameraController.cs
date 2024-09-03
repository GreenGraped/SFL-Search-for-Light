using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float smoothing = 0.2f;
    [SerializeField] GameObject player;
    public bool isCameraMoving = true;
    public float cameraSize = 10f;
    private Camera mainCamera;
    private Player playerSc;
    private float height;
    private float width;
    private Vector2[] center = new Vector2[5];
    private Vector2[] mapSize = new Vector2[5];

    
    void Awake() {
        mainCamera = Camera.main;
        mainCamera.backgroundColor = Color.black;
        height = mainCamera.orthographicSize;
        width = height * Screen.width / Screen.height;
        center[1] = new Vector2(29.5f, 5f);
        mapSize[1] = new Vector2(100, 20) / 2;
    }

    void Start() {
        playerSc = GameManager.Instance.playerSc;
        player = GameManager.Instance.player;
    }

    private void FixedUpdate()
    {
        if (playerSc.currentLocation == GameManager.Location.Home) {
            CameraFix(new Vector2(-50, 1));
        }
        else {
            if (isCameraMoving) {
                cameraFollowing();
            }
        }
    }

    public void changeCameraSize(float s) {
        cameraSize = s;
        mainCamera.orthographicSize = s;
    }
    
    public void changeCameraSizeSmooth(float s) {
        StartCoroutine(smoothCameraSize(s));
        
    }

    IEnumerator smoothCameraSize(float targetSize) {
        float initialSize = mainCamera.orthographicSize;
        float timeElapsed = 0f;
        float duration = 0.5f; // 부드럽게 변경하는 데 걸리는 시간 (초 단위)

        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration); // 비율을 0과 1 사이로 고정
            mainCamera.orthographicSize = Mathf.Lerp(initialSize, targetSize, t);
            yield return null;
        }

        // 루프가 끝난 후, 정확히 목표 크기로 설정
        mainCamera.orthographicSize = targetSize;
    }

    private void CameraFix(Vector2 pos) {
        transform.position = new Vector3(pos.x, pos.y, -10);
    } // 집 안에서 밖으로 나갈 때 카메라가 부드럽게 따라감
    private void cameraFollowing() {
        float lx = 0;
        float clampX = 0;
        float ly = 0;
        float clampY = 0;
        if (playerSc.currentLocation == GameManager.Location.Cp1)
        {
            lx = mapSize[1].x - width;
            clampX = Mathf.Clamp(player.transform.position.x, center[1].x - lx, center[1].x + lx);
            ly = mapSize[1].y - height;
            clampY = Mathf.Clamp(player.transform.position.y, center[1].y - ly, center[1].y + ly);
        }
        Vector3 targetPos = new Vector3(clampX, clampY, this.transform.position.z);
        Debug.Log(targetPos);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }
    public void MoveCamera(Vector2 pos) {
        mainCamera.transform.position = new Vector3(pos.x, pos.y, -10);
    }
    public void MoveCameraSmooth(Vector2 pos) {
        StartCoroutine(moveCamera(pos));
    }
    IEnumerator moveCamera(Vector2 pos) {
        Vector3 target = new Vector3(pos.x, pos.y, -10);
        while (Vector2.Distance(transform.position, target) > 0.001f) {
            transform.position = Vector3.Lerp(transform.position, target, 0.1f);
            Debug.Log(Vector3.Lerp(transform.position, target, 0.1f));
            yield return null;
        }
        CameraFix(pos);
    }
}
