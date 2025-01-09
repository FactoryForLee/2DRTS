using UnityEngine;
using UnityEngine.Events;
using Define;

public class StatControl : MonoBehaviour, IHurtable
{
    
    //public UnityEvent<float, float> 
    //TODO:[이준형] 모든 객체의 default Stat을 들고있는 싱글톤 필요
    [SerializeField] private float curHP;
    public float CurHP => curHP;

    private void OnEnable()
    {
        RefillHP();
    }

    public virtual void ReduceHP(float damage)
    {

    }

    public virtual void RefillHP()
    {

    }
}
