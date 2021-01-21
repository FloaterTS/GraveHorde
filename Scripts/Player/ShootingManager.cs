using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    public static ShootingManager instance;

    [SerializeField] private Transform bulletsParent = null;
    [SerializeField] private Transform weaponTransform = null;
    [SerializeField] private Vector3 weaponTransformOffset = Vector3.zero;
    [SerializeField] private float bulletForce = 25f;
    [SerializeField] private float minClickDistanceFromPlayer = .25f;
    [SerializeField] bool zombieAutoAimHeightAssist = true;

    private CharacterControl characterControl;
    private float shootCooldown = 0f;
    private float bulletLifetime = 5f;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("Another shooting manager present.");

        characterControl = GetComponent<CharacterControl>();
    }

    void Update()
    {
        shootCooldown -= Time.deltaTime;

        if (characterControl.IsAimingWithWeapon())
        {
            WeaponItem equippedWeapon = characterControl.GetEquippedWeapon();
            if (equippedWeapon.hasAutomaticFire)
            {
                if (Input.GetButton("Fire1"))
                    ShootAt(equippedWeapon);
                
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                    ShootAt(equippedWeapon);
                
                   
            }
        }
    }

    void ShootAt(WeaponItem equippedWeapon)
    {
        if (shootCooldown > 0f)
            return;
        SoundManagerScript.PlaySound("pistol");

        
        Ray locationRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(locationRay, out RaycastHit hitLocation, 300f, ~LayerMask.GetMask("Ignore Raycast")))
        {
            Vector3 bulletDirection;

            if (zombieAutoAimHeightAssist && hitLocation.collider.gameObject.CompareTag("Zombie"))
            {
                bulletDirection = hitLocation.point - (weaponTransform.position + transform.rotation * weaponTransformOffset);
            }
            else
            {
                bulletDirection = hitLocation.point - transform.position;
                bulletDirection.y = 0f;
            }

            if (bulletDirection.magnitude < minClickDistanceFromPlayer)
                return;

            GameObject bullet;
            if (equippedWeapon.bulletPool.Count > 0)
            {
                bullet = equippedWeapon.bulletPool.Dequeue();
                bullet.transform.position = weaponTransform.position + transform.rotation * weaponTransformOffset;
                bullet.transform.rotation = transform.rotation;
                bullet.SetActive(true);
            }
            else
            {
                bullet = Instantiate(equippedWeapon.bulletPrefab, weaponTransform.position + transform.rotation * weaponTransformOffset, transform.rotation, bulletsParent);
            }
            bullet.GetComponent<Rigidbody>().velocity = bulletDirection.normalized * bulletForce;

            StartCoroutine(StoreBulletInPool(bullet, equippedWeapon));

            shootCooldown = equippedWeapon.rateOfFire;
        }
    }

    IEnumerator StoreBulletInPool(GameObject bullet, WeaponItem equippedWeapon)
    {
        yield return new WaitForSeconds(bulletLifetime);
        bullet.SetActive(false);
        equippedWeapon.bulletPool.Enqueue(bullet);
    }
}
