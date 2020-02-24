using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public static class InspectorCollapseAllComponents
{
    [MenuItem("CONTEXT/Component/Collapse All")]
    private static void CollapseAllInGameObject(MenuCommand command)
    {
        SetAllInspectorsExpanded((command.context as Component).gameObject, false);
    }

    [MenuItem("CONTEXT/Component/Expand All")]
    private static void ExpandAllInGameObject(MenuCommand command)
    {
        SetAllInspectorsExpanded((command.context as Component).gameObject, true);
    }

    [MenuItem("Component/Components/Collapse All")]
    private static void CollapseAll()
    {
        foreach (var gameObject in Object.FindObjectsOfType<GameObject>())
            SetAllInspectorsExpanded(gameObject, false);
    }

    [MenuItem("Component/Components/Expand All")]
    private static void ExpandAll()
    {
        foreach (var gameObject in Object.FindObjectsOfType<GameObject>())
            SetAllInspectorsExpanded(gameObject, true);
    }

    public static void SetAllInspectorsExpanded(GameObject go, bool expanded)
    {
        Component[] components = go.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component is Transform)
                continue;
            else if (component is Renderer)
            {
                var mats = ((Renderer)component).sharedMaterials;
                for (int i = 0; i < mats.Length; ++i)
                {
                    InternalEditorUtility.SetIsInspectorExpanded(mats[i], expanded);
                }
            }
            InternalEditorUtility.SetIsInspectorExpanded(component, expanded);
        }
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }
}