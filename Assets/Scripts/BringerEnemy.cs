using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BringerEnemy : MonoBehaviour
{

    public Animator anim;
    public Transform attackPoint;
    public float attackRange = 1.3f;

    [SerializeField] private AudioSource hitSoundEffect;
    [SerializeField] private AudioSource attackSoundEffect;
    [SerializeField] private AudioSource castSpellSoundEffect;
    [SerializeField] private AudioSource spellSoundEffect;
    [SerializeField] private AudioSource deathSoundEffect;


    bool attacking = false;

    public int attackDamageHeavy = 50;
    public int attackDamage = 20;
    float nextAttackTime = 0f;

    public float attackRate = 2f ;

    public LayerMask playerLayer;

    public int maxHealth = 100;

    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        
        foreach (Collider2D enemy in hitEnemies) {
            if(Time.time >= nextAttackTime && enemy.gameObject.CompareTag("Player")){
                RandomAttack();
                nextAttackTime = Time.time + 6f / attackRate;
            }
        }
        
    }

    void RandomAttack(){
        attacking = true;
        System.Random rand = new System.Random();
        int num = rand.Next(0, 2);
        if (num == 0) {
            anim.SetTrigger("attack");
            StartCoroutine(Damaging(1.3f, attackDamage, "attack"));
        }
        else {
            anim.SetTrigger("spell");
            castSpellSoundEffect.Play();
            StartCoroutine(Damaging(1.5f, attackDamageHeavy, "spell")); 
        }
            
    }

    IEnumerator Damaging(float duration, int damages, string attackType) {
        yield return new WaitForSeconds(duration);
        switch (attackType)
        {
            case "attack": attackSoundEffect.Play(); break;
            case "spell": spellSoundEffect.Play(); break;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach (Collider2D enemy in hitEnemies) {
            if (enemy.gameObject.CompareTag("Player"))  
            {
                enemy.GetComponent<PlayerLife>().TakeDamage(damages);
            }
            attacking = false; 
        }
    }

    public void TakeDamage(int damage){
        if (attacking == false){
            currentHealth -= damage;
            
            if (currentHealth <= 0)
            {
                deathSoundEffect.Play();
                Die();
            } else {
                hitSoundEffect.Play();
                anim.SetTrigger("hurt");
            }
        }
        
    }

    void Die(){
        attacking = true;
        anim.SetTrigger("death");
        StartCoroutine(destroy());
        
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
