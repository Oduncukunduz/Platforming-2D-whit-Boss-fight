using UnityEngine;

public class Persist : MonoBehaviour
{
    void Awake()
    {
        int numberOfGamePersists = FindObjectsOfType<Persist>().Length;
        if (numberOfGamePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void ResetGameSession()
    {
        Destroy(gameObject);
    }

}
