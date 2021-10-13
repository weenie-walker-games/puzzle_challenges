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
        [SerializeField] private Material availableMat;
        [SerializeField] private Material coveredMat;
        [SerializeField] private List<Renderer> glowRends = new List<Renderer>();
        [SerializeField] private List<Collider> borderColliders = new List<Collider>();
        [SerializeField] List<BlockPiece> blocks = new List<BlockPiece>();
        public bool AllCorrect { get; private set; } = false;
        public bool allCorrect;

        private Vector3 startingPos;
        private Quaternion startRot;
        public int CurrentOrientation { get; set; }

        private bool isMouseIn = false;
        private bool isCurrentlySelected = false;
        public bool IsCurrentlySelected { get { return isCurrentlySelected; } }
        [SerializeField] private PlacementLocation currentlyHoveredLocation;
        private bool isPiecePlaced = false;
        public bool IsAvailableForPlacement { get; private set; } = true;
        private bool isFalling = false;
        private Coroutine routine;
        private WaitForSeconds resetYield;

        private void OnEnable()
        {
            PuzzleManager.OnGameReset += GameReset;
            PuzzleManager.OnHoverButtonClick += ButtonClicked;
        }

        private void OnDisable()
        {
            PuzzleManager.OnGameReset -= GameReset;
            PuzzleManager.OnHoverButtonClick -= ButtonClicked;
        }

        private void Start()
        {
            resetYield = new WaitForSeconds(timeReset);
            startingPos = transform.position;
            startRot = transform.rotation;
        }

        public void ReleasePiece()
        {
            gameObject.layer = LayerMask.NameToLayer("MousePosition");
            Resetting(false);
        }

        private void GameReset()
        {
            transform.position = startingPos;
            transform.rotation = startRot;
            Resetting();
        }

        private void Resetting(bool addExplosiveForce = true)
        {
            if (routine != null)
                StopCoroutine(routine);

            glowRends.ForEach(r => r.gameObject.SetActive(false));
            isMouseIn = false;
            isPiecePlaced = false;
            isCurrentlySelected = false;

            pieceCollider.isTrigger = false;
            rb.isKinematic = false;
            rb.useGravity = true;
            borderColliders.ForEach(c => c.enabled = true);
            
            if (addExplosiveForce)
            {
                rb.AddExplosionForce(100, new Vector3(10, 10, -3), 24f, 10f, ForceMode.VelocityChange);
                rb.AddExplosionForce(100, Vector3.zero, 24f, 10f, ForceMode.VelocityChange);
            }
            else
            {
                rb.AddForce(Vector3.down * 8, ForceMode.VelocityChange);
            }

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
            isPiecePlaced = false;
            isFalling = false;
            pieceCollider.isTrigger = true;
            borderColliders.ForEach(c => c.enabled = false);
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;

            if(routine != null)
                StopCoroutine(routine);
        }

        private void OnMouseEnter()
        {

            isMouseIn = true;

        }

        private void OnMouseExit()
        {

            isMouseIn = false;


        }

        private void ButtonClicked(bool isPickingUp)
        {
            if (!isMouseIn)
                return;

            if (isPickingUp)
            {
                PuzzleManager.Instance.GetPieceUnderMouse(this, true);
                CurrentOrientation = Mathf.RoundToInt(transform.rotation.z / 90f);
                AfterFall();
                Rotate(false);
                blocks.ForEach(b => b.ResetResults());
                AllCorrect = false;

                glowRends.ForEach(r => r.gameObject.SetActive(true));
                gameObject.layer = 2;

            }
        }


        public void Rotate(bool toAddRotation = true)
        {
            CurrentOrientation = toAddRotation? (CurrentOrientation + 1) % 4: CurrentOrientation;
            transform.localEulerAngles = new Vector3(0, 0, CurrentOrientation * 90);
        }

        public void Placed()
        {

            //Set the variable to true for the test
            bool tempResults = true;
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].CheckResults();
            }

            for (int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i].LocationResult == false)
                {
                    tempResults = false;
                    break;
                }
            }

            AllCorrect = tempResults;

            isPiecePlaced = true;


            isFalling = false;
            pieceCollider.isTrigger = true;
            borderColliders.ForEach(c => c.enabled = false);
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            gameObject.layer = LayerMask.NameToLayer("MousePosition");

            glowRends.ForEach(r => r.gameObject.SetActive(false));

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PuzzlePiece"))
            {
                glowRends.ForEach(r => r.material = coveredMat);
                IsAvailableForPlacement = false;
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("PuzzlePiece"))
            {
                glowRends.ForEach(r => r.material = availableMat);
                IsAvailableForPlacement = true;
            }
        }
    }
}
