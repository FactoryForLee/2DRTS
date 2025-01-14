using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private Vector2Int bottomLeft, topRight;
    [SerializeField] private bool canEnterWall;

    public void ChangeWallEnterState(bool canEnterWall) => this.canEnterWall = canEnterWall;

    public bool IsPathPending { get; private set; } = false;
    private List<WeightNode> openList = new List<WeightNode>();
    private List<WeightNode> closeList = new List<WeightNode>();
    public List<WeightNode> finalList { get; private set; } = new List<WeightNode>();

    private WeightNode curNode;
    private WeightNode[,] weightMap;
    private UniTaskVoid taskCash;

    private void Awake()
    {
        bottomLeft = MapNodeManager.Instance.bottomLeftPos;
        topRight = MapNodeManager.Instance.topRightPos;

        weightMap = new WeightNode[MapNodeManager.Instance.topRightPos.y - MapNodeManager.Instance.bottomLeftPos.y,
                                  MapNodeManager.Instance.topRightPos.x - MapNodeManager.Instance.bottomLeftPos.x];

        for (int i = 0; i < weightMap.GetLength(0); i++)//y
            for (int j = 0; j < weightMap.GetLength(1); j++)//x
                weightMap[i, j] = new WeightNode(j + bottomLeft.y, i + bottomLeft.x);
    }

    public void SetDestination(Vector2 pos)
    {
        PathFind_Async(Vector2Int.RoundToInt(pos)).Forget();
    }

    private async UniTaskVoid PathFind_Async(Vector2Int targetPos)
    {
        IsPathPending = true;
        Vector2Int startPos = Vector2Int.RoundToInt(transform.position);
        openList.Add(weightMap[startPos.y - bottomLeft.y, startPos.x - bottomLeft.x]);

        while (openList.Count > 0)
        {
            //계속된 OpenList 탐색

            curNode = openList[0];

            foreach (WeightNode node in openList)
                if (node.F <= curNode.F && node.H < curNode.H)//openList중에서 가중치가 가장 작은 존재를 먼저 탐색
                    curNode = node;

            openList.Remove(curNode);
            closeList.Add(curNode);

            AddToOpenList(curNode.x + 1, curNode.y + 1);//우상
            AddToOpenList(curNode.x + 1, curNode.y - 1);//우하
            AddToOpenList(curNode.x - 1, curNode.y - 1);//좌하
            AddToOpenList(curNode.x - 1, curNode.y + 1);//좌상

            AddToOpenList(curNode.x, curNode.y + 1);//상
            AddToOpenList(curNode.x, curNode.y - 1);//하
            AddToOpenList(curNode.x - 1, curNode.y);//좌
            AddToOpenList(curNode.x + 1, curNode.y);//우
        }

        await UniTask.CompletedTask;
        IsPathPending = false;
    }

    private void AddToOpenList(int xpos, int ypos)
    {
        //맵 안에 있는지 그리고...닫힌 리스트에 있는지... 그리고... Iswall이 아닌지...근데 Iswall은 없지...

        int checkX = xpos - bottomLeft.x,
            checkY = ypos - bottomLeft.y;

        if (MapNodeManager.Instance.IsInMap(new Vector2(xpos, ypos)) &&//맵에 있는지
            CheckWallOutCost(new Vector2Int(xpos, ypos)) &&//벽을 통과할 수 없다면 벽이 있는지
            closeList.Contains(weightMap[checkY, checkX]))//닫힌 리스트에 노드가 들어있는지
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
        return Mathf.Abs(topRight.x - xPos) + Mathf.Abs(topRight.y - yPos);
    }


    private bool CheckWallOutCost(Vector2Int pos)
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