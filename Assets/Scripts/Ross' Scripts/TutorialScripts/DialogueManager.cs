using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class DialogueManager : MonoBehaviour
{

    private float heldXSpeed;
    private float heldYSpeed;

    private GameState previousState;

    private Queue<string> sentences;

    private string sentence;

    public Animator animator;

    public CinemachineFreeLook cinemachineFreeLook;

    [Range(0.0f, 0.1f)]
    public float typeSpeed;

    public GameObject messageWindow;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        heldXSpeed = cinemachineFreeLook.m_XAxis.m_MaxSpeed;
        heldYSpeed = cinemachineFreeLook.m_YAxis.m_MaxSpeed;
    }

    private void Update()
    {
        //  Do nothing unless we are in the message game state.
        if (GameManager.Instance.gameState != GameState.Message) return;

        //  If the user presses the Escape key or one of the Enter keys, display the next sentence.
        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.KeypadEnter) ||
            Input.GetKeyDown(KeyCode.Escape))
        {
            TryNewSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //  Set our message window to be active, so when we make it become visible and type it will actually do so.
        messageWindow.SetActive(true);

        //  Using "isOpen" to make message appear on screen through a fade in.
        animator.SetBool("isOpen", true);

        //  Store the previous state of the game and set it to be in messaging mode.
        previousState = GameManager.Instance.gameState;
        GameManager.Instance.gameState = GameState.Message;

        //  Lock the game camera since we want the player to focus on the message being displayed and so they can't cheese the system by looking around while a message is on screen.
        LockCamera();

        //  Set the name text to the name of the Dialogue passed.
        nameText.text = dialogue.name;

        //  Clear our sentences.
        sentences.Clear();

        //  Enqueue our sentences.
        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        var agents = FindObjectsOfType<NavMeshAgent>();

        foreach (NavMeshAgent agent in agents)
            agent.isStopped = true;

        //  Retrieve all animators in the scene.
        var animators = FindObjectsOfType<Animator>();

        //  Loop through the animators and disable them if they are not the one utilized for the message window.
        foreach (Animator anim in animators)
            if (anim != animator)
                anim.enabled = false;

        //  Display the next sentence.
        DisplayNextSentence();
    }

    private void TryNewSentence()
    {
        if (dialogueText.text != sentence)
        {
            StopAllCoroutines();
            dialogueText.text = sentence;
        }
        else
            DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        //  If there are no more sentences, run the End of Dialogue function and close this function prematurely.
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        //  Get the next sentence in line and begin the process of typing it.
        sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentenceToType)
    {
        //  Clear out the dialogue text.
        dialogueText.text = string.Empty;

        //  Loop through the sentence passed and write letters progressively at the specified speed until the entire sentence is written.
        foreach (char letter in sentenceToType.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    private void EndDialogue()
    {
        //  Using "isOpen" to make message disappear from screen through a fade out.
        animator.SetBool("isOpen", false);

        //  Set the game's state back to what it was before the dialogue ran.
        GameManager.Instance.gameState = previousState;

        //  Allow the camera to move again.
        UnlockCamera();

        var agents = FindObjectsOfType<NavMeshAgent>();

        foreach (NavMeshAgent agent in agents)
            agent.isStopped = false;

        //  Retrieve all animators in the scene.
        var animators = FindObjectsOfType<Animator>();

        //  Loop through the animators and enable them if they are not the one utilized for the message window.
        foreach (Animator anim in animators)
            if (anim != animator)
                anim.enabled = true;

        //  Set our message window to not be active, so that it cannot interfere while other menus are open or when the player requires use of the mouse in the message's area of the window.
        messageWindow.SetActive(false);
    }

    private void LockCamera()
    {
        cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
        cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
    }

    private void UnlockCamera()
    {
        cinemachineFreeLook.m_XAxis.m_MaxSpeed = heldXSpeed;
        cinemachineFreeLook.m_YAxis.m_MaxSpeed = heldYSpeed;
    }

}
