using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowEffort
{
    public class BuilderScript : MonoBehaviour
    {

        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private Transform blockParent;
        [SerializeField] private Vector2 dimensions;

        private void Start()
        {
            for (int i = 0; i < dimensions.x; i++)
            {
                for (int j = 0; j < dimensions.y; j++)
                {
                    GameObject obj = Instantiate(blockPrefab, new Vector3(i, j, 0) + blockParent.position, Quaternion.identity, blockParent);
                    obj.name = "Position Block";
                }

            }

            blockParent.localScale *= 1.5f;
        }
    }
}
