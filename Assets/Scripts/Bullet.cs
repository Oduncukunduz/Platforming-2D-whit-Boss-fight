
using UnityEngine;

public class Bullet : MonoBehaviour
{


    [SerializeField] float bulletSpeed = 20f;
    Rigidbody2D myRigidbody;
    PlayerMove player;
    Animator animator;
    float xSpeed;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMove>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        myRigidbody.linearVelocity = new Vector2(xSpeed, 0f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Boss"))
        {
            Animator bossAnimator = other.gameObject.GetComponent<Animator>();
            bossAnimator.SetTrigger("Hurt");

            BossHealtBar bossHealth = other.gameObject.GetComponent<BossHealtBar>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(25);
            }

            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        Destroy(gameObject, 1f);
    }
}