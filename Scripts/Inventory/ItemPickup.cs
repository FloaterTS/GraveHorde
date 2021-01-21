using System;
using System.Collections;
using System.Collections.Generic;
//using Packages.Rider.Editor.UnitTesting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ItemPickup : MonoBehaviour
{
    public ItemObject pickupItem;
    public bool despawnItem = true;
    public float despawnTime = 60f;

    private void Start()
    {
        Despawn();
    }

    public void PickUp(ItemObject item)
    {
        bool canPickUp;
        canPickUp = Inventory.Instance.AddToInventory(item);

        if (canPickUp)
        {
            Destroy(gameObject);
        }
    }

    private void Despawn()
    {
        if (despawnItem)
            Destroy(gameObject, despawnTime);
    }
}
