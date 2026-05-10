//using System.Diagnostics;
//using System.Numerics;


using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerMove : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    float gravityScaleAtStart;
    BoxCollider2D boxCollider2D;
    CapsuleCollider2D myBodyCollider;
    bool isAlive = true;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpspped = 5f;
    [SerializeField] float Climbspped = 5f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] float invincibilityDuration = 1.5f;
    [SerializeField] int maxLives = 3;
    [SerializeField] TextMeshProUGUI livesText;

    public Vector2 deadkick = new Vector2(10f, 20f);
    private float lastAttackTime = 0f;

    private int currentLives;
    private bool isInvincible = false;
    private AudioSource audioSource;



    void Start()
    {
        //my rigidbody kısmınna erişim sağlıyor 
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        audioSource = GetComponent<AudioSource>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        currentLives = maxLives;
        livesText.text = currentLives.ToString();
    }
    void Update()
    {
        if (!isAlive) { return; }

        Run();
        FlipSprite();
        //Running();
        ClimbLadder();
        Die();

    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();

    }

    void Run()
    {
        //myRigidbody kısmı oyuncu baz alarak y ekseninde harekt yapamsını değil yerçekimini baz alarak bu hareketi yapmasını sağlıyor
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.linearVelocity.y);
        myRigidbody.linearVelocity = playerVelocity;

        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", hasHorizontalSpeed);
    }
    void OnJump(InputValue value)
    {
        if (!boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (value.isPressed)
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpspped);
        }
        // else if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        // {
        //     myRigidbody.linearVelocity += new Vector2(0f, jumpspped);
        // }


    }
    void ClimbLadder()
    {
        if (boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidbody.gravityScale = 0f;

            myRigidbody.linearVelocity = new Vector2(
                myRigidbody.linearVelocity.x,
                moveInput.y * Climbspped
            );
            Vector2 climbVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.linearVelocity.y);
            myRigidbody.linearVelocity = climbVelocity;

            bool verticalSpeed = Mathf.Abs(myRigidbody.linearVelocity.y) > Mathf.Epsilon;
            myAnimator.SetBool("isClimb", verticalSpeed);

        }
        else
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimb", false);

        }
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
        }
    }
    // void Running()
    // {
    //     bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
    //     if (hasHorizontalSpeed)
    //     {
    //         myAnimator.SetBool("isRunning", true);
    //     }
    //     else
    //     {
    //         myAnimator.SetBool("isRunning", false);
    //     }
    // }
    void OnAttack(InputValue value)
    {
        if (!isAlive) { return; }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            Instantiate(bullet, gun.position, transform.rotation);
        }
    }
    void Die()
    {
        if (isInvincible) { return; }

        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemys", "Hazards", "Boss")))
        {
            currentLives--;
            livesText.text = currentLives.ToString();

            if (currentLives <= 0)
            {
                isAlive = false;
                myAnimator.SetTrigger("Dying");
                myRigidbody.linearVelocity = deadkick;
                FindObjectOfType<GameSes>().processPlayerDeath();
            }
            else
            {
                StartCoroutine(InvincibilityFrames());
            }
        }
    }
    System.Collections.IEnumerator InvincibilityFrames()
    {
        isInvincible = true;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float elapsed = 0f;

        while (elapsed < invincibilityDuration)
        {
            sr.enabled = !sr.enabled; // Aç/kapa
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        sr.enabled = true; // Sona erince görünür yap
        isInvincible = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            if (isInvincible) { return; }

            currentLives--;
            livesText.text = currentLives.ToString();

            if (currentLives <= 0)
            {
                isAlive = false;
                myAnimator.SetTrigger("Dying");
                myRigidbody.linearVelocity = deadkick;
                FindObjectOfType<GameSes>().processPlayerDeath();
            }
            else
            {
                StartCoroutine(InvincibilityFrames());
            }
        }
    }

}

