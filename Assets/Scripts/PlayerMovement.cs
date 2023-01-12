using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    public Transform attackPoint;
    public float attackRange = 0.5f;

    private enum MovementState { idle, running, jumping, attack, die }

    [SerializeField] private LayerMask jumpableGround;
    public LayerMask enemyLayers;


    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float jumpForce = 7f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        Vector2 playerMovement = new Vector2(dirX*moveSpeed, rb.velocity.y);

        Collider2D[] hitWalls = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, jumpableGround);

        foreach (Collider2D wall in hitWalls) {
            playerMovement = new Vector2(0f, rb.velocity.y);
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies) {
            if(enemy.gameObject.CompareTag("Enemy") || enemy.gameObject.CompareTag("Slime"))
            playerMovement = new Vector2(0f, rb.velocity.y);
        }
        rb.velocity = playerMovement;
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();

    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            transform.localScale = new Vector2(-0.7f,0.7f);
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            transform.localScale = new Vector2(0.7f,0.7f);
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 4f)
        {
            state = MovementState.jumping;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
