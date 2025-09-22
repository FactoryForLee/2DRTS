using UnityEngine;
using System;
using Define;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;

[Serializable]
public class Movement2D
{
    [SerializeField] private float speed;
    [SerializeField] private float defaultDistance;
    [SerializeField] private float stopDistance;
    public Rigidbody2D rb;

    public void AssignStopDistance(float distance) => stopDistance = distance;
    public void ResetStopDistance() => stopDistance = defaultDistance;

    public IEnumerator MoveFixed_co(Vector2 dest)
    {
        Vector2 dir = dest - rb.position;

        while (dir.magnitude > stopDistance)
        {
            rb.MovePosition(rb.position + dir.normalized * speed * Time.fixedDeltaTime);
            dir = dest - rb.position;
            yield return StaticValues.waitForFixed;
        }
    }
}
