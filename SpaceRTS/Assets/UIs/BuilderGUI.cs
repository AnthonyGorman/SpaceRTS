using UnityEngine;
using UnityEditor;

using GameLogic;

[CustomEditor(typeof(BuilderBehaviour))]
public class BuilderGUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Separator();

        var behaviour = (BuilderBehaviour)target;

        EditorGUILayout.LabelField("Current action: ", BuilderLogic.displayCurrentAction(behaviour.currentAction));
    }
}
