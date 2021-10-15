using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Puzzle
{
    public class ColorPickerEditor : EditorWindow
    {
        public static event System.Action<Color> OnColorSelected;

        static Color color = Color.black;
        static ColorPickerEditor picker;
        static Vector2 windowSize = new Vector2(200, 200);
        
        public ColorPickerEditor()
        {

        }

        public static void Init()
        {

            picker = (ColorPickerEditor)EditorWindow.GetWindow(typeof(ColorPickerEditor), true, "Color Picker");
            picker.minSize = windowSize;
            picker.maxSize = windowSize;
            picker.titleContent.text = "ColorPicker";
            picker.Repaint();
        }

        private void OnGUI()
        {
            
            EditorGUILayout.LabelField("Select Color", EditorStyles.boldLabel, GUILayout.Height(50));

            color = EditorGUILayout.ColorField("Color:", color);

            if (GUILayout.Button("Select Color"))
            {
                OnColorSelected?.Invoke(color);
                Close();
            }
        }
    }
}
