using Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace LowEffort
{
    public class PuzzleManager : MonoSingleton<PuzzleManager>
    {
        public static event System.Action OnGameReset;
        public static event System.Action<bool> OnHoverButtonClick;

        [SerializeField] private GameObject resetButton;
        [SerializeField] private GameObject winText;
        [SerializeField] private GameObject puzzlePieceParent;
        private PlacementLocation locationHover = null;
        private PuzzlePiece pieceHover = null;
        private bool isGameRunning = true;
        public static bool puzzleNotSolved = true;
        [SerializeField] private List<PuzzlePiece> pieces = new List<PuzzlePiece>();

        private void OnEnable()
        {
            PlacementLocation.OnLocationHover += GetLocationUnderMouse;
            PuzzlePiece.OnPieceHover += GetPieceUnderMouse;
        }

        private void OnDisable()
        {
            PlacementLocation.OnLocationHover -= GetLocationUnderMouse;
            PuzzlePiece.OnPieceHover -= GetPieceUnderMouse;
        }

        private void Start()
        {
            //create the pieces list
            pieces = puzzlePieceParent.GetComponentsInChildren<PuzzlePiece>().ToList<PuzzlePiece>();
            

            winText.SetActive(false);
            resetButton.SetActive(false);
            //Re-arrange pieces
            Invoke("ResetGame", 3f);
        }

        public void ResetGame()
        {
            isGameRunning = true;
            OnGameReset?.Invoke();
        }

        private void Update()
        {
            if (!isGameRunning)
                return;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.x = Mathf.Clamp(worldPosition.x, -10f, 23.5f);
            worldPosition.y = Mathf.Clamp(worldPosition.y, -8.5f, 16f);
            worldPosition.z = -2;
            if(pieceHover != null)
                pieceHover.transform.position = worldPosition;

            //Right click to rotate
            if (Input.GetMouseButtonDown(1))
            {
                if (pieceHover != null)
                {
                    pieceHover.Rotate();
                }
            }

            //Left click to pick up a piece and try to place
            if (Input.GetMouseButtonDown(0))
            {
                //check if already holding a piece
                if(pieceHover != null)
                {
                    if (locationHover != null)
                    {
                        TryPlacement();
                    }
                    else
                    {

                        pieceHover.ReleasePiece();
                        pieceHover = null;
                    }

                }
                else
                {
                    //See if able to get a piece
                    OnHoverButtonClick?.Invoke(true);
                }



            }
             
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

            //Check with the piece to see if it is overlapping
            //This should also account for a piece already placed
            if (!pieceHover.IsAvailableForPlacement)
                return;

            Vector3 newPos = locationHover.transform.position;
            newPos.x = Mathf.RoundToInt(newPos.x*2)/2f;
            newPos.y = Mathf.RoundToInt(newPos.y*2)/2f;
            newPos.z = -2;
            pieceHover.transform.position = newPos;
            //piecesRemaining--;
            pieceHover.Placed();
            pieceHover = null;

            //check the results and go to win game if true
            bool winCondition = true;

            for (int i = 0; i < pieces.Count; i++)
            {
                if(pieces[i].AllCorrect == false)
                {
                    winCondition = false;
                    break;
                }
            }

            if (winCondition)
            {
                    winText.SetActive(true);
                    resetButton.SetActive(true);
                    isGameRunning = false;
                
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

        public void GetPieceUnderMouse(PuzzlePiece piece, bool isUnder)
        {
            pieceHover = piece;

        }
    }
}
