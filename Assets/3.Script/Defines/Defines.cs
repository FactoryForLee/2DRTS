using UnityEngine;

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

    public interface IPoolObject
    {
        public int PrefabKey { get; }
    }
}
