using UnityEngine;
using Define;

public abstract class Building : MonoBehaviour, IPoolObject
{
    [SerializeField] private int prefabKey;
    public int PrefabKey => prefabKey;
}