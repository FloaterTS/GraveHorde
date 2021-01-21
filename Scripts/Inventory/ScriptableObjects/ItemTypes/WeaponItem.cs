using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon", menuName = "Item Object/Weapon Item")]

public class WeaponItem : ItemObject
{
    [HideInInspector]
    public Queue<GameObject> bulletPool;
    public GameObject weaponPrefab;
    public GameObject bulletPrefab;
    public WeaponType weaponType;
    public Vector3 weaponPosition;
    public Vector3 weaponRotation;
    public float rateOfFire;
    public bool hasAutomaticFire;
}


public enum WeaponType
{
    Rifle = 0,
    Gun = 1
}