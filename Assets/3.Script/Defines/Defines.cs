using UnityEngine;
using UnityEngine.Events;
using System;

namespace Define
{
    public interface IHurtable
    {
        public float CurHP
        {
            get;
        }

        void ReduceHP(float damage);
        void RefillHP();
    }

    public interface IOwnKey<T>
    {
        public T PrefabKey
        {
            get;
            set;
        }
    }

    public interface IPoolObject<T> : IOwnKey<int>
    {
        public UnityAction<T> OnDisableAction
        {
            get;
            set;
        }
    }

    public static class StaticValues
    {
        public static readonly WaitForFixedUpdate waitForFixed = new WaitForFixedUpdate();
    }

    [Serializable]
    public class ArrayHolder<T>
    {
        public ArrayHolder(T[] arr)
        {
            this.arr = arr;
        }

        [SerializeField] private T[] arr;
        public T[] Arr => arr;
    }

    /*
     * Input은 해당 map을 가져오면 된다.
     * InputSystem.actions.actionMaps[0] => Player Action
     * InputSystem.actions.actionMaps[1] => Input Action
     */
}
