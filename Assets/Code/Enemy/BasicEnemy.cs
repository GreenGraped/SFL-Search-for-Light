using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    public BasicEnemy() {
        this.moveSpeed = 2f;
        this.maxHealth = 100f;
        this.sightRange = 3f;
    }
}
