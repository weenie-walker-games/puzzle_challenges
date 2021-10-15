using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swap
{
    [CreateAssetMenu(menuName ="Create Image Data File")]
    public class ImageDataSO : ScriptableObject
    {
        public List<ImageData> imageDataItems = new List<ImageData>();

    }



}
