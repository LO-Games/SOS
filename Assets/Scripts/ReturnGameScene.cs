using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Video;

public class ReturnGameScene : MonoBehaviour
{
    [SerializeField] private string previousScene;
    [SerializeField] private float cprSceneTime = 60f;
    private VideoPlayer MyVideoPlayer;

    private void Start()
    {
        StartCoroutine(WaitSixtySeconds());
    }

    IEnumerator WaitSixtySeconds()
    {
        yield return new WaitForSeconds(cprSceneTime);
        MyVideoPlayer = GetComponent<VideoPlayer>();
        MyVideoPlayer.Stop();
        SceneManager.LoadScene(previousScene);
    }
}