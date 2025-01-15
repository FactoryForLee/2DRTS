using UnityEngine;
using System;
using Define;
using System.Collections;
using DG.Tweening;

[Serializable]
public class Movement2D
{
    [SerializeField] private float speed;
    [SerializeField] private float defaultDistance;
    [SerializeField] private float stopDistance;
    public bool isMoving { get; private set; } = false;

    public void AssignStopDistance(float distance) => stopDistance = distance;
    public void ResetStopDistance() => stopDistance = defaultDistance;

    public IEnumerator MoveFixed_co(Transform mover, Vector3 dest)
    {
        Vector2 start = mover.position;
        Vector2 dir = dest - mover.position;
        isMoving = true;

        while (dir.magnitude > stopDistance)
        {
            dir = dest - mover.position;
            mover.Translate(dir.normalized * speed * Time.deltaTime);
            yield return StaticValues.waitForFixed;
        }

        isMoving = false;
    }
}
