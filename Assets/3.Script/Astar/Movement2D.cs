using UnityEngine;
using System;
using Define;
using System.Collections;

[Serializable]
public class Movement2D
{
    [SerializeField] private float speed;
    [SerializeField] private float defaultDistance;
    [SerializeField] private float stopDistance;

    public void AssignStopDistance(float distance) => stopDistance = distance;
    public void ReturnSet() => stopDistance = defaultDistance;
    

    public IEnumerator MoveFixed_co(Transform mover, Vector3 dest)
    {
        float lerpFactor = 0.0f;
        Vector2 start = mover.position;

        while ((dest - mover.position).magnitude > stopDistance)
        {
            mover.position = Vector2.Lerp(start, dest, lerpFactor);
            lerpFactor += speed * Time.fixedDeltaTime;
            yield return StaticValues.waitForFixed;
        }
    }
}
