using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Define;
using System;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
    private List<MapNode> bindedNodes = new List<MapNode>();
    private BoxCollider2D col;
    [SerializeField] private Vector2 size;
    public Vector2 Size => size;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnDeploy()
    {
        bindedNodes = GetBindNodes();
    
        foreach (MapNode node in bindedNodes)
            node.AssignBuilding(this);
    }

    private List<MapNode> GetBindNodes()
    {
        //TODO: 빌딩 로직 꼭 완성하기!
        return null;
    }

#if UNITY_EDITOR
    [ContextMenu("GetSize")]
    private void GetSize()
    {
        size = GetComponent<BoxCollider2D>().size;
        EditorUtility.SetDirty(gameObject);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif

    public bool CanBuild(List<MapNode> map) => GetBindNodes().Count == 0;

    private void OnDisable()
    {
        foreach (MapNode node in bindedNodes)
            node.RemoveBuilding();

        bindedNodes.Clear();
    }
}