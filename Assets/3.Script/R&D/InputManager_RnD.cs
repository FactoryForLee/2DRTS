using UnityEngine;

public class InputManager_RnD : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask placementLayerMask;
    [SerializeField] private Vector3 lastPostion;

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);

        Debug.DrawRay(cam.ScreenToWorldPoint(mousePos), ray.direction * 100, Color.blue);

        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            lastPostion = hit.point;
        }       

        return lastPostion;
    }

}
