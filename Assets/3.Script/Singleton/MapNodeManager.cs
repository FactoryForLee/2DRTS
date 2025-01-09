using UnityEngine;
using System.Collections.Generic;

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
    [field : SerializeField] public MapNode[,] maps { get; private set; }

    protected override void Init()
    {
        Vector2Int bounds = Vector2Int.RoundToInt(topRight.position - bottomLeft.position);

        foreach (MapCostSO.CostByLayer costByLayer in mapCostSO.LayerCosts)
            Cost_Diction.Add(costByLayer.Layer, costByLayer.Cost);

        maps = new MapNode[bounds.y, bounds.x];

        for (int i = 0; i < maps.GetLength(0); i++)//Y 
        {
            for (int j = 0; j < maps.GetLength(1); j++)//X
            {
                Vector2 pos = new Vector2(bottomLeft.position.x + j, bottomLeft.position.y + i);
                Collider2D obstacle = Physics2D.OverlapCircle(pos, 0.4f, obstacleLayer);
                int _layer = -1;

                if (obstacle != null)
                    _layer = 1 << obstacle.gameObject.layer;

                //TODO: [이준형] 건물에 따른 가중치 추가 필요 Dictionary에 Key : Layer, Item : Cost이런 식으로 구현

                int value = 0;
                Cost_Diction.TryGetValue(_layer, out value);

                //maps[i,j] = new MapNode(_pos, bottomLeft.position, _layer, value);
            }
        }
    }



    public Vector2 ConvIndexToCoord(int xIndex, int yIndex)
    {
        return new Vector2(xIndex + bottomLeft.position.x, yIndex + +bottomLeft.position.y);
    }
}