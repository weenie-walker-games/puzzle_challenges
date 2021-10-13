using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowEffort
{
    public class BlockPiece : MonoBehaviour
    {
        [SerializeField] private bool isInPuzzle = false;
        [SerializeField] private Material inPuzzleMat;
        [SerializeField] private Material outPuzzleMat;
        [SerializeField] private Renderer rend;
        public bool LocationResult { get; private set; } = false;
        [SerializeField] private PlacementLocation location;
        [SerializeField] private GameObject triggerBlock;

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
            rend.material = isInPuzzle ? inPuzzleMat : outPuzzleMat;
        }

        private void GameReset()
        {
            triggerBlock.SetActive(false);
            location = null;
            LocationResult = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Location"))
            {

                location = other.GetComponent<PlacementLocation>();

                if(location != null)
                {
                    LocationResult = location.IsPartOfPuzzle == isInPuzzle;
                    if(location.IsPartOfPuzzle == isInPuzzle)
                    {
                        LocationResult = true;
                    }
                    else
                    {
                        LocationResult = false;
                    }
                }

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Location"))
            {
                LocationResult = false;
            }
            location = null;
        }

        public void CheckResults()
        {
            triggerBlock.SetActive(true);

            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.forward, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Location"))
                {
                    location = hit.collider.gameObject.GetComponent<PlacementLocation>();
                }
            }

            if (location == null)
                return;

            if (location.IsPartOfPuzzle == isInPuzzle)
            {
                LocationResult = true;
            }
            else
            {
                LocationResult = false;
            }

        }

        public void ResetResults()
        {
            LocationResult = false;
        }
    }
}
