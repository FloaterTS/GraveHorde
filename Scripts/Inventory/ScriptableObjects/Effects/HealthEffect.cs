using UnityEngine;


    [CreateAssetMenu(fileName = "New Health Effect", menuName = "Item Effects/Consumable Effects/Health Effect")]
    public class HealthEffect : ItemEffect
    {
        public int healAmount = 10 ;
        public override void Use(GameObject gameObject)
        {
            var playerHealth = gameObject.GetComponent<PlayerHealth>();
            playerHealth.HealPlayer(healAmount);
        }
    }
