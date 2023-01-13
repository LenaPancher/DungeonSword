using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public Image healthBar;
    private Rigidbody2D rb;
    private Animator anim;
    private bool isDead = false;

    public int maxHealth = 100;

    int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spikes") && isDead == false)
        {
            isDead = true;
            Die(); 
        } 
        
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        if (healthBar != null){
            float diff = (float)currentHealth / (float)maxHealth;
            healthBar.fillAmount = diff;
        }

        if (currentHealth <= 0)
        {
            Die();
        } else {
            rb.velocity = new Vector2(-15f, 5f);
        }
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
