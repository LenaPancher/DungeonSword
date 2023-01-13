using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinController : MonoBehaviour
{

    public bool moving = true;
    private bool goingLeft = false;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public Animator anim;
    public Transform attackPoint;
    public float attackRange = 1.3f;
    private bool isDead = false;

    [SerializeField] private AudioSource hitSoundEffect;

    bool attacking = false;

    float nextChangeDirectionTime = 0f;
    public float moveRate = 1.5f;
    bool hurt = false;

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
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (moving)
        {
            Vector2 moving = new Vector2(0.0f, rb.velocity.y);
            if (!attacking && !hurt)
            {
                if (goingLeft && Time.time < nextChangeDirectionTime)
                {
                    moving = new Vector2(-3.0f, rb.velocity.y);
                    spriteRenderer.flipX = true;
                } else if (Time.time < nextChangeDirectionTime)
                {
                    moving = new Vector2(3.0f, rb.velocity.y);
                    spriteRenderer.flipX = false;
                } 
                else {
                    nextChangeDirectionTime = Time.time + moveRate;
                    goingLeft = !goingLeft;
                }
            }
            rb.velocity = moving;           
        }


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
        anim.SetTrigger("attack");
        StartCoroutine(Damaging(0.2f, attackDamage));
            
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
        hurt = true;
        if (attacking == false){
            currentHealth -= damage;
            
            if (currentHealth <= 0)
            {
                isDead = true;
                Die();
            } else {
                hitSoundEffect.Play();
                anim.SetTrigger("hurt");
                StartCoroutine(EndHurt());
            }
        }
        
    }

    void Die(){
        anim.SetTrigger("death");
        StartCoroutine(destroy());
        
    }

    IEnumerator EndHurt(){
        yield return new WaitForSeconds(0.3f);
        hurt = false;
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
