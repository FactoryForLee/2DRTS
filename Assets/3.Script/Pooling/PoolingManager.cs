using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Define;

public class PoolingManager : MonoBehaviour
{
    [SerializeField] private BasePoolListSO poolingListSO;
    private Queue<PoolingObject>[] buildingPool;

    private void Awake()
    {
        buildingPool = new Queue<PoolingObject>[poolingListSO.List.Count];

        for (int i = 0; i < buildingPool.Length; i++)
            buildingPool[i] = new Queue<PoolingObject>();
    }

    public void PoolingObj(int index, Vector2 pos)
    {
        if (!buildingPool[index].TryDequeue(out PoolingObject newBuilding))
        {
            newBuilding = Instantiate(poolingListSO.List[index], transform);
            newBuilding.OnDisableAction += x => buildingPool[x.PrefabKey].Enqueue(x);
        }

        newBuilding.transform.position = pos;
        newBuilding.gameObject.SetActive(true);
        buildingPool[index].Enqueue(newBuilding);
    }
}