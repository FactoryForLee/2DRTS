using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    protected override void Init()
    {

    }

    public Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public RaycastHit GetMouseRayInfo()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Debug.DrawRay(Camera.main.ScreenToWorldPoint(mousePos), ray.direction * 100, Color.blue);

        Physics.Raycast(ray, out RaycastHit hit, 100);
        return hit;
    }
}
