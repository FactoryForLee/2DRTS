using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using Define;

/// <summary>
/// 각 노드의 간격은 0.1f 에디터에서 노드를 생성
/// 노드에 변화가 생길 때마다 이를 해당 메니저에서 변경
/// </summary>
public class MapNodeManager : Singleton<MapNodeManager>
{
    [Tooltip("Must be place bigger than \'bottomRight\'`s Coordinate.")]
    [SerializeField] private Transform topRight;
    [Tooltip("Must be place smaller than \'bottomLeft\'`s Coordinate.")]
    [SerializeField] private Transform bottomLeft;

    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private MapCostSO mapCostSO;
    [Tooltip("Key = LayerMask, Value = Cost | 건물의 Cost 접근을 위해")]
    public Dictionary<int, int> Cost_Diction { get; } = new Dictionary<int, int>();

    [Tooltip("[y, x]")]
    [field: SerializeField] public ArrayHolder<MapNode>[] Maps;

    protected override void Init()
    {
        foreach (MapCostSO.CostByLayer costByLayer in mapCostSO.LayerCosts)
            Cost_Diction.Add(costByLayer.Layer, costByLayer.Cost);
    }

    #region 에디터에서의 노드 생성 로직
#if UNITY_EDITOR
    public void GenerateNodes()
    {
        Vector2Int bounds = Vector2Int.RoundToInt(topRight.position - bottomLeft.position);

        Maps = new ArrayHolder<MapNode>[bounds.y + 1];
        Transform nodeParent = new GameObject("NodeParent").transform;
        nodeParent.position = Vector3.zero;
        int nodeCount = 0;

        for (int i = 0; i < Maps.Length; i++)//Y 
        {
            Maps[i] = new ArrayHolder<MapNode>(new MapNode[bounds.x + 1]);
            for (int j = 0; j < Maps[i].Arr.Length; j++)//X
            {
                Vector2 pos = GetPosByIndex(j,i);

                MapNode newNode = new GameObject("Node").AddComponent<MapNode>();
                newNode.transform.SetParent(nodeParent);
                newNode.transform.position = pos;
                Maps[i].Arr[j] = newNode;
                nodeCount++;
            }
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log(nodeCount + "개의 노드가 생성됨");
    }

    public void GenerateBoundobjs()
    {
        bottomLeft = new GameObject("BottomLeft").transform;
        topRight = new GameObject("TopRight").transform;
        bottomLeft.SetParent(transform);
        topRight.SetParent(transform);
        bottomLeft.localPosition = Vector3.zero;
        topRight.localPosition = Vector3.zero;
    }
#endif
    #endregion

    /// <summary>
    /// 노드의 인덱스를 Bottom Right에서 더하여 World Postion의 Vector2로 반환
    /// </summary>
    /// <param name="xIndex"></param>
    /// <param name="yIndex"></param>
    /// <returns></returns>
    private Vector2 GetPosByIndex(int xIndex, int yIndex)
    {
        return new Vector2(xIndex + bottomLeft.position.x, yIndex + bottomLeft.position.y);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MapNodeManager))]
public class GenerateNodeButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapNodeManager map = (MapNodeManager)target;


        if (GUILayout.Button("Generate Nodes"))
            map.GenerateNodes();

        if (GUILayout.Button("Generate Bound Objects"))
            map.GenerateBoundobjs();
    }
}
#endif