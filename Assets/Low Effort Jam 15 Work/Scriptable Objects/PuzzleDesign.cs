using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowEffort
{
    [CreateAssetMenu(menuName ="Create Puzzle Design")]
    public class PuzzleDesign : ScriptableObject
    {
        public string CodeValue { get; private set; }
        public List<Color> colors = new List<Color>();
        public BackingValue[,] backingValues = new BackingValue[40,40];
        private Vector2 activeDimensions;
        

        public PuzzleDesign()
        {
        }

        public PuzzleDesign(int height, int width)
        {
            backingValues = new BackingValue[height, width];
        }

        public void SaveData(BackingValue[,] values, List<Color> colorList)
        {
            backingValues = values;
            colors = colorList;
        }

        public void SetCodeValue(string code)
        {
            CodeValue = code;
        }

        public void SetDimensions(int height, int width)
        {
            activeDimensions.x = height;
            activeDimensions.y = width;
        }

    }
}
