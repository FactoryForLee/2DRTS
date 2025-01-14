using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildManager : PoolingManager
{
    [SerializeField] private BuildingHoloUI holoUI;
    private PlacementManager placement;
    [SerializeField] private Image panelUI;
    [SerializeField] private BuildSlotUI slotUIPrefab;
    [SerializeField] private GameObject gridSprite;

    /// <summary>
    /// Panel에 SO에 있는 모든 빌딩 슬롯 생성
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        placement = holoUI.GetComponent<PlacementManager>();
        placement.OnStartBuild.AddListener(StartBuild);

        foreach (PoolingObject item in poolingListSO.List)
        {
            BuildSlotUI ui = Instantiate(slotUIPrefab, panelUI.transform);

            ui.Icon.sprite =
                item.GetComponent<SpriteRenderer>().sprite;
            ui.PrefabKey = item.PrefabKey;//디버깅을 위한 key 삽입

            //각 버튼 데이터에 따른 Holo 활성화 로직
            ui.OnBtnUp.AddListener(() =>
                EnableHoloUI(poolingListSO.List[item.PrefabKey].GetComponent<Building>(), item.PrefabKey)
            );
        }
    }

    private void EnableHoloUI(Building data, int prefabkey)
    {//빌딩 할당 후 홀로 활성화
        placement.prefabKey = prefabkey;
        holoUI.AssignBuidling(data);
        holoUI.gameObject.SetActive(true);
        gridSprite.SetActive(true);
    }

    private void StartBuild(int prefabIndex, Vector2 spawnPos, Queue<SpriteRenderer> nodePoses)
    {
        Building newBuilding = GetObjByPool(prefabIndex).GetComponent<Building>();
        newBuilding.transform.position = spawnPos;

        foreach (SpriteRenderer node in nodePoses)
        {
            MapNode bindNode = MapNodeManager.Instance.GetNodeByPos(Vector2Int.RoundToInt(node.transform.position));
            newBuilding.bindedNodes.Add(bindNode);
            bindNode.AssignBuilding(newBuilding);
        }
    }
}
