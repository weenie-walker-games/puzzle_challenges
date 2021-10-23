using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WeenieWalker
{
    public class Door : MonoBehaviour
    {
        public Animator anim;

        //public List<Activators> activators = new List<Activators>();
        //Dictionary<Activators, bool> activatorsDictionary = new Dictionary<Activators, bool>();

        public List<ActivatorStatusItem> activatorItems = new List<ActivatorStatusItem>();
        Dictionary<Activators, bool> activatorItemsDictionary = new Dictionary<Activators, bool>();

        bool isDoorOpen = false;

        private void OnEnable()
        {
            Activators.OnActivatorAction += ReceiveActivator;
        }

        private void OnDisable()
        {
            Activators.OnActivatorAction -= ReceiveActivator;
        }

        private void ReceiveActivator(Activators activated, bool isActive)
        {
            var item = activatorItems.FirstOrDefault(a => a.item == activated);
            if (activatorItems.Contains(item))
            {
                activatorItemsDictionary[item.item] = isActive;
                OpenDoor(CheckResults());

            }

            //if (activators.Contains(activated))
            //{
            //    activatorsDictionary[activated] = isActive;
            //    OpenDoor(CheckResults());
            //}
        }

        private void Start()
        {
            //for (int i = 0; i < activators.Count; i++)
            //{
            //    activatorsDictionary.Add(activators[i], false);
            //}

            for (int i = 0; i < activatorItems.Count; i++)
            {
                activatorItemsDictionary.Add(activatorItems[i].item, false);
            }

            //isDoorOpen = AC.GlobalVariables.GetVariable(0).BooleanValue;
        }

        private bool CheckResults()
        {
            bool result = true;

            result = activatorItems.All(a => a.status == activatorItemsDictionary[a.item]);

            //foreach (KeyValuePair<Activators,bool> item in activatorsDictionary)
            //{
            //    if(item.Value == false)
            //    {
            //        result = false;
            //        break;
            //    }
            //}

            return result;
        }

        private void OpenDoor(bool isOpen)
        {
            anim.SetBool("Open", isOpen);
            //AC.GlobalVariables.GetVariable(0).BooleanValue = isOpen;
            
            //For AC 2D games, turn off/on the collider depending on if the door is open
            //Have a hotspot hidden behind that is activated when door is open
        }
    }

    [System.Serializable]
    public struct ActivatorStatusItem
    {
        public Activators item;
        public bool status;
    }
}
