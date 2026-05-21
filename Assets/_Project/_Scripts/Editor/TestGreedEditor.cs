using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestGreed))]
public class TestGreedEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Spawn Cells"))
        {
            foreach (Object selectedTarget in targets)
            {
                TestGreed greed = (TestGreed)selectedTarget;
                greed.SpawnCells();
                EditorUtility.SetDirty(greed);
            }
        }
    }
}
