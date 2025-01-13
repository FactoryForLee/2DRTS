using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Define;

public class PoolingManager : MonoBehaviour
{
    [SerializeField] protected BasePoolListSO poolingListSO;
    public BasePoolListSO PoolingListSO => poolingListSO;
    private Queue<PoolingObject>[] poolQueue_Arr;

    protected virtual void Awake()
    {
        poolQueue_Arr = new Queue<PoolingObject>[poolingListSO.List.Count];

        for (int i = 0; i < poolQueue_Arr.Length; i++)
            poolQueue_Arr[i] = new Queue<PoolingObject>();
    }

    public void GetObjByPool(int index, Vector2 pos)
    {
        if (!poolQueue_Arr[index].TryDequeue(out PoolingObject newBuilding))
        {
            newBuilding = Instantiate(poolingListSO.List[index], transform);
            newBuilding.OnDisableAction += x => poolQueue_Arr[x.PrefabKey].Enqueue(x);
        }

        newBuilding.transform.position = pos;
        newBuilding.gameObject.SetActive(true);
        poolQueue_Arr[index].Enqueue(newBuilding);
    }
}