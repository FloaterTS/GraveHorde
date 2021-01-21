using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Consumable", menuName = "Item Object/Consumable Item")]

/*[System.Serializable] 
public class ItemEvent : UnityEvent<ItemObject>{}*/
public class ConsumableItem : ItemObject
{
    public List<ItemEffect> itemEffects;

    public void Use(GameObject gameObject)
    {
        foreach (var effect in itemEffects)
        {
            effect.Use(gameObject);
        }
    }
}