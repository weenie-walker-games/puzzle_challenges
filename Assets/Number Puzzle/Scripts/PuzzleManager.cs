using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puzzle;

namespace NumberPuzzle
{
    public class PuzzleManager : MonoSingleton<PuzzleManager>
    {
        public static event System.Action OnGameWon;

        private Dictionary<CircleObject, bool> currentResults = new Dictionary<CircleObject, bool>();
        private int lineCount;


        private void OnEnable()
        {
            CircleObject.OnCircleValueChanged += ObtainResults;
            GameManager.OnGameStart += GetLineCount;
        }

        private void OnDisable()
        {
            CircleObject.OnCircleValueChanged -= ObtainResults;
            GameManager.OnGameStart -= GetLineCount;
        }

        private void GetLineCount(int lineCount)
        {
            this.lineCount = lineCount;
        }

        private void ObtainResults(CircleObject circle, bool result)
        {
            if (currentResults.ContainsKey(circle))
                currentResults[circle] = result;
            else
                currentResults.Add(circle, result);

            //Only check if dictionary has all entries
            if (currentResults.Count != Mathf.Pow(lineCount, 2))
                return;

            bool gameWon = CheckResult();

            if (gameWon)
                OnGameWon?.Invoke();

        } 

        private bool CheckResult()
        {
            
            //Check if all are correct
            foreach (KeyValuePair<CircleObject, bool> item in currentResults)
            {
                if (item.Value == false)
                {
                    return false;
                }
            }

            return true;
        }


    }
}
