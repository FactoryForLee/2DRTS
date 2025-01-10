using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CreateAssetMenu(fileName = "BaseListSO", menuName = "Scriptable Objects/BaseListSO")]
public class BasePoolListSO : ScriptableObject
{
    [SerializeField] private List<PoolingObject> list;
    public List<PoolingObject> List => list;
}

#if UNITY_EDITOR
[CustomEditor(typeof(BasePoolListSO))]
public class AssignKey : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BasePoolListSO prefabs = (BasePoolListSO)target;


        if (GUILayout.Button("Assign Key"))
        {
            for (int i = 0; i < prefabs.List.Count; i++)
            {
                prefabs.List[i].PrefabKey = i;
                EditorUtility.SetDirty(prefabs.List[i]);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
#endif