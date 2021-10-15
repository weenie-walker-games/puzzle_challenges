using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Paint
{
    public class ImageButtonScript : MonoBehaviour
    {
        [SerializeField] Image image;
        Sprite sprite;
        Sprite currentSpriteBeingChecked = null;

        private void OnEnable()
        {
            GameManager.OnImageReleased += OnReceiveSpriteToCheck;

            if (image != null)
                sprite = image.sprite;
        }

        private void OnDisable()
        {
            GameManager.OnImageReleased -= OnReceiveSpriteToCheck;
        }



        private void OnReceiveSpriteToCheck(Sprite currentSprite)
        {
            currentSpriteBeingChecked = currentSprite;
        }

        public void OnTap()
        {
            if(sprite != null)
            {
                if(currentSpriteBeingChecked == sprite)
                {
                    //play correct sound
                    image.enabled = false;
                    GameManager.Instance.ImageCorrect();
                }
                else
                {
                    //play incorrect sound
                    //shake image
                }
            }
        }
    }
}
