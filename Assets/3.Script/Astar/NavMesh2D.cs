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
    [SerializeField] private float pathFindThresHold;

    private float curThresHold;
    public UnityAction<MapNode> OnNodeHasBuilding;
    private Vector2 prevDest;

    private void Start()
    {
        path = new PathFinding(transform);
        path.OnPathFinded += MoveToPath;
    }

    public void SetDestination(Vector2 destination)
    {
        curThresHold += Time.deltaTime;

        if (destination == prevDest ||
            curThresHold < pathFindThresHold ||
            movement.isMoving) return;

        curThresHold = 0.0f;
        prevDest = destination;
        StopCoroutine(movement.MoveFixed_co(transform, destination));
        prevDest = destination;
        path.GetPath(destination);
    }

    private void MoveToPath(List<MapNode> path)
    {
        StartCoroutine(MoveToPath_co(path));
    }

    private IEnumerator MoveToPath_co(List<MapNode> path)
    {
        movement.ResetStopDistance();

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
        }
    }

    private void OnDisable()
    {
        path.OnPathFinded -= MoveToPath;
    }
}
