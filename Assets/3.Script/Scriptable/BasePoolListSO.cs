using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseListSO", menuName = "Scriptable Objects/BaseListSO")]
public class BasePoolListSO : ScriptableObject
{
    [SerializeField] private List<GameObject> list;
}
