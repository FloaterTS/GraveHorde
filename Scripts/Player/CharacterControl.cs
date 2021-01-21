using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class CharacterControl : MonoBehaviour
{
    public CharacterController controller;
    //public float aimMoveSpeed = 2.5f;
    public float moveSpeed = 5f;
    public float aimMoveSpeedDecrease = 2f;
    public float gravity = -9.81f;
    public float rotateSpeed = 10f;
    public float aimRotateSpeed = 7f;
    public Transform handSlot;
    
    [HideInInspector]
    public bool speedEffectOn = false;
    
    private float moveHorizontal, moveVertical;
    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;
    private bool isAiming;
    
    private GameObject equippedWeaponPrefab;
    private WeaponItem equippedWeaponItem;
    private bool weaponIsEquipped;
    
    private bool onStopAiming;
    private float currentSpeed;
    private bool checkSpeedChange;
    private float currentAimMoveSpeed;
    private float goodSpeed;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentSpeed = moveSpeed;
        goodSpeed = moveSpeed;
        currentAimMoveSpeed = currentSpeed / aimMoveSpeedDecrease;
        
        if (handSlot.childCount != 0)
            equippedWeaponPrefab = handSlot.GetChild(0).gameObject;
    }

    private void Update()
    {
        GetInput();
        ApplyGravity();
        Move();
        ApplyAnimations();
        RotateToCursor();
    }

    private void GetInput()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        isAiming = Input.GetMouseButton(1);
        onStopAiming = Input.GetMouseButtonUp(1);
        //onStartAiming = Input.GetMouseButtonDown(1);
    }

    void Move()
    {
        Vector3 move = new Vector3(moveHorizontal, 0f, moveVertical);
        if (move.magnitude >= 0.1f)
        {
            //SoundManagerScript.PlaySound("footstep");
            var scale = currentSpeed * Time.deltaTime;

            if (move.magnitude >= 1)
            {
                move = move.normalized;
            }
            controller.Move(move * scale);

            //Don't use move rotation when aiming
            if (!isAiming)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move),
                    rotateSpeed * Time.deltaTime);
            }
        }
    }

    void ApplyGravity()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void ApplyAnimations()
    {
        Vector3 inputVector = new Vector3(moveHorizontal, 0f, moveVertical);
        Vector3 animationVector = transform.InverseTransformDirection(inputVector);
        var velocityX = animationVector.x;
        var velocityZ = animationVector.z;

        //play different animation when aiming
        animator.SetBool("isAiming", isAiming);
        if (equippedWeaponItem)
        {
            weaponIsEquipped = true;
            //equipped weapon animation (rifle or gun)
            int weaponType = (int) equippedWeaponItem.weaponType;
            animator.SetInteger("weaponType", weaponType);
        }
        else
        {
            weaponIsEquipped = false;
            //no weapon animation
            animator.SetInteger("weaponType", -1);
        }

        animator.SetFloat("VelX", velocityX);
        animator.SetFloat("VelY", velocityZ);
    }

    void RotateToCursor()
    {
        CheckSpeedEffectChange();
        //Rotate player to cursor when aiming (pressing right click)
        if (isAiming)
        {
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitDist = 0.0f;
            
            if (playerPlane.Raycast(ray, out hitDist))
            {
                Vector3 targetPoint = ray.GetPoint(hitDist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                targetRotation.x = 0;
                targetRotation.z = 0;
                currentSpeed = currentAimMoveSpeed;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aimRotateSpeed * Time.deltaTime);
            }
        }
        else if (onStopAiming)
        {
            currentSpeed = goodSpeed;
        }
    }

    void CheckSpeedEffectChange()
    {
        //check if speed effect was turned on
        if (speedEffectOn != checkSpeedChange) 
        {  
            checkSpeedChange=speedEffectOn;
            goodSpeed = moveSpeed;
            
            //save speed to apply during the speed effect
            if (speedEffectOn)
            {
                goodSpeed = currentSpeed;
            }
            currentAimMoveSpeed = goodSpeed / aimMoveSpeedDecrease;
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        ItemPickup item = other.GetComponent<ItemPickup>();
        if (item)
        {
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("player picked up " + item.pickupItem.itemName);
                item.PickUp(item.pickupItem);
            }
        }
    }

    //assigned to inventory onUse event in inspector
    public void UseItem(ItemObject item)
    {
        //chcek if item is a consumable or a weapon
        //use the consumable effect on player if consumable and equip if weapon
        if (item is ConsumableItem consumableItem)
        {
            if (consumableItem.name == "Healing Item")
                SoundManagerScript.PlaySound("heal");
            else if (consumableItem.name == "HealSpeed item")
                SoundManagerScript.PlaySound("speed");

            consumableItem.Use(gameObject);
            Inventory.Instance.RemoveFromInventory(item);
            Debug.Log("Used consumable :" + consumableItem.name);
        }
        else if (item is WeaponItem weaponItem)
        {
            SoundManagerScript.PlaySound("changeweapon");

            //destroy current equipped item when equipping a new one
            if (equippedWeaponPrefab != null)
            {
                //equippedWeaponPrefab.SetActive(false);
                Destroy(equippedWeaponPrefab);
            }
            
            //equip weapon on character at given position
            equippedWeaponPrefab=Instantiate(weaponItem.weaponPrefab, handSlot);
            equippedWeaponPrefab.transform.localPosition = weaponItem.weaponPosition;
            equippedWeaponPrefab.transform.localEulerAngles = weaponItem.weaponRotation;
            equippedWeaponItem = weaponItem;
            equippedWeaponPrefab.SetActive(true);

            if (weaponItem.bulletPool == null)
                weaponItem.bulletPool = new Queue<GameObject>();
            else
                weaponItem.bulletPool.Clear();
        }
        else
        {
            Debug.Log("not consumable or weapon");
        }
    }

    //assigned to inventory onRemoved event in inspector
    //unequips weapon from character when delete(x) button is pressed
    public void RemoveItem(ItemObject item)
    {
        if (!(item is WeaponItem weaponItem)) return;
        if (weaponItem != equippedWeaponItem) return;
        Destroy(equippedWeaponPrefab);
        equippedWeaponPrefab = null;
        equippedWeaponItem = null;
        Debug.Log("weapon removed & unequipped");

    }

    public float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = value;
    }

    public bool IsAimingWithWeapon()
    {
        return isAiming && weaponIsEquipped;
    }

    public WeaponItem GetEquippedWeapon()
    {
        return equippedWeaponItem;
    }
    
}