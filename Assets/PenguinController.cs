using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinController : MonoBehaviour
{

    public Animator anim;
    public Transform attackPoint;
    public float attackRange = 1.3f;
    private bool isDead = false;

    [SerializeField] private AudioSource hitSoundEffect;

    bool attacking = false;

    public int attackDamageHeavy = 50;
    public int attackDamage = 10;
    float nextAttackTime = 0f;

    public float attackRate = 2f ;

    public LayerMask playerLayer;

    public int maxHealth = 40;

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
            if(Time.time >= nextAttackTime && enemy.gameObject.CompareTag("Player") && isDead == false){
                RandomAttack();
                nextAttackTime = Time.time + 3f / attackRate;
            }
        }
        
    }

    void RandomAttack(){
        attacking = true;
        System.Random rand = new System.Random();
        int num = rand.Next(0, 2);
        if (num == 0) {
            anim.SetTrigger("attack");
            StartCoroutine(Damaging(0.2f, attackDamage));
        }
        else {
            anim.SetTrigger("attack");
            StartCoroutine(Damaging(0.2f, attackDamage)); 
        }
            
    }

    IEnumerator Damaging(float duration, int damages) {
        yield return new WaitForSeconds(duration);
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
                isDead = true;
                Die();
            } else {
                hitSoundEffect.Play();
                anim.SetTrigger("hurt");
            }
        }
        
    }

    void Die(){
        anim.SetTrigger("death");
        StartCoroutine(destroy());
        
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
