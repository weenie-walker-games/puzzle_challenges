using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puzzle;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Paint
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public static event System.Action<Sprite> OnImageReleased;

        [SerializeField] Image imageToCheck;
        [SerializeField] List<Sprite> images;
        [SerializeField] GameObject winConditionItems;


        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void Start()
        {
            winConditionItems.SetActive(false);
            NextImage();
        }

        public void ImageCorrect()
        {
            //spit out next image
            NextImage();
        }

        private void NextImage()
        {
            if (images.Count != 0)
            {
                int randomImage = Random.Range(0, images.Count);
                Sprite nextSprite = images[randomImage];
                images.Remove(nextSprite);
                imageToCheck.sprite = nextSprite;
                OnImageReleased?.Invoke(nextSprite);


            }
            else
            {
                WinCondition();
            }
        }

        private void WinCondition()
        {
            imageToCheck.gameObject.SetActive(false);
            winConditionItems.SetActive(true);
        }

        public void ResetGame()
        {
            SceneManager.LoadScene(0);
        }

    }
}
