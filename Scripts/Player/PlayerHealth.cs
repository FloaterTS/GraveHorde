using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class PlayerHealth : MonoBehaviour
{
    public Healthbar healthbar;
    public int maxHealth = 100;
    public float currentHealth;
    public bool active;
    public bool regenOn;
    public float healthIncreasedPerSecond;

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (regenOn)
        {
            HealPlayer(healthIncreasedPerSecond * Time.deltaTime);
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }
    }
    
    public void GameOver()
    {
        if (active == true)
        {
            Debug.Log("ACTIVE");
            gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("NONACTIVE");
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth((int)currentHealth);
        
        if (currentHealth < 0)
        {
            //gameObject.SetActive(true);
            SceneManager.LoadScene("Scenes/Menu");
        }
    }
    
    public void HealPlayer(float amount)
    {
        currentHealth += amount;
        healthbar.SetHealth((int)currentHealth);
    }


    private void Die()
    {
        int rng = Random.Range(1, 3);

        if (rng == 1)
            SoundManagerScript.PlaySound("zombiedeath1");
        else
            SoundManagerScript.PlaySound("zombiedeath2");
        
        animator.SetTrigger("dieBack");
    }
    
    public IEnumerator RegenEffect(float duration, float healthIncrease)
    {
        Debug.Log("started regen effect ");
        var initRegenOn = regenOn;
        regenOn = true;
        var initialIncrease = healthIncreasedPerSecond;
        healthIncreasedPerSecond = healthIncrease;
        yield return new WaitForSeconds(duration);
        regenOn = initRegenOn;
        healthIncreasedPerSecond = initialIncrease;
        Debug.Log("removed regen effect ");
    }
}