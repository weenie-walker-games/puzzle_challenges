using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ImageRotate
{
    public class GameControl : MonoBehaviour
    {
        /// <summary>
        /// Based on the YouTube tutorial by Alexander Zotov (https://www.youtube.com/watch?v=TMQrO3Hy_LE)
        /// Adjusted by me to use more advanced techniques and prevent the need for dragging each picture into the Inspector
        /// </summary>
        [SerializeField] private GameObject pictureParent;
        private List<TouchRotate> pictures = new List<TouchRotate>();

        [SerializeField] private GameObject winText;

        public static bool youWin;

        private void Start()
        {
            winText.SetActive(false);
            youWin = false;

            pictures = pictureParent.GetComponentsInChildren<TouchRotate>().ToList<TouchRotate>();
        }

        private void Update()
        {
            List<TouchRotate> results = pictures.Where(t => t.gameObject.transform.rotation.z != 0).Select(t => t).ToList<TouchRotate>();
            if (results.Count == 0)
                youWin = true;

            if (youWin == true)
                winText.SetActive(true);
        }
    }
}
