using UnityEngine;
using UnityEngine.UI;

public class BossHealtBar : MonoBehaviour
{
    [Header("Can")]
    [SerializeField] int maxHealth = 100;
    private int currentHealth;
    private bool hasRecovered = false;
    private bool isDead = false;

    [Header("UI")]
    [SerializeField] Image healthBar;
    [SerializeField] ParticleSystem recoverParticle;

    [Header("Animasyon")]
    [SerializeField] Animator bossAnimator;
    private bool isRecovering = false;
    private bool isPaused = false;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public bool IsRecovering()
    {
        return isRecovering;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) { return; }
        if (isRecovering) { return; }

        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }

        if (currentHealth <= maxHealth / 2 && !hasRecovered)
        {
            StartCoroutine(Recover());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator Recover()
    {
        hasRecovered = true;
        isRecovering = true;

        rb.linearVelocity = Vector2.zero;
        bossAnimator.SetTrigger("Recovery");

        yield return null; // Event hallediyor gerisini
    }


    public void OnRecoverPause()
    {
        StartCoroutine(RecoverPause());
    }

    System.Collections.IEnumerator RecoverPause()
    {
        isPaused = true;
        bossAnimator.speed = 0f;

        recoverParticle.Play();
        yield return new WaitForSeconds(5f);

        recoverParticle.Stop();
        bossAnimator.speed = 1f;
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.fillAmount = 1f;
        }

        isPaused = false;
        isRecovering = false;
    }

    void Die()
    {
        isDead = true;
        bossAnimator.speed = 1f;
        if (healthBar != null)
            healthBar.transform.parent.gameObject.SetActive(false);
        FindObjectOfType<GameSes>().BossDefeated();
        Destroy(gameObject);
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        hasRecovered = false;
        isDead = false;
        isRecovering = false;
        isPaused = false;
        bossAnimator.speed = 1f;

        if (healthBar != null)
        {
            healthBar.fillAmount = 1f;
            healthBar.transform.parent.gameObject.SetActive(true);
        }
    }
}