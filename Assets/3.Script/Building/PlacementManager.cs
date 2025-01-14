using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private bool isEven;
    [SerializeField] private LayerMask cantBuildLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Color validColor;
    [SerializeField] private Color invalidColor;
    [SerializeField] private InputManager_RnD inputManager;
    [SerializeField] public int prefabKey;

    private bool canBuild;
    private InputAction onLeftMouseClick;
    private InputAction onCancelAction;
    public bool ChangeEvenState(bool isEven) => this.isEven = isEven;
    private BuildingHoloUI holoUI;
    private Vector3 evenOffset = new Vector3(0.5f, 0.5f, 0.0f);
    private Vector3 updatedPos;

    public UnityEvent<int, Vector2, Queue<SpriteRenderer>> OnStartBuild;

    private void Awake()
    {
        holoUI = GetComponent<BuildingHoloUI>();
        onCancelAction = InputSystem.actions.actionMaps[1].FindAction("Cancel");
        onLeftMouseClick = InputSystem.actions.actionMaps[0].FindAction("Attack");
    }

    private void OnEnable()
    {
        onLeftMouseClick.started += OnClickDeploy;
        onCancelAction.started += SetActiveFalse;
    }

    private void Update()
    {
        updatedPos = Vector3Int.RoundToInt(InputManager.Instance.GetMousePos());
        if (isEven) updatedPos -= evenOffset;
        transform.position = updatedPos;
        canBuild = CheckCondition(holoUI.buildHolos);
    }

    private bool CheckCondition(Queue<SpriteRenderer> buildHolos)
    {
        bool isValid = false;

        foreach (SpriteRenderer grid in buildHolos)
        {
            if (MapNodeManager.Instance.IsInMap(grid.transform.position) &&
                !MapNodeManager.Instance.GetNodeByPos(Vector2Int.RoundToInt(grid.transform.position)).BuildingExist &&
                Physics2D.OverlapBox(grid.transform.position, DataManager.Instance.overlapBoxSize, 0.0f, cantBuildLayer)
                == null)//한 칸 == float 1
            {
                isValid = true;
                grid.color = validColor;
            }

            else
            {
                isValid = false;
                grid.color = invalidColor;
            }
        }

        return isValid;
    }

    private void OnClickDeploy(InputAction.CallbackContext context)
    {
        if (!canBuild) return;
        OnStartBuild?.Invoke(prefabKey, transform.position, holoUI.buildHolos);
    }

    private void SetActiveFalse(InputAction.CallbackContext con) => gameObject.SetActive(false);

    private void OnDisable()
    {
        onLeftMouseClick.started -= OnClickDeploy;
        onCancelAction.started -= SetActiveFalse;
    }
}
