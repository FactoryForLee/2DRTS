using UnityEngine;

[CreateAssetMenu(fileName = "BaseStatusSO", menuName = "Scriptable Objects/BaseStatusSO")]
public class BaseStatusSO : ScriptableObject
{
    [SerializeField] private float maxHP;
    [SerializeField] private float speed;

    public float Speed => speed;
    public float MaxHP => maxHP;
}