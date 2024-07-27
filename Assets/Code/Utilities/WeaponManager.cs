using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Sword swordPrefab;
    public Sword sword;

    void Awake() {
        swordPrefab = Resources.Load<Sword>("Prefabs/Sword");
        sword = Instantiate(swordPrefab);
        DontDestroyOnLoad(sword);
    }


}
