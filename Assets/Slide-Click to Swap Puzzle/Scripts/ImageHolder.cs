using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Swap
{
    public class ImageHolder : MonoBehaviour
    {
        public static event System.Action<ImageHolder> OnImageHolderClicked;
        public static event System.Action<ImageHolder, bool> OnImageHolderChecked;

        [SerializeField] private Vector2 puzzleLocation;
        public Vector2 PuzzleLocation { get { return puzzleLocation; } }
        [SerializeField] Image image; 
        public ImageData CurrentSprite { get; private set; }
        [SerializeField] Color highlightedColor;

        private void OnEnable()
        {
            GameManager.OnResetColors += ResetColors;
        }

        private void OnDisable()
        {
            GameManager.OnResetColors -= ResetColors;
        }

        public void ReceiveCurrentSpriteData(ImageData data)
        {
            CurrentSprite = data;
            image.sprite = CurrentSprite.sprite;
            image.color = Color.white;
            CheckResults();

        }

        private void CheckResults()
        {
            bool isCorrect = false;

            //compare the coordinates for this location to what is currently shown
            if(CurrentSprite.imageCoordinates == puzzleLocation)
            {
                isCorrect = true;
            }

            OnImageHolderChecked?.Invoke(this, isCorrect);
        }

        public void ClickedGamePiece()
        {
            image.color = highlightedColor;
            OnImageHolderClicked?.Invoke(this);
        }

        private void ResetColors()
        {
            image.color = Color.white;
        }
    }
}
