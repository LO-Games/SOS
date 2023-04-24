using UnityEngine;
using UnityEngine.SceneManagement;

public class OnCollisionPayphone : MonoBehaviour
{
    [SerializeField] string triggeringTag;
    [SerializeField] private string nextScene;
    [SerializeField] private string currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == triggeringTag)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}