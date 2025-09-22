using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NavMesh2D : MonoBehaviour
{
    [SerializeField] private Movement2D movement;
    [SerializeField] private PathFinding path;
    [SerializeField] private float stoppingDistance;

    public UnityAction<MapNode> OnNodeHasSomthing;
    private Vector2 prevDest;

    private Coroutine movePath_coCash;

    private void Start()
    {
        movement.rb = GetComponent<Rigidbody2D>();
        path = new PathFinding(transform);
        path.OnPathFinded += MoveToPath;
    }

    public void SetDestination(Vector2 destination)
    {
        if (destination == prevDest) return;

        prevDest = destination;
        path.GetPath(destination);
    }

    private void MoveToPath(List<MapNode> path)
    {
        if (movePath_coCash != null)
            StopCoroutine(movePath_coCash);

        movePath_coCash = StartCoroutine(MoveToPath_co(path));
    }

    private IEnumerator MoveToPath_co(List<MapNode> path)
    {
        movement.ResetStopDistance();

        for (int i = 0; i < path.Count; i++)
        {
            if(path[i].Cost > 0)
            {
                OnNodeHasSomthing?.Invoke(path[i]);
                yield break;
            }

            if (i == path.Count - 1)
                movement.AssignStopDistance(stoppingDistance);

            yield return movement.MoveFixed_co(path[i].transform.position);
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
