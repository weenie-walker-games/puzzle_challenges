using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puzzle;
using UnityEngine.SceneManagement;

namespace NumberPuzzle
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public static event System.Action OnGameEnd;
        public static event System.Action<int> OnGameStart;


        [SerializeField] private int numberOfLines = 3;
        [SerializeField] GameObject winConditionItems;


        private void OnEnable()
        {
            PuzzleManager.OnGameWon += GameWon;
        }

        private void OnDisable()
        {
            PuzzleManager.OnGameWon -= GameWon;
        }

        private void Start()
        {
            winConditionItems.SetActive(false);
            OnGameStart?.Invoke(numberOfLines);
        }

        private void GameWon()
        {
            OnGameEnd?.Invoke();
            winConditionItems.SetActive(true);
        }

        public void ResetGame(int sceneNumber)
        {
            SceneManager.LoadScene(sceneNumber);
        }
    }
}
