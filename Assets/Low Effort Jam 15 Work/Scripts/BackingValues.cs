using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowEffort
{
    [System.Serializable]
    public class BackingValue
    {

        public Vector2 location = Vector2.zero;
        public Color color = Color.green;

        public BackingValue()
        {

        }

        public BackingValue(Vector2 location, Color color)
        {
            this.location = location;
            this.color = color;
        }
    }
}
