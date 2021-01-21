using UnityEngine;


[CreateAssetMenu(fileName = "New Health Effect", menuName = "Item Effects/Consumable Effects/Regen Effect")]
public class RegenEffect : ItemEffect
{
    public float healthIncreasedPerSecond;
    public float duration;
    public override void Use(GameObject gameObject)
    {
        var playerHealth = gameObject.GetComponent<PlayerHealth>();
        playerHealth.StartCoroutine(playerHealth.RegenEffect(duration, healthIncreasedPerSecond));
    }
}