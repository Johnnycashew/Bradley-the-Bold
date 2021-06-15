using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenScript : MonoBehaviour
{

    private float timer = 0.0f;

    public float timeUntilSwap;

    public string sceneName;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeUntilSwap)
            SceneManager.LoadScene(sceneName);
    }
}
