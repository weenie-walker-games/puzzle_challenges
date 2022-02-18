using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeenieWalker
{
    public class CreateLineRenderer : MonoBehaviour
    {
        [SerializeField] private List<CorrectLines> correctLines = new List<CorrectLines>();
        [SerializeField] private List<ConnectedObjects> connectedObjects = new List<ConnectedObjects>();
        [SerializeField] GameObject lineObjectPrefab;
        [SerializeField] Transform lineParentHolder;

        private void Start()
        {
            CreateLine();
        }

        public void CreateLine()
        {

            foreach (var line in correctLines)
            {

            }

            /*
            foreach (var item in connectedObjects)
            {
                GameObject lineItem = Instantiate(lineObjectPrefab, Vector3.zero, Quaternion.identity, lineParentHolder);
                LineRenderer lineRenderer;

                if(lineItem.TryGetComponent<LineRenderer>(out lineRenderer))
                {
                    lineRenderer.SetPosition(0, item.fromObject.circleObject.GetConnectionPointLocation(item.fromObject.directionEnum));
                    lineRenderer.SetPosition(1, item.toObject.circleObject.GetConnectionPointLocation(item.toObject.directionEnum));
                }
            }
            */
        }
    }
}
