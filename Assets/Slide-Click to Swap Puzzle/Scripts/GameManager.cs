using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swap
{
    public class GameManager : MonoBehaviour
    {
        public static event System.Action OnResetColors;

        [SerializeField] List<ImageDataSO> spritesData = new List<ImageDataSO>();
        [SerializeField] List<ImageHolder> imageHolders = new List<ImageHolder>();
        [SerializeField] int selectedPuzzle = 0;
        [SerializeField] GameObject winConditionHolder;
        List<ImageData> spriteRandomizer = new List<ImageData>();
        public ImageHolder currentlyClicked;
        Dictionary<ImageHolder, bool> holderSolutions = new Dictionary<ImageHolder, bool>();

        private void OnEnable()
        {
            ImageHolder.OnImageHolderClicked += ClickedGamePiece;
            ImageHolder.OnImageHolderChecked += UpdateResults;
        }

        private void OnDisable()
        {
            ImageHolder.OnImageHolderClicked -= ClickedGamePiece;
            ImageHolder.OnImageHolderChecked -= UpdateResults;
        }

        private void Start()
        {
            winConditionHolder.SetActive(false);

            //Create a list of all possible sprites from within the SO
            ResetSprites();

            //Randomly assign the sprites
            AssignSprites();
        }

        private void ResetSprites()
        {
            for (int i = 0; i < spritesData[selectedPuzzle].imageDataItems.Count; i++)
            {
                spriteRandomizer.Add(spritesData[selectedPuzzle].imageDataItems[i]);
            }

            holderSolutions.Clear();

            //Create a dictionary to store the results
            for (int i = 0; i < imageHolders.Count; i++)
            {
                holderSolutions.Add(imageHolders[i], false);
            }
        }

        private void AssignSprites()
        {
            for (int i = 0; i < imageHolders.Count; i++)
            {
                int rand = Random.Range(0, spriteRandomizer.Count);
                ImageData temp = spriteRandomizer[rand];
                imageHolders[i].ReceiveCurrentSpriteData(temp);
                spriteRandomizer.Remove(temp);
            }
        }

        private void ClickedGamePiece(ImageHolder clickedHolder)
        {
            if(currentlyClicked == null)
            {
                currentlyClicked = clickedHolder;
            }
            else
            {
                //Check to see if these are neighbors
                Vector2 currentClickLocation = currentlyClicked.PuzzleLocation;
                Vector2 newLocation = clickedHolder.PuzzleLocation;
                Vector2 results;
                results.x = Mathf.Abs(currentClickLocation.x - newLocation.x);
                results.y = Mathf.Abs(currentClickLocation.y - newLocation.y);
                int distance = (int)results.x + (int)results.y;

                if (distance > 1)
                {
                    currentlyClicked = null;
                    OnResetColors?.Invoke();
                    return; //not close and shouldn't swap
                }

                ImageData temp = clickedHolder.CurrentSprite;
                clickedHolder.ReceiveCurrentSpriteData(currentlyClicked.CurrentSprite);
                currentlyClicked.ReceiveCurrentSpriteData(temp);
                currentlyClicked = null;
                OnResetColors?.Invoke();
            }
        }

        private void UpdateResults(ImageHolder holder, bool isCorrect)
        {
            holderSolutions[holder] = isCorrect;

            //check all solutions for a win condition
            bool isWon = true;

            foreach (KeyValuePair<ImageHolder, bool> item in holderSolutions)
            {
                if(item.Value == false)
                {
                    isWon = false;
                    break;
                }
            }

            if (isWon)
                WinCondition();
        }

        private void WinCondition()
        {
            winConditionHolder.SetActive(true);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}
