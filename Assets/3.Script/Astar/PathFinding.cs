using Define;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[Serializable]
public class PathFinding
{
    [SerializeField] private Vector2Int bottomLeft;
    [SerializeField] private Vector2Int topRight;
    [SerializeField] private bool canEnterWall;
    [SerializeField] private bool loopBugTest = false;
    public UnityAction<List<MapNode>> OnPathFinded;
    public UnityAction OnStartFinding;
    private Transform transform;

    public void ChangeWallEnterState(bool canEnterWall) => this.canEnterWall = canEnterWall;

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
        topRight = MapNodeManager.Instance.topRightPos;

        weightMap = new WeightNode[topRight.y - bottomLeft.y + 1, topRight.x - bottomLeft.x + 1];

        for (int i = 0; i < weightMap.GetLength(0); i++)//y
            for (int j = 0; j < weightMap.GetLength(1); j++)//x
                weightMap[i,j] = new WeightNode(j + bottomLeft.x, i + bottomLeft.y);
    }

    public void GetPath(Vector2 pos)
    {
        ClearWeightNodes(openList);
        ClearWeightNodes(closeList);

        foreach (MapNode node in finalList)
            weightMap[Mathf.RoundToInt(node.transform.position.y),
                Mathf.RoundToInt(node.transform.position.x)].ClearGnH();
        finalList.Clear();

        pos.x = Mathf.Clamp(pos.x, bottomLeft.x, topRight.x);
        pos.y = Mathf.Clamp(pos.y, bottomLeft.y, topRight.y);

        targetNode = weightMap[Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.x)];
        startNode = weightMap[Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.x)];
        
        PathFind();
    }

    private void PathFind()
    {
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

            if (curNode == targetNode)
            {
                while (targetNode != startNode)
                {
                    finalList.Add(MapNodeManager.Instance.GetNodeByPos(targetNode.x, targetNode.y));
                    targetNode = targetNode.prev;
                }
            
                finalList.Reverse();
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
        OnPathFinded?.Invoke(finalList);
        return;
    }

    private void AddToOpenList(int xpos, int ypos)
    {
        //맵 안에 있는지 그리고...닫힌 리스트에 있는지... 그리고... Iswall이 아닌지...근데 Iswall은 없지...
        int checkX = xpos - bottomLeft.x,
            checkY = ypos - bottomLeft.y;
        Vector2 neighborPos = new Vector2(xpos, ypos);

        if (MapNodeManager.Instance.IsInMap(neighborPos) &&//맵에 있는지
            CheckWallCost(Vector2Int.RoundToInt(neighborPos)) &&//벽을 통과할 수 없다면 벽이 있는지
            !closeList.Contains(weightMap[checkY, checkX]))//닫힌 리스트에 노드가 들어있는지
        {
            WeightNode neighborNode = weightMap[checkY, checkX];
            int moveCost = curNode.G + ((curNode.x - xpos) == 0 || (curNode.y - ypos) == 0 ? 10 : 14);

            moveCost += MapNodeManager.Instance.GetNodeByPos(xpos, ypos).Cost;

            if (moveCost < curNode.G || !openList.Contains(neighborNode))
            {
                weightMap[checkY, checkX].G = moveCost;
                weightMap[checkY, checkX].H = GetHeuristic(neighborNode);
                weightMap[checkY, checkX].prev = curNode;
                openList.Add(weightMap[checkY, checkX]);
            }
        }
    }

    private void ClearWeightNodes(List<WeightNode> nodes)
    {
        foreach (WeightNode node in nodes)
            node.ClearGnH();
        nodes.Clear();
    }

    private int GetHeuristic(WeightNode node)
    {
        return Mathf.Abs(targetNode.x - node.x) + Mathf.Abs(targetNode.y - node.y);
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
        ClearGnH();
    }

    public void ClearGnH()
    {
        G = 0;
        H = 0;
        prev = null;
    }

    public int y, x;
    public WeightNode prev;
    public int G, H;
    public int F => G + H;
}