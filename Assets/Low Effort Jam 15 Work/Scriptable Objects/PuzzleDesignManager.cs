using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowEffort
{
    [CreateAssetMenu(fileName ="Assets/Low Effort Jam 15 Work/PuzzleDesignManager1.asset")]
    public class PuzzleDesignManager : ScriptableObject
    {
        public List<PuzzleDesign> puzzleDesigns = new List<PuzzleDesign>();

    }
}
