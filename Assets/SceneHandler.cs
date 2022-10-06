using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void SetCustomScene(string newScene)
    {
        try
        {
            SceneManager.LoadSceneAsync(newScene);
        }
        finally
        {

        }
    }
}
