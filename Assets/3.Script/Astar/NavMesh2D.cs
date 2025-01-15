using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NavMesh2D : MonoBehaviour
{
    [SerializeField] private Movement2D movement = new Movement2D();
    [SerializeField] private PathFinding path;

    [SerializeField] private float stoppingDistance;
    public UnityAction<MapNode> OnNodeHasBuilding;
    private Vector2 prevDest;
    private bool isMoving;

    private void Start()
    {
        path = new PathFinding(transform);
        path.OnPathFinded += MoveToPath;
        path.OnStartFinding += CancelMove;
    }

    public void SetDestination(Vector2 destination)
    {
        if (!path.IsPathPending && !prevDest.Equals(destination))
        {
            Debug.Log("길 찾기 시작");
            prevDest = destination;
            path.GetPath(destination);
        }
    }

    private void MoveToPath(List<MapNode> path)
    {
        StopCoroutine(MoveToPath_co(path));
        StartCoroutine(MoveToPath_co(path));
    }

    private void CancelMove()
    {
        StopCoroutine(MoveToPath_co(null));
    }

    private IEnumerator MoveToPath_co(List<MapNode> path)
    {
        movement.ReturnSet();

        for (int i = 0; i < path.Count; i++)
        {
            if(path[i].Cost > 0)
            {
                OnNodeHasBuilding?.Invoke(path[i]);
                yield break;
            }

            if (i == path.Count - 1)
                movement.AssignStopDistance(stoppingDistance);

            yield return movement.MoveFixed_co(transform, path[i].transform.position);
        }
    }

    private void OnEnable()
    {
        if (path != null)
        {
            path.OnPathFinded += MoveToPath;
            path.OnStartFinding += CancelMove;
        }
    }

    private void OnDisable()
    {
        path.OnPathFinded -= MoveToPath;
        path.OnStartFinding -= CancelMove;
    }
}
