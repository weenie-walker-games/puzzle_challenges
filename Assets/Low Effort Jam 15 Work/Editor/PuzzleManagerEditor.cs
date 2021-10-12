using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LowEffort
{
    [CustomEditor(typeof(PuzzleManager))]
    public class PuzzleManagerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PuzzleManager manager = (PuzzleManager)target;

            if(GUILayout.Button("Reset Game"))
            {
                manager.ResetGame();
            }
        }
    }
}
