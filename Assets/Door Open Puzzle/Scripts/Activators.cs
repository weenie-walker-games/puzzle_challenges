using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeenieWalker
{
    public class Activators : MonoBehaviour
    {
        public static event System.Action<Activators, bool> OnActivatorAction;

        private bool isActive = false;

        public Animator anim;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void OnMouseDown()
        {
            Debug.Log("Clicked");
            Activate();
        }

        public void Activate()
        {
            isActive = !isActive;
            OnActivatorAction?.Invoke(this, isActive);
            if (anim != null)
                anim.SetBool("Activated", isActive);
        }
    }
}
