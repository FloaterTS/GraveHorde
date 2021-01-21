using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

[System.Serializable] 
public class ItemInteractEvent : UnityEvent<ItemObject>{}

public class Inventory : MonoBehaviour
{
    public float maxSpace;
    public List<ItemObject> inventoryItems;
    public UnityEvent onChanged;
    public ItemInteractEvent onUsed, onRemoved;
	
    public static Inventory Instance => inventoryInstance;
	
	private static Inventory inventoryInstance;

    void Awake()
    {
        if (inventoryInstance != null && inventoryInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        inventoryInstance = this;
    }

    public bool AddToInventory(ItemObject item)
    {
        //check for max space
        bool canAdd = false;
        if (inventoryItems.Count < maxSpace)
        {
            inventoryItems.Add(item);
            onChanged.Invoke();
            canAdd = true;
        }
        else
        {
            Debug.Log("inventory is full");
        }

        return canAdd;
    }

    public void RemoveFromInventory(ItemObject item)
    {
        inventoryItems.Remove(item);
        onRemoved.Invoke(item);
        onChanged.Invoke();
    }

    public void UseItem(ItemObject item)
    {
        if (onUsed != null)
        {
            onUsed.Invoke(item);
        }
    }
}

