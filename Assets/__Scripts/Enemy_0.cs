using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0 : Enemy
{
    void Awake()
    {
        base.Awake();
        score = 50; // Basic enemy with lowest score
    }
}
