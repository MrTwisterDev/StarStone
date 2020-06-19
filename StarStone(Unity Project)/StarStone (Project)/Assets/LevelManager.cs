using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadNextLevel()
    {
        int levelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(levelToLoad);
    }

}
