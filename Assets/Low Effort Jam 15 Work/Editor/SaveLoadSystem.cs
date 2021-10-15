using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace LowEffort
{
    public class SaveLoadSystem
    {

        public void SaveFile(PuzzleDesignManager fileToSave, string path)
        {
            //AssetDatabase.CreateAsset(fileToSave, path);
            AssetDatabase.SaveAssets();
        }

        //public t LoadFile()
        //{

        //}
    }
}
