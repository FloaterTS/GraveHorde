using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Item", menuName = "Item Object")]

/*[System.Serializable] 
public class ItemEvent : UnityEvent<ItemObject>{}*/
public class ItemObject : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
}
