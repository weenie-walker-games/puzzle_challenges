using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace NumberPuzzle
{
    public class CircleObject : MonoBehaviour, IPointerClickHandler
    {

        public static event System.Action<CircleObject, bool> OnCircleValueChanged;

        [SerializeField] private List<Transform> connectionPoints = new List<Transform>();
        [SerializeField] private TMP_Text textBox;
        [SerializeField] int correctValue;
        [SerializeField] bool isProvided = false;

        private bool isGameActive = false;
        private int lineCount;
        private int currentValue = 0;

        private void OnEnable()
        {
            GameManager.OnGameStart += GetNumberLines;
            GameManager.OnGameEnd += GameEnd;
        }

        private void OnDisable()
        {
            GameManager.OnGameStart -= GetNumberLines;
            GameManager.OnGameEnd -= GameEnd;
        }

        private void Start()
        {
            if (isProvided)
                currentValue = correctValue;

            UpdateText(isProvided);

            OnCircleValueChanged?.Invoke(this, currentValue == correctValue);
        }

        private void GetNumberLines(int lineCount)
        {
            this.lineCount = lineCount;
            isGameActive = true;
        }

        private void GameEnd()
        {
            isGameActive = false;
        }

        public Vector3 GetConnectionPointLocation(int direction)
        {
            return connectionPoints[direction].position;
        }

        public Vector3 GetConnectionPointLocation(DirectionEnum direction)
        {
            return connectionPoints[(int)direction].position;
        }

        private void UpdateText(bool toShow = true)
        {
            if (!toShow || currentValue == 0)
                textBox.text = "";
            else
                textBox.text = currentValue.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isGameActive)
                return;

            //Don't allow a click if this number was provided
            if (isProvided)
                return;
            
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                //increase currentValue and then find it mod the total number of lines + 1 (to account for the "" option)
                currentValue = (currentValue + 1) % (lineCount + 1);
                Debug.Log("Line count " + lineCount);
            }

            if(eventData.button == PointerEventData.InputButton.Right)
            {
                currentValue = 0;
            }

            UpdateText();
            OnCircleValueChanged?.Invoke(this, currentValue == correctValue);
        }

    }
}
