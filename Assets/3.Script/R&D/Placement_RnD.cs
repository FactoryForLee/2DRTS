using UnityEngine;

public class Placement_RnD : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private InputManager_RnD inputManager;
    [SerializeField] private Grid grid;

    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3 gridPos = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = gridPos;
    }
}
