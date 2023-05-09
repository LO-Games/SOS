using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnButtonClick : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void SceneChange()
    {
        SceneManager.LoadScene(sceneName);
    }  
}
