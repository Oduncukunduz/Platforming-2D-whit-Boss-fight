using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Referanslar")]
    public Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Saldırı")]
    public float meleeRange = 1.5f;
    public GameObject meleeHitbox;

    [Header("Takip")]
    [SerializeField] float chaseRange = 3f;
    [SerializeField] float moveSpeed = 3f;

    private AudioSource audioSource;

    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (GetComponent<BossHealtBar>().IsRecovering())
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isRunning", false);
            isAttacking = false;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (!isAttacking)
        {
            if (distance <= meleeRange)
            {
                rb.linearVelocity = Vector2.zero;
                animator.SetBool("isRunning", false);
                StartCoroutine(MeleeAttack());
            }
            else if (distance <= chaseRange)
            {
                ChasePlayer();
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                animator.SetBool("isRunning", false);
            }
        }
    }

    void ChasePlayer()
    {
        animator.SetBool("isRunning", true);

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        // Sola bakan sprite için tersine çevirdik
        if (direction.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    System.Collections.IEnumerator MeleeAttack()
    {
        isAttacking = true;

        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isRunning", false);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(1f);

        isAttacking = false;
    }

    public void EnableHitbox()
    {
        meleeHitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        meleeHitbox.SetActive(false);
    }
    // void Recover()
    // {
    //     animator.SetTrigger("Recover");
    // }

}