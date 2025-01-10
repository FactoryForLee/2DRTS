using UnityEngine;
using System;
using Define;
using System.Collections;

public class Movement2D : MonoBehaviour
{
    [SerializeField] private float speed;

    public void SetPos(Vector2 dest)
    {
        StopCoroutine(MoveFixed_co(dest));
        StartCoroutine(MoveFixed_co(dest));
    }

    private IEnumerator MoveFixed_co(Vector3 dest)
    {
        float lerpFactor = 0.0f;
        Vector2 start = transform.position;

        while ((dest - transform.position).magnitude > 0.05f)
        {
            transform.position = Vector2.Lerp(start, dest, lerpFactor);
            lerpFactor += speed * Time.fixedDeltaTime;
            yield return StaticValues.waitForFixed;
        }
    }
}
