using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class ItemUI : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Image icon;
    public Button removeButton;
    private ItemObject item;
    private KeyCode keyCode = KeyCode.None;


    private void Awake()
    {
        var keyNumber = transform.GetSiblingIndex() + 1;
        keyCode = KeyCode.Alpha0 + keyNumber;
    }

    public void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            UseItem();
        }
    }

    public void DisplayItemUI(ItemObject itemObject)
    {
        item = itemObject;
        icon.sprite = itemObject.itemIcon;
        icon.enabled = true;
        if (item)
        {
            removeButton.interactable = true;
        } 
    }

    public void ClearSlotUI()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    //onclick event for remove button
    public void RemoveItemClick()
    {
        Inventory.Instance.RemoveFromInventory(item);
    }
    
    //onclick event for slot button
    public void UseItem()
    {
        Inventory.Instance.UseItem(item);
    }

}
