using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class RingTester : MonoBehaviour
    {

        public static event System.Action<RingTester, bool> OnEnableRingTester;

        bool isMatching = false;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void Start()
        {
            OnEnableRingTester?.Invoke(this, false);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Collision");
                isMatching = true;
                OnEnableRingTester?.Invoke(this, isMatching);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isMatching = false;
                OnEnableRingTester?.Invoke(this, isMatching);
            }
        }
    }
}
