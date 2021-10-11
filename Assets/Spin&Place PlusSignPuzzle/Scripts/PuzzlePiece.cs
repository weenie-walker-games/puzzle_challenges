using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class PuzzlePiece : MonoBehaviour
    {

        [SerializeField] private Animator anim;
 

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            
        }

        public void PlayAnimation()
        {
            anim.SetTrigger("Incorrect");
        }
    }
}
