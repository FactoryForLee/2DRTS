using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(PlacementManager))]
public class BuildingHoloUI : MonoBehaviour
{
    [SerializeField] private Building curBuilding;
    [SerializeField] private Vector2Int prefabSize;
    [SerializeField] private Sprite frame;
    [SerializeField] public Queue<SpriteRenderer> buildHolos { get; private set; } = new Queue<SpriteRenderer>();
    [SerializeField] private LayerMask cantBuildLayer;
    //[SerializeField] public bool canBuild;//건물 건설 가능 여부 확인을 위한 bool
    [SerializeField] private SpriteRenderer icon;

    private PlacementManager placement;

    public void AssignBuidling(Building newBuilding)
    {
        curBuilding = newBuilding;
        icon.sprite = curBuilding.GetComponentInChildren<SpriteRenderer>().sprite;
    }

    private void Awake()
    {
        placement = GetComponent<PlacementManager>();
    }

    private void OnEnable()
    {
        prefabSize = curBuilding.Size;
        int x = prefabSize.x;
        int y = prefabSize.y;
        int frameAmount = x * y;

        for (int i = buildHolos.Count; i < frameAmount; i++)
        {
            SpriteRenderer newFrame = new GameObject("Frame").AddComponent<SpriteRenderer>();
            newFrame.sortingOrder = 4;
            buildHolos.Enqueue(newFrame);
            newFrame.sprite = frame;
            newFrame.transform.SetParent(transform);
            newFrame.gameObject.SetActive(false);
        }

        Vector2 startPos = new Vector3(-(prefabSize.x * 0.5f) + 0.5f,0.5f,0.0f);
        Debug.Log("시작 위치 : " + startPos);
        placement.ChangeEvenState(x%2 == 0);

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
    }

    private void OnDisable()
    {
        foreach (SpriteRenderer item in buildHolos)
            item.gameObject.SetActive(false);
    }
}
