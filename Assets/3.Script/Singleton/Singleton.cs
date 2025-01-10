using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {

#if UNITY_EDITOR
            if (FindObjectsByType(typeof(T), FindObjectsSortMode.None).Length > 1)
                Debug.LogWarning($"씬에 {typeof(T)}가 여러개 존재합니다!");
#endif

            instance = this as T;
            (instance as Singleton<T>).Init();
        }

        else
            Destroy(gameObject);
    }

    protected abstract void Init();
}
