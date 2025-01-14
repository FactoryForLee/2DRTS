using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Define;
using System;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
    public List<MapNode> bindedNodes { get; private set; } = new List<MapNode>();
    private BoxCollider2D col;
    [SerializeField] private Vector2Int size;
    public Vector2Int Size => size;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

#if UNITY_EDITOR
    [ContextMenu("GetSize")]
    private void GetSize()
    {
        size = Vector2Int.RoundToInt(GetComponent<BoxCollider2D>().size);        
        EditorUtility.SetDirty(gameObject);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif

    private void OnDisable()
    {
        foreach (MapNode node in bindedNodes)
            node.RemoveBuilding();

        bindedNodes.Clear();
    }
}