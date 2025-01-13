using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildManager : PoolingManager
{
    [SerializeField] private BuildingHoloUI holoUI;
    [SerializeField] private Image panelUI;
    [SerializeField] private BuildSlotUI slotUIPrefab;
    [SerializeField] private GameObject gridSprite;

    /// <summary>
    /// Panel에 SO에 있는 모든 빌딩 슬롯 생성
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        foreach (PoolingObject item in poolingListSO.List)
        {
            BuildSlotUI ui = Instantiate(slotUIPrefab, panelUI.transform);

            ui.Icon.sprite =
                item.GetComponent<SpriteRenderer>().sprite;
            ui.PrefabKey = item.PrefabKey;//디버깅을 위한 key 삽입

            //각 버튼 데이터에 따른 Holo 활성화 로직
            ui.OnBtnUp.AddListener(() =>
            {
                EnableHoloUI(item.PrefabKey);
                gridSprite.SetActive(true);
            });
        }
    }

    private void EnableHoloUI(int prefabKey)
    {//빌딩 할당 후 홀로 활성화
        Debug.Log("할당된 key = " + prefabKey);
        holoUI.AssignBuidling(poolingListSO.List[prefabKey].GetComponent<Building>());
        holoUI.gameObject.SetActive(true);
    }
}
