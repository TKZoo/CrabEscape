using UnityEngine;

public class ArmHeroComponent : MonoBehaviour
{
   private Hero _hero;


   public void ArmHero(GameObject go)
   {
      var hero = go.GetComponent<Hero>();
      if (hero != null)
      {
         hero.ArmHero();
      }
   }
   
}
