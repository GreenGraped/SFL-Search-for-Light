using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public float cooldown;
    [HideInInspector] public float attackRange;

    public abstract void Attack();
}
