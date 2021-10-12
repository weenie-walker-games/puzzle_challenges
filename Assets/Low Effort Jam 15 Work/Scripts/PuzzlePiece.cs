using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowEffort
{
    public class PuzzlePiece : MonoBehaviour
    {
        public static event System.Action<PuzzlePiece, bool> OnPieceHover;

        [SerializeField] private Collider pieceCollider;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float timeReset = 10f;

        private Vector3 startingPos;

        private bool isMouseIn = false;
        private bool isPiecePlaced = false;
        private bool isFalling = false;
        private Coroutine routine;
        private WaitForSeconds resetYield;

        private void OnEnable()
        {
            PuzzleManager.OnGameReset += GameReset;
        }

        private void OnDisable()
        {
            PuzzleManager.OnGameReset -= GameReset;
        }

        private void Start()
        {
            resetYield = new WaitForSeconds(timeReset);
            startingPos = transform.position;
        }

        private void GameReset()
        {
            if(routine != null)
                StopCoroutine(routine);

            isMouseIn = false;
            isPiecePlaced = false;

            transform.position = startingPos;
            pieceCollider.isTrigger = false;
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.down * 3, ForceMode.Acceleration);

            isFalling = true;
            routine = StartCoroutine("ResetRoutine");
        }

        IEnumerator ResetRoutine()
        {
            while (isFalling)
            {
                yield return resetYield;

                AfterFall();
            }
        }

        private void AfterFall()
        {
            isFalling = false;
            pieceCollider.isTrigger = true;
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            StopCoroutine(routine);
        }

        private void OnMouseEnter()
        {

            isMouseIn = true;

            OnPieceHover?.Invoke(this, true);
        }

        private void OnMouseExit()
        {

            isMouseIn = false;

            OnPieceHover?.Invoke(this, false);
        }
    }
}
