using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private ItemUI[] itemUI;
    void Start()
    {
        itemUI = GetComponentsInChildren<ItemUI>();

        if (Inventory.Instance)
        {
            UpdateUIonChanged();
        }
    }
    
    public void UpdateUIonChanged()
    {
        for (int i = 0; i < itemUI.Length; i++)
        {
            if (i < Inventory.Instance.inventoryItems.Count)
            {  
                itemUI[i].DisplayItemUI(Inventory.Instance.inventoryItems[i]); 
            }
            else
            {
                itemUI[i].ClearSlotUI();
            }
        }
    }
    
}
