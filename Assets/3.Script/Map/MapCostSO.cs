using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MapCostSO", menuName = "Scriptable Objects/MapCostSO")]
public class MapCostSO : ScriptableObject
{
    [SerializeField] private List<CostByLayer> layerCosts;
    public List<CostByLayer> LayerCosts => layerCosts;

    [Serializable]
    public class CostByLayer
    {
        [SerializeField] private LayerMask layer;
        [SerializeField] private int cost;

        public int Cost => cost;
        public LayerMask Layer => layer;
    }
}