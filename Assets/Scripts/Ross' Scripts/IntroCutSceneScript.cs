using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroCutSceneScript : MonoBehaviour
{

    private enum State
    {
        Dialogue,
        Playing,
        TransitionIn,
        TransitionOut,
    }

    private float currentAlpha = 0.0f;

    private int currentScene = 1;
    private int lastScene;

    private State state;

    public Dialogue[] dialogues;

    public Image image;

    public GameObject[] scenes;

    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        currentAlpha = image.color.a;
        lastScene = scenes.Length;
        state = State.TransitionIn;

        foreach (GameObject go in scenes)
            if (!go.name.Contains(currentScene.ToString()))
                go.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Dialogue:
                RunScene();
                break;
            case State.Playing:
                Waiting();
                break;
            case State.TransitionIn:
                FadeIn();
                break;
            case State.TransitionOut:
                FadeOut();
                break;
        }
    }

    private void Waiting()
    {
        if (GameManager.Instance.gameState == GameState.Message) return;
        state = State.TransitionOut;
    }

    private void RunScene()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogues[currentScene - 1]);
        state = State.Playing;
    }

    private void FadeOut()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);
        currentAlpha += Time.deltaTime;

        if (image.color.a >= 1.0f)
        {
            currentAlpha = 1.0f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);
            //scenes[currentScene - 1].SetActive(false);

            if (currentScene == lastScene)
            {
                SceneManager.LoadScene(sceneName);
            }

            currentScene++;
            ChangeCamera(currentScene.ToString());

            state = State.TransitionIn;
        }
    }

    private void FadeIn()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);
        currentAlpha -= Time.deltaTime;

        if (image.color.a <= 0.0f)
        {
            currentAlpha = 0.0f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);
            //currentScene++;
            //scenes[currentScene - 1].SetActive(true);

            state = State.Dialogue;
        }
    }

    private void ChangeCamera(string sceneCam)
    {
        foreach (GameObject go in scenes)
        {
            if (!go.name.Contains(sceneCam.ToString()))
                go.SetActive(false);
            else
                go.SetActive(true);
        }
    }

}
