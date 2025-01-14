using UnityEngine;
using System;

[Serializable]
public class MapNode : MonoBehaviour
{
    public int Cost { get; private set; } = 0;

    public Building Building { get; private set; }

    public bool BuildingExist { get; private set; } = false;

    public void AssignBuilding(Building building)
    {
        this.Building = building;
        BuildingExist = true;
        Cost = MapNodeManager.Instance.Cost_Diction[1 << building.gameObject.layer];
    }

    public void RemoveBuilding()
    {
        BuildingExist = false;
        Building = null;
        Cost = 0;
    }

    //각 노드에 변화가 있다면 어떻게 노드에게 이를 알려줄까?
    //건물을 지을 때, 랜덤 맵이 생성될 때, 건물이 파괴될 때
    //상위 3개의 이벤트에 이걸 등록해야겠다


    //TODO: 디버깅용 필히 삭제!!!
    private static Color blue = new Color(0, 0.2f, 0.2f, 0.8f);
    private void OnDrawGizmos()
    {
        Gizmos.color = blue;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}