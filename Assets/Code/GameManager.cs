using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject player;
    public GameObject Lantern;
    public GameObject GlobalLight;
    public GameObject mainCamera;
    public Sword sword;

    public enum Location {
        Home,
        Ground,
        Cp1
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveCamera(Vector2 pos) {
        mainCamera.transform.position = new Vector3(pos.x, pos.y, -10);
    }
}
