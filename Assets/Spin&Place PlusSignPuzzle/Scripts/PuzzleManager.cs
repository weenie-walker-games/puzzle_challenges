using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleManager : MonoSingleton<PuzzleManager>
    {

        [SerializeField] GameObject piecePrefab = null;
        private GameObject ghostPiece;
        private int currentOrientation = 0;
        [SerializeField] private PuzzlePieceLocation hoverLocation = null;

        private void OnEnable()
        {
            PuzzlePieceLocation.OnLocationHover += GetLocationUnderMouse;
        }

        private void OnDisable()
        {
            PuzzlePieceLocation.OnLocationHover -= GetLocationUnderMouse;
        }

        private void Start()
        {
            ghostPiece = Instantiate(piecePrefab, Vector3.zero, Quaternion.identity, this.transform);
        }

        private void Update()
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = -1;
            ghostPiece.transform.position = worldPosition;

            //Right click to rotate
            if (Input.GetMouseButtonDown(1))
            {
                currentOrientation = (currentOrientation + 1) % 4;
                float rotation = ReturnRotation((PieceOrientation)currentOrientation);
                ghostPiece.transform.localEulerAngles = new Vector3(0, 0, rotation);
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
            if (hoverLocation == null)
                return;

            bool didPlace = hoverLocation.PlacePiece(currentOrientation);

            if (didPlace)
            {
                ghostPiece.transform.rotation = Quaternion.Euler(Vector3.zero);
                currentOrientation = 0;
            }
            else
            {
                PuzzlePiece piece = ghostPiece.GetComponentInChildren<PuzzlePiece>();
                if(piece != null)
                    piece.PlayAnimation();
            }
        }

        private void GetLocationUnderMouse(PuzzlePieceLocation location, bool isUnder)
        {
            if(hoverLocation == location && !isUnder)
            {
                hoverLocation = null;
            }

            if (isUnder)
            {
                hoverLocation = location;
            }
        }
    }
}
