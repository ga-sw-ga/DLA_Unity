using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    private Vector3 _currentVelocity;
    
    public bool IsStuck { get; private set; }
}
