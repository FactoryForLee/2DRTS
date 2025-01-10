using UnityEngine;
using UnityEngine.Events;

public class PoolingObject : MonoBehaviour, Define.IPoolObject<PoolingObject>
{
    [SerializeField] private int prefabKey;

    public int PrefabKey
    { 
        get => prefabKey; 
        set => prefabKey = value;
    }

    private UnityAction<PoolingObject> onDisableAction;
    public UnityAction<PoolingObject> OnDisableAction
    { 
        get => onDisableAction;
        set => onDisableAction = value;
    }

    private void OnDisable()
    {
        OnDisableAction?.Invoke(this);
    }
}
