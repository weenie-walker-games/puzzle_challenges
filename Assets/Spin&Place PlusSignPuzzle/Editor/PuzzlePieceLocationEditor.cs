using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Puzzle
{
    [CustomEditor(typeof(PuzzlePieceLocation))]
    public class PuzzlePieceLocationEditor : Editor
    {


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PuzzlePieceLocation locationScript = (PuzzlePieceLocation)target;
            //locationScript.LocationOrientation = (PieceOrientation)EditorGUILayout.EnumPopup("Orientation for Piece", locationScript.LocationOrientation);

            if(GUILayout.Button("Update Active State"))
            {
                locationScript.UpdateTarget(locationScript.LocationOrientation);
            }
        }

    }
}
