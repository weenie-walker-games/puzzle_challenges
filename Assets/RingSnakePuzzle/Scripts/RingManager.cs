using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class RingManager : MonoSingleton<RingManager>
    {
        [SerializeField] List<GameObject> rings = new List<GameObject>();
        int currentRing = 0;
        Dictionary<RingTester, bool> ringResults = new Dictionary<RingTester, bool>();
        [SerializeField] float spinSpeed = 5;
        [SerializeField] GameObject uiObject;

        bool gameWon = false;

        private void OnEnable()
        {
            RingTester.OnEnableRingTester += AddRingTester;
            uiObject.SetActive(false);
        }

        private void OnDisable()
        {
            RingTester.OnEnableRingTester -= AddRingTester;
        }

        private void AddRingTester(RingTester tester, bool isMatching = false)
        {
            Debug.Log($"RingTester {tester.name} has result {isMatching}");

            if (ringResults.ContainsKey(tester))
                ringResults[tester] = isMatching;
            else
                ringResults.Add(tester, isMatching);
        }

        private void Update()
        {
            if (gameWon)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                //pick next ring to spin
                currentRing = (currentRing + 1) % rings.Count;
            }

            float input = Input.GetAxisRaw("Horizontal");

            if (input > 0.01f)
            {
                rings[currentRing].transform.Rotate(Vector3.forward, 15f * input * Time.deltaTime * spinSpeed);
            }

            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                bool winGame = CheckResult();

                if (winGame)
                {
                    gameWon = true;
                    Debug.Log("Game Won!");
                    uiObject.SetActive(true);
                }
            }
        }

        private bool CheckResult()
        {
            bool matchCondition = true;

            foreach(KeyValuePair<RingTester, bool> state in ringResults)
            {
                if (state.Value == false)
                    matchCondition = false;
            }

            return matchCondition;
        }
    }
}
