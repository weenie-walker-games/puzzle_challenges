using Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowEffort
{
    public class PuzzleManager : MonoSingleton<PuzzleManager>
    {
        public static event System.Action OnGameReset;

        
        private GameObject ghostPiece;
        private int currentOrientation = 0;
        [SerializeField] private PlacementLocation locationHover = null;
        [SerializeField] private PuzzlePiece pieceHover = null;

        private void OnEnable()
        {
            PlacementLocation.OnLocationHover += GetLocationUnderMouse;
        }

        private void OnDisable()
        {
            PlacementLocation.OnLocationHover -= GetLocationUnderMouse;
        }

        private void Start()
        {
            //Re-arrange pieces
            Invoke("ResetGame", 3f);
        }

        public void ResetGame()
        {
            OnGameReset?.Invoke();
        }

        private void Update()
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = -1;
            //ghostPiece.transform.position = worldPosition;

            //Right click to rotate
            if (Input.GetMouseButtonDown(1))
            {
                currentOrientation = (currentOrientation + 1) % 4;
                float rotation = ReturnRotation((PieceOrientation)currentOrientation);
                //ghostPiece.transform.localEulerAngles = new Vector3(0, 0, rotation);
            }

            //Left click to try and place
            if (Input.GetMouseButtonDown(0))
                TryPlacement();
        }

        private float ReturnRotation(PieceOrientation direction)
        {
            switch (direction)
            {
                case PieceOrientation.Down:
                    return 0;
                case PieceOrientation.Right:
                    return 90;
                case PieceOrientation.Up:
                    return 180;
                case PieceOrientation.Left:
                    return 270;
                default:
                    return 0;
            }
        }

        private void TryPlacement()
        {
            //ignore click outside the playing field
            if (locationHover == null)
                return;

            bool didPlace = locationHover.PlacePiece();

            if (didPlace)
            {
                //ghostPiece.transform.rotation = Quaternion.Euler(Vector3.zero);
                currentOrientation = 0;
            }
            else
            {
                //PuzzlePiece piece = ghostPiece.GetComponentInChildren<PuzzlePiece>();
                //if(piece != null)
                //    piece.PlayAnimation();
            }
        }

        private void GetLocationUnderMouse(PlacementLocation location, bool isUnder)
        {
            if(locationHover == location && !isUnder)
            {
                locationHover = null;
            }

            if (isUnder)
            {
                locationHover = location;
            }
        }
    }
}
