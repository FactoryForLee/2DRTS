using UnityEngine;
using UnityEngine.Events;
using Define;
using System;
using System.Collections.Generic;

public abstract class Building : MonoBehaviour
{
    private List<MapNode> bindedNodes = new List<MapNode>();

    protected void OnEnable()
    {
        OnDeploy();
    }

    private void OnDeploy()
    {
        bindedNodes = GetBindNodes();

        foreach (MapNode node in bindedNodes)
            node.AssignBuilding(this);
    }

    protected abstract List<MapNode> GetBindNodes();

    private void OnDisable()
    {
        foreach (MapNode node in bindedNodes)
            node.RemoveBuilding();

        bindedNodes.Clear();
    }
}