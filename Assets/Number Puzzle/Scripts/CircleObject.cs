using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WeenieWalker
{
    public class CircleObject : MonoBehaviour
    {
        [SerializeField] private List<Transform> connectionPoints = new List<Transform>();
        [SerializeField] private TMP_Text textBox;
        [SerializeField] int correctValue;
        [SerializeField] bool isProvided = false;

        private int currentValue;

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            
        }

        private void Start()
        {
            if (isProvided)
                currentValue = correctValue;

            UpdateText(isProvided);
        }

        public Vector3 GetConnectionPointLocation(int direction)
        {
            return connectionPoints[direction].position;
        }

        public Vector3 GetConnectionPointLocation(DirectionEnum direction)
        {
            return connectionPoints[(int)direction].position;
        }

        private void UpdateText(bool toShow = false)
        {
            if (!toShow || currentValue == 0)
                textBox.text = "";
            else
                textBox.text = currentValue.ToString();
        }
    }
}
