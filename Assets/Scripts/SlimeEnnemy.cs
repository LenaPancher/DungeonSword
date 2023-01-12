using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SlimeEnnemy : MonoBehaviour
{

    public Animator anim;

    public LayerMask playerLayer;

    public int maxHealth = 100;

    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        } else {
            anim.SetTrigger("hurt");
        }
        
    }

    void Die(){
        anim.SetTrigger("death");
        StartCoroutine(destroy());
        
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
