using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;

public class BuildingHoloUI : MonoBehaviour
{
    [SerializeField] private Building curBuilding;
    [SerializeField] private Vector2 prefabSize;
    [SerializeField] private Sprite frame;
    [SerializeField] private Queue<SpriteRenderer> buildHolos = new Queue<SpriteRenderer>();
    [SerializeField] private LayerMask cantBuildLayer;
    [SerializeField] private bool canBuild;//건물 건설 가능 여부 확인을 위한 bool

    [SerializeField] private Color vaildColor;
    [SerializeField] private Color invalidColor;
    [SerializeField] private SpriteRenderer icon;
    private InputAction onLeftMouseClick;
    private InputAction onCancelAction;


    public void AssignBuidling(Building newBuilding)
    {
        curBuilding = newBuilding;
        icon.sprite = curBuilding.GetComponentInChildren<SpriteRenderer>().sprite;
    }

    private void Awake()
    {
        onCancelAction = InputSystem.actions.actionMaps[1].FindAction("Cancel");
        onLeftMouseClick = InputSystem.actions.actionMaps[0].FindAction("Attack");
    }

    private void OnEnable()
    {
        prefabSize = curBuilding.Size;
        int x = Mathf.CeilToInt(prefabSize.x);
        int y = Mathf.CeilToInt(prefabSize.y);
        int frameAmount = x * y;
        Debug.Log("총 프레임 : " + frameAmount);
        Debug.Log("x 값 : " + x);
        Debug.Log("y 값 : " + y);

        for (int i = buildHolos.Count; i < frameAmount; i++)
        {
            Debug.Log("몇 번 생성 되니?" + i);
            SpriteRenderer newFrame = new GameObject("Frame").AddComponent<SpriteRenderer>();
            newFrame.sortingOrder = 4;
            buildHolos.Enqueue(newFrame);
            newFrame.sprite = frame;
            newFrame.transform.SetParent(transform);
            newFrame.gameObject.SetActive(false);
        }
        

        Vector2 startPos = new Vector3(-(prefabSize.x * 0.5f),0.5f,0.0f);

        for (int i = 0; i < y; i++)
        {
            Vector2 curPos = startPos;
            curPos.y += i;

            for (int j = 0; j < x; j++)
            {
                SpriteRenderer frame = buildHolos.Dequeue();
                frame.transform.localPosition = curPos;
                frame.gameObject.SetActive(true);
                buildHolos.Enqueue(frame);
                curPos.x += 1;
            }
        }


        onLeftMouseClick.started += OnClickDeploy;
        onCancelAction.started += SetActiveFalse;
    }

    public void Update()
    {
        canBuild = CheckCondition(transform.position, prefabSize, out Collider2D[] innerobjs);
        ChangeHoloColor(innerobjs);
        SetPosByMouse();
    }

    private void SetPosByMouse()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        newPos.z = 0.0f;
        transform.position = newPos;
    }

    private void ChangeHoloColor(Collider2D[] innerobjs)
    {
        Vector2Int gridPos;
        //사이즈에서 start에서 빼버려서 index로 쓰면 바로 가능?

        foreach (SpriteRenderer grid in buildHolos)
        {
            gridPos = Vector2Int.RoundToInt(grid.transform.position);

            if (innerobjs != null &&
                innerobjs.Select((x) => gridPos.Equals(Vector2Int.RoundToInt(x.transform.position))) != null)
                grid.color = invalidColor;

            else
                grid.color = vaildColor;
        }
    }

    private bool CheckCondition(Vector2 pos, Vector2 buildingSize, out Collider2D[] innerObjs)
    {
        innerObjs = Physics2D.OverlapBoxAll(pos, buildingSize, 0.0f, cantBuildLayer);
        return innerObjs.Length == 0;
    }

    private void OnClickDeploy(InputAction.CallbackContext context)
    {
        Debug.Log("한 번 불림");

        if (canBuild)
            Debug.Log("건물 짓는 로직 추가 필요");
    }

    private void SetActiveFalse(InputAction.CallbackContext con) => gameObject.SetActive(false);

    private void OnDisable()
    {
        onLeftMouseClick.started -= OnClickDeploy;
        onCancelAction.started -= SetActiveFalse;

        foreach (SpriteRenderer item in buildHolos)
            item.gameObject.SetActive(false);
    }
}
