using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowEffort
{
    public class PlacementLocation : MonoBehaviour
    {
        public static event System.Action<PlacementLocation, bool> OnLocationHover;

        private bool isMouseIn = false;
        private bool isPiecePlaced = false;

        private void OnEnable()
        {
            PuzzleManager.OnGameReset += GameReset;
        }

        private void OnDisable()
        {
            PuzzleManager.OnGameReset -= GameReset;
        }

        public bool PlacePiece()
        {

            return false;
        }


        private void OnMouseEnter()
        {

            isMouseIn = true;

            //OnLocationHover?.Invoke(this, true);
        }

        private void OnMouseExit()
        {

            isMouseIn = false;

            //OnLocationHover?.Invoke(this, false);
        }

        private void GameReset()
        {
            isMouseIn = false;
            isPiecePlaced = false;
        }
    }
}
