using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NumberPuzzle
{
    public class CreateLineRenderer : MonoBehaviour
    {
        [SerializeField] private List<CorrectLines> correctLines = new List<CorrectLines>();
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
                for (int i = 0; i < line.objectsInLine.Count - 1; i++)
                {
                    GameObject lineItem = Instantiate(lineObjectPrefab, Vector3.zero, Quaternion.identity, lineParentHolder);
                    LineRenderer lineRenderer;

                    CircleObject fromCircle = line.objectsInLine[i];
                    CircleObject toCircle = line.objectsInLine[i + 1];

                    DirectionEnum toDirection = GetDirection(fromCircle.transform, toCircle.transform);

                    if (lineItem.TryGetComponent<LineRenderer>(out lineRenderer))
                    {
                        lineRenderer.SetPosition(0, fromCircle.GetConnectionPointLocation(toDirection));
                        lineRenderer.SetPosition(1, toCircle.GetConnectionPointLocation(DirectionConverter(toDirection)));
                    }
                }
            }

            
        }

        private DirectionEnum GetDirection(Transform fromPos, Transform toPos)
        {
            DirectionEnum returnDirection = DirectionEnum.S;

            Vector2 distance = toPos.position - fromPos.position;

            Vector2 direction = Vector2.zero;

            direction.x = distance.x > 0 ? 1 : 0;
            direction.x = distance.x < 0 ? -1 : direction.x;
            direction.y = distance.y > 0 ? 1 : 0;
            direction.y = distance.y < 0 ? -1 : direction.y;

            if (direction.x == 0 && direction.y > 0)
            {
                returnDirection = DirectionEnum.N;
            }

            if (direction.x > 0 && direction.y < 0)
            {
                returnDirection = DirectionEnum.NE;
            }

            if (direction.x > 0 && direction.y == 0)
            {
                returnDirection = DirectionEnum.E;
            }

            if (direction.x > 0 && direction.y < 0)
            {
                returnDirection = DirectionEnum.SE;
            }

            if (direction.x == 0 && direction.y < 0)
            {
                returnDirection = DirectionEnum.S;
            }

            if (direction.x < 0 && direction.y < 0)
            {
                returnDirection = DirectionEnum.SW;
            }

            if (direction.x < 0 && direction.y == 0)
            {
                returnDirection = DirectionEnum.W;
            }

            if (direction.x < 0 && direction.y > 0)
            {
                returnDirection = DirectionEnum.NW;
            }


            return returnDirection;
        }

        private DirectionEnum DirectionConverter(DirectionEnum inDirection)
        {
            DirectionEnum outDirection = DirectionEnum.S;

            switch (inDirection)
            {
                case DirectionEnum.N:
                    outDirection = DirectionEnum.S;
                    break;
                case DirectionEnum.NE:
                    outDirection = DirectionEnum.SW;
                    break;
                case DirectionEnum.E:
                    outDirection = DirectionEnum.W;
                    break;
                case DirectionEnum.SE:
                    outDirection = DirectionEnum.NW;
                    break;
                case DirectionEnum.S:
                    outDirection = DirectionEnum.N;
                    break;
                case DirectionEnum.SW:
                    outDirection = DirectionEnum.NE;
                    break;
                case DirectionEnum.W:
                    outDirection = DirectionEnum.E;
                    break;
                case DirectionEnum.NW:
                    outDirection = DirectionEnum.SE;
                    break;
                default:
                    break;
            }

            return outDirection;
        }
    }

    [System.Serializable]
    public struct CorrectLines
    {
        public List<CircleObject> objectsInLine;
    }

}
