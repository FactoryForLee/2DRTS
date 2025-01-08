using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private T instance;
    public T Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            if (FindObjectsByType(typeof(T), FindObjectsSortMode.None).Length > 1)
                Debug.LogWarning($"씬에 {typeof(T)}가 여러개 존재합니다!");

            instance = this as T;
            (instance as Singleton<T>).Init();
        }

        else
            Destroy(gameObject);
    }

    protected abstract void Init();
}
