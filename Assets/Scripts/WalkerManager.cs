using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalkerManager : MonoBehaviour
{
    public static WalkerManager Instance;
    
    public GameObject walkerObject;

    public bool is3D;
    public float walkerZoneRadius;
    public float targetWalkerCount;
    public float forceMagnitude;
    public float restoringForcePower;
    public float maxSpeed;
    public float particleHue;
    [HideInInspector] public float maxWalkerCenterDistance;

    private Transform _structureRoot;
    private float _structureRadius = 0f;

    private Slider _walkerZoneRadiusSlider;
    private Slider _targetWalkerCountSlider;
    private Slider _forceMagnitudeSlider;
    private Slider _restoringForcePowerSlider;
    private Slider _maxSpeedSlider;
    private Slider _particleHueSlider;

    private int WalkerCount => transform.childCount;

    private void Awake()
    {
        Instance = this;

        _structureRoot = GameObject.FindWithTag("StructureRoot").transform;
        maxWalkerCenterDistance = walkerZoneRadius;
        
        _walkerZoneRadiusSlider = GameObject.Find("ZoneRadiusS").GetComponent<Slider>();
        _targetWalkerCountSlider = GameObject.Find("WalkerDensityS").GetComponent<Slider>();
        _forceMagnitudeSlider = GameObject.Find("RandomForceS").GetComponent<Slider>();
        _restoringForcePowerSlider = GameObject.Find("RestoreForceS").GetComponent<Slider>();
        _maxSpeedSlider = GameObject.Find("MaxSpeedS").GetComponent<Slider>();
        _particleHueSlider = GameObject.Find("ParticleColorHueS").GetComponent<Slider>();
    }

    private void Update()
    {
        walkerZoneRadius = _walkerZoneRadiusSlider.value;
        targetWalkerCount = _targetWalkerCountSlider.value;
        forceMagnitude = _forceMagnitudeSlider.value;
        restoringForcePower = _restoringForcePowerSlider.value;
        maxSpeed = _maxSpeedSlider.value;
        particleHue = _particleHueSlider.value;
        
        float currentWalkerDensity = WalkerCount;
        if (currentWalkerDensity < targetWalkerCount)
        {
            CreateWalker(maxWalkerCenterDistance);
        }
        else if (currentWalkerDensity > targetWalkerCount)
        {
            
            DestroyWalker();
        }
    }
    
    public void RecalculateStructureRadius()
    {
        Transform latestChild = _structureRoot.GetChild(_structureRoot.childCount - 1);
        _structureRadius = Mathf.Max(latestChild.position.magnitude, _structureRadius);
        maxWalkerCenterDistance = walkerZoneRadius + _structureRadius;
    }

    public void ToggleDimension()
    {
        is3D = !is3D;
    }

    public void ResetSim()
    {
        int numberOfChildren = transform.childCount;
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < numberOfChildren; i++)
        {
            if (i != 0) // Exclude the first child
            {
                children.Add(transform.GetChild(i));
            }
        }

        foreach (Transform child in children)
        {
            GameObject.Destroy(child.gameObject);
        }

        numberOfChildren = _structureRoot.childCount;
        children.Clear(); // Reuse the list for _structureRoot

        for (int i = 0; i < numberOfChildren; i++)
        {
            if (i != 0) // Exclude the first child
            {
                children.Add(_structureRoot.GetChild(i));
            }
        }

        foreach (Transform child in children)
        {
            GameObject.Destroy(child.gameObject);
        }

        _structureRadius = 0f;
        maxWalkerCenterDistance = walkerZoneRadius;
    }

    private void CreateWalker(float distanceFromCenter)
    {
        Vector3 position = Vector3.zero;
        if (is3D)
        {
            position = VectorUtils.RandomVectorOnSphere(Vector3.zero, distanceFromCenter);
        }
        else
        {
            position = VectorUtils.RandomVectorOnCircle(Vector2.zero, distanceFromCenter);
            position.z = 0f;
        }
        GameObject walker = Instantiate(walkerObject, position, Quaternion.identity, transform);
    }

    private void DestroyWalker()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
