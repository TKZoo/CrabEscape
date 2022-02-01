using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoInteractionComponent : MonoBehaviour
{
   public void DoInteraction(GameObject go)
   {
      var interactible = go.GetComponent<InteractibleComponent>();
      if (interactible != null)
      {
         interactible.Interact();
      }
   }
}
