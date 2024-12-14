using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumWalkerText : MonoBehaviour
{
    private bool hasPaused;
    private TextMeshProUGUI text;
    private Transform _structureRoot;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        _structureRoot = GameObject.FindWithTag("StructureRoot").transform;
    }

    private void Update()
    {
        text.text = _structureRoot.childCount.ToString();

        if (!hasPaused && _structureRoot.childCount >= 250)
        {
            WalkerManager.Instance.GetComponent<GameObjectUtils>().ToggleActive();
            hasPaused = true;
        }
    }
}
