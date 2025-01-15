using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

[Serializable]
public class PathFinding
{
    [SerializeField] private Vector2Int bottomLeft;
    [SerializeField] private Vector2Int topRight;
    [SerializeField] private bool canEnterWall;
    public UnityAction<List<MapNode>> OnPathFinded;
    public UnityAction OnStartFinding;
    private Transform transform;

    public void ChangeWallEnterState(bool canEnterWall) => this.canEnterWall = canEnterWall;

    public bool IsPathPending { get; private set; } = false;
    private List<WeightNode> openList = new List<WeightNode>();
    private List<WeightNode> closeList = new List<WeightNode>();
    public List<MapNode> finalList { get; private set; } = new List<MapNode>();

    private WeightNode startNode;
    private WeightNode curNode;
    private WeightNode targetNode;
    private WeightNode[,] weightMap;

    public PathFinding(Transform transform)
    {
        this.transform = transform;

        bottomLeft = MapNodeManager.Instance.bottomLeftPos;

        weightMap = new WeightNode[MapNodeManager.Instance.topRightPos.y - MapNodeManager.Instance.bottomLeftPos.y + 1,
                                  MapNodeManager.Instance.topRightPos.x - MapNodeManager.Instance.bottomLeftPos.x + 1];

        for (int i = 0; i < weightMap.GetLength(0); i++)//y
            for (int j = 0; j < weightMap.GetLength(1); j++)//x
                weightMap[i, j] = new WeightNode(j + bottomLeft.y, i + bottomLeft.x);
    }

    public void GetPath(Vector2 pos)
    {
        if (IsPathPending) return;
        pos.x = Mathf.Clamp(pos.x, bottomLeft.x, topRight.x);
        pos.y = Mathf.Clamp(pos.y, bottomLeft.y, topRight.y);

        targetNode = weightMap[Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.x)];
        startNode = weightMap[Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.x)];

        openList.Clear();
        closeList.Clear();
        finalList.Clear();
        PathFind();
    }

    private void PathFind()
    {
        IsPathPending = true;
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            //계속된 OpenList 탐색

            curNode = openList[0];

            foreach (WeightNode node in openList)
                if (node.F <= curNode.F && node.H < curNode.H)//openList중에서 가중치가 가장 작은 존재를 먼저 탐색
                    curNode = node;

            openList.Remove(curNode);
            closeList.Add(curNode);

            if (curNode.Equals(targetNode))
            {
                while (!curNode.Equals(startNode))
                {
                    finalList.Add(MapNodeManager.Instance.GetNodeByPos(curNode.x, curNode.y));
                    curNode = curNode.prev;
                }

                finalList.Reverse();
                OnPathFinded?.Invoke(finalList);
                break;
            }

            AddToOpenList(curNode.x + 1, curNode.y + 1);//우상
            AddToOpenList(curNode.x + 1, curNode.y - 1);//우하
            AddToOpenList(curNode.x - 1, curNode.y - 1);//좌하
            AddToOpenList(curNode.x - 1, curNode.y + 1);//좌상

            AddToOpenList(curNode.x, curNode.y + 1);//상
            AddToOpenList(curNode.x, curNode.y - 1);//하
            AddToOpenList(curNode.x - 1, curNode.y);//좌
            AddToOpenList(curNode.x + 1, curNode.y);//우
        }

        Debug.Log("Path Finding is Complete.");
        IsPathPending = false;
    }

    private void AddToOpenList(int xpos, int ypos)
    {
        //맵 안에 있는지 그리고...닫힌 리스트에 있는지... 그리고... Iswall이 아닌지...근데 Iswall은 없지...

        int checkX = xpos - bottomLeft.x,
            checkY = ypos - bottomLeft.y;

        if (MapNodeManager.Instance.IsInMap(new Vector2(xpos, ypos)) &&//맵에 있는지
            CheckWallCost(new Vector2Int(xpos, ypos)) &&//벽을 통과할 수 없다면 벽이 있는지
            !closeList.Contains(weightMap[checkY, checkX]))//닫힌 리스트에 노드가 들어있는지
        {
            int dirCost = (curNode.x - xpos) + (curNode.y - ypos) == 0 ? 10 : 14;
            dirCost += MapNodeManager.Instance.GetNodeByPos(xpos, ypos).Cost;

            weightMap[checkY, checkX].G = dirCost + curNode.G;
            weightMap[checkY, checkX].H = GetHeuristic(xpos, ypos);
            weightMap[checkY, checkX].prev = curNode;
            openList.Add(weightMap[checkY, checkX]);
        }
    }

    private int GetHeuristic(int xPos, int yPos)
    {
        return Mathf.Abs(targetNode.x - xPos) + Mathf.Abs(targetNode.y - yPos);
    }


    private bool CheckWallCost(Vector2Int pos)
    {
        return canEnterWall || MapNodeManager.Instance.GetNodeByPos(pos).Cost == 0;
    }
}

public class WeightNode
{
    public WeightNode(int x, int y)
    {
        this.y = y;
        this.x = x;
    }

    public int y, x;
    public WeightNode prev;
    public int G, H;
    public int F => G + H;
}