using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PopupScript : MonoBehaviour
{
    [Header("Time")]
    public float durationDisplay;
    [Range(0.01f, 0.1f)]
    public float letterSpacing;
    private float letterCounter = 0.0f;

    [Header("Message")]
    public string message;
    private string displayedMessage = "";

    [Header("Sound")]
    private AudioSource audioSource;
    public AudioClip entranceClip;
    public AudioClip exitClip;

    private int i = 0;

    [Header("Interface")]
    public RectTransform imageTransform;
    public RectTransform parentTransform;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = entranceClip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (displayedMessage != message)
        {
            letterCounter += Time.deltaTime;

            if (letterCounter >= letterSpacing)
            {
                displayedMessage += message[i++];
                ResizeRectangles();
                text.text = displayedMessage;
                letterCounter = 0.0f;
            }

            return;
        }

        if (durationDisplay <= 0.0f)
        {
            audioSource.clip = exitClip;
            audioSource.Play();

            if (!audioSource.isPlaying)
            {
                Destroy(gameObject);
            }
            return;
        }
        durationDisplay -= Time.deltaTime;
    }
    private void ResizeRectangles()
    {
        imageTransform.sizeDelta = new Vector2((displayedMessage.Length + 1) * 8.25f + 5, imageTransform.sizeDelta.y);
        parentTransform.sizeDelta = new Vector2(displayedMessage.Length * 8.25f, imageTransform.sizeDelta.y); ;
    }

}
