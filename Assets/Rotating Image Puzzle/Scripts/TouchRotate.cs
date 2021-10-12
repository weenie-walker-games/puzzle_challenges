using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageRotate
{
    public class TouchRotate : MonoBehaviour
    {
        private void Start()
        {
            int random = Random.Range(0, 4);
            SetRotation(random);
        }

        private void OnMouseDown()
        {
            if (!GameControl.youWin)
                transform.Rotate(0, 0f, 90f);

            //Force the rotation to match 0 degrees
            if(transform.rotation.z < 0.01f && transform.rotation.z > -0.01f)
            {
                SetRotation(0, true);
            }
        }


        private void SetRotation(int random, bool reset = false)
        {
            Vector3 rot = Vector3.zero;
            if (reset)
                rot.z = 0;
            else
                rot.z += 90 * random;
            transform.rotation = Quaternion.Euler(rot);
        }

    }
}
