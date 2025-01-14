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

    public PoolingObject GetObjByPool(int index)
    {
        if (!poolQueue_Arr[index].TryDequeue(out PoolingObject newObject))
        {
            newObject = Instantiate(poolingListSO.List[index], transform);
            newObject.OnDisableAction += x => poolQueue_Arr[x.PrefabKey].Enqueue(x);
        }

        newObject.gameObject.SetActive(true);
        poolQueue_Arr[index].Enqueue(newObject);
        return newObject;
    }
}