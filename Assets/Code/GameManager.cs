using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject player;
    public GameObject Lantern;
    public GameObject GlobalLight;
    public GameObject mainCamera;
    public CameraController cameraCon;
    public SceneManagement sceneManagement;
    public WeaponManager weaponManager;
    private Player playerSc;

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
        DontDestroyOnLoad(player.gameObject);
        DontDestroyOnLoad(mainCamera.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;    
    }

    void Start() {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        playerSc = player.GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Intro" || scene.name == "Chapter1") {
            Lantern = GameObject.Find("Lantern");
        }
        if (scene.name == "Chapter1") {
            playerSc.hasLantern = false;
            Lantern.transform.position = new Vector2(15, -2);
        }
        GlobalLight = GameObject.Find("GlobalLight");
    }

    
}
