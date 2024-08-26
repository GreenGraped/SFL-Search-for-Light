using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [HideInInspector] public GameObject Lantern;
    [HideInInspector] public GameObject GlobalLight;
    public GameObject player;
    public Player playerSc;
    public GameObject mainCamera;
    public CameraController cameraCon;
    public WeaponManager weaponManager;
    public DialogueManager dialogueManager;
    public int currentDialogId;
    public Mephistopeles Mep;

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
        init();
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(player.gameObject);
        DontDestroyOnLoad(mainCamera.gameObject);
        playerSc = player.GetComponent<Player>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void init() {
        weaponManager = GetComponent<WeaponManager>();
        dialogueManager = GetComponent<DialogueManager>();
        player = GameObject.Find("Player");
        playerSc = player.GetComponent<Player>();
        mainCamera = GameObject.Find("MainCamera");
        cameraCon = mainCamera.GetComponent<CameraController>();
        GlobalLight = GameObject.Find("GlobalLight");
    }


    void Start() {
        
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
            Mep = GameObject.Find("Mephistopeles").GetComponent<Mephistopeles>();
            playerSc.hasLantern = false;
            Lantern.transform.position = new Vector2(0, -2);
            player.transform.position = new Vector2(-15, 0);
            playerSc.currentLocation = Location.Cp1;
            cameraCon.MoveCamera(new Vector2(-5, 0));
        }
        else if (scene.name == "Intro") {
            dialogueManager.canvas = GameObject.Find("Dialog");
            dialogueManager.dialogText = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
            StartTalk(100);
            DontDestroyOnLoad(dialogueManager.canvas);
        }
        init();
    }

    public void StartTalk(int dialogId) {
        dialogueManager.talkIndex = 0;
        currentDialogId = dialogId;
        dialogueManager.talk(dialogId);
    }
    
}
