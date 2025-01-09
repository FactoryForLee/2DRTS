using UnityEngine;
using Define;

public class MapNode : MonoBehaviour
{
    public int Cost
    {
        get
        {
            if (building != null &&
                MapNodeManager.Instance.
                Cost_Diction.TryGetValue(1 << building.layer, out int value))
                return value;

            return 0;
        }
    }
    
    public GameObject building;

    //각 노드에 변화가 있다면 어떻게 노드에게 이를 알려줄까?
    //건물을 지을 때, 랜덤 맵이 생성될 때, 건물이 파괴될 때
    //상위 3개의 이벤트에 이걸 등록해야겠다
}