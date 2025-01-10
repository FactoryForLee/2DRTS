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

    public interface IPoolObject<T>
    {
        public int PrefabKey 
        { 
            get;
            set;
        }

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
}
