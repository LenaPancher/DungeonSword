using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;
    public Transform attackPoint;
    public float attackRange = 0.5f;

    public int attackDamage = 30;
    float nextAttackTime = 0f;

    public float attackRate = 2f ;
    public LayerMask enemyLayers;
    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime){
            if (Input.GetKeyDown(KeyCode.E)){
                Attack();
                nextAttackTime = Time.time + 1.5f / attackRate;
            }
        }
        
    }
    void Attack() {
        anim.SetTrigger("attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies) {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                StartCoroutine(GiveDamages(enemy));
            }
        }
    }
    IEnumerator GiveDamages(Collider2D enemy){
        yield return new WaitForSeconds(0.5f);
        enemy.GetComponent<BringerEnemy>().TakeDamage(attackDamage);
    }

}
