using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinSound;
    [SerializeField] int pointsForCoin = 100;
    bool isCollected = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            FindObjectOfType<GameSes>().AddToScore(pointsForCoin);
            AudioSource.PlayClipAtPoint(coinSound, Camera.main.transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

}
