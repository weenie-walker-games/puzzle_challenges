using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Puzzle
{
    public class PuzzlePieceLocation : MonoBehaviour
    {
        public static event System.Action<PuzzlePieceLocation, bool> OnLocationHover;


        [SerializeField] private Renderer rend;
        [SerializeField] private Color fillColor = Color.yellow;
        private Material innerMat;
        private Material iconMat;
        private bool isMouseIn = false;
        private bool isPiecePlaced = false;

        [SerializeField] private PieceOrientation locationOrientation;
        public PieceOrientation LocationOrientation { get { return locationOrientation; } set { locationOrientation = value; } }

        private void OnEnable()
        {
            Material[] mats = rend.materials;
            innerMat = mats[1];
            iconMat = mats[2];
        }

        private void OnDisable()
        {
            
        }

        private void Start()
        {
            var rot = Quaternion.Euler(transform.rotation.eulerAngles);
            float zRot = rot.eulerAngles.z;

            switch (zRot)
            {
                case 0:
                    locationOrientation = PieceOrientation.Down;
                    break;
                case 90:
                    locationOrientation = PieceOrientation.Right;
                    break;
                case 180:
                    locationOrientation = PieceOrientation.Up;
                    break;
                case 270:
                    locationOrientation = PieceOrientation.Left;
                    break;
            }
        }


        private void OnMouseEnter()
        {
            ChangeColor(true);
            isMouseIn = true;

            OnLocationHover?.Invoke(this, true);
        }

        private void OnMouseExit()
        {
            ChangeColor(false);
            isMouseIn = false;

            OnLocationHover?.Invoke(this, false);
        }

        public void ChangeColor(bool isOver)
        {
            //stop changing the color once piece is placed
            if (isPiecePlaced)
                return;

            Debug.Log("Change color " + isOver);
            int alpha = isOver ? 255 : 0;
            Color col = new Color(255,0,0,alpha);
            innerMat.color = col;
        }

        public bool PlacePiece(int orientation)
        {
            //if already a piece here, ignore this click
            if (isPiecePlaced)
                return false;

            if ((int)locationOrientation == orientation)
            {
                //piece placed
                Debug.Log("Piece placed");
                isPiecePlaced = true;
                fillColor.a = 255;
                innerMat.color = fillColor;
                
                iconMat.color = Color.blue;
                return true;
            }
            else
            {
                Debug.Log("Does not match " + this.name);
                return false;
            }

        }

        public void UpdateTarget(PieceOrientation orientation)
        {
            locationOrientation = orientation;

            Vector3 newRot = Vector3.zero;
            switch (orientation)
            {
                case PieceOrientation.Down:
                    newRot = Vector3.zero;
                    break;
                case PieceOrientation.Right:
                    newRot.z = 90;
                    break;
                case PieceOrientation.Up:
                    newRot.z = 180;
                    break;
                case PieceOrientation.Left:
                    newRot.z = 270;
                    break;
                default:
                    break;
            }

            transform.rotation = Quaternion.Euler(newRot);
        }
    }
}
