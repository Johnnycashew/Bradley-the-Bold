using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable][RequireComponent(typeof(AudioSource), typeof(BoxCollider))]
public class Item : MonoBehaviour
{

    #region Variables

    private GameManager gameManager;
    private PlayerControllerTest player;
    private bool canInteract = false;

    [Header("UI Elements")]
    public Transform canvas;
    public GameObject interactableUIElement;

    [Header("Search UI")]
    public GameObject searchBar;
    public Image searchMeter;

    [Header("Item")]
    public ItemObject item;

    [Header("Timer")]
    [Range(0.0f, 5.0f)]
    public float timeToSearch = 2.5f;
    private float timeCounter = 0.0f;
    private KeyCode keyUsed;

    [Header("Sounds")]
    public AudioClip pickupSound;
    public AudioClip searchSound;
    private AudioSource audioSource;

    [Header("Dialogue Popup")]
    public Dialogue itemFound;
    public Dialogue noItemFound;

    #endregion

    private void Start()
    {
        //  Set our Game Manager Instance
        gameManager = GameManager.Instance;

        keyUsed = KeyCode.E;

        //  Get our Audio Source and set it's Audio Clip to the search sound
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = searchSound;
    }

    private void Update()
    {
        if (gameManager.gameState != GameState.Game) return;

        //  If we are able to interact and this is the frame we press the interaction key
        if (canInteract && Input.GetKeyDown(keyUsed))
        {
            //  Make our search bar active and begin playing the searching sound
            searchBar.SetActive(true);
            audioSource.Play();

            //  ******************************************************  //
            //  Jimmy, add the logic you need to here for telling the
            //  player controller what it needs to do. Remove ability
            //  for player to use other controls, point player toward
            //  this game object etc. The variable for your script is
            //  named player.
            //  ******************************************************  //

        }

        //  If we are able to interact and this is the frame we release the interaction key
        else if (canInteract && Input.GetKeyUp(keyUsed))
        {
            //  Disable the search bar and set the fill amount to 0
            searchBar.SetActive(false);
            searchMeter.fillAmount = 0.0f;

            //  Reset the timer and stop playing audio
            timeCounter = 0.0f;
            audioSource.Stop();

            //  ******************************************************  //
            //  Jimmy, add the logic you need to here for telling the
            //  player controller what it needs to do. Return ability
            //  for player to use other controls. The reference still
            //  is player for your script.
            //  ******************************************************  //

        }

        //  If the search bar is active
        //  Prevents holding down of key then walking into collider bug
        if (searchBar.activeSelf)
        {
            //  If we are holding the interaction key down
            if (canInteract && Input.GetKey(keyUsed))
            {
                //  If we have completed searching
                if (timeCounter >= timeToSearch)
                {
                    //  Add the item to our inventory if there is an item to add or if we don't have it already
                    if (item != null && !gameManager.inventory.Contains(item))
                    {
                        gameManager.inventory.Add(item);
                        FindObjectOfType<DialogueManager>().StartDialogue(itemFound);
                    }
                    else
                    {
                        FindObjectOfType<DialogueManager>().StartDialogue(noItemFound);
                    }

                    //  Disable our Canvas UI Elements and reset our fill value on the search bar
                    searchBar.SetActive(false);
                    searchMeter.fillAmount = 0.0f;
                    timeCounter = 0.0f;

                    //  Stop playing the currently playing audio and play a one shot of the pickup sound
                    audioSource.Stop();
                    audioSource.PlayOneShot(pickupSound);
                    
                    //  ******************************************************  //
                    //  Jimmy, add the logic you need to here for telling the
                    //  player controller what it needs to do. Return ability
                    //  for player to use other controls, maybe play a pickup
                    //  animation or something. The reference still is player
                    //  for your script.
                    //  ******************************************************  //

                }

                //  Increase our timer and update our search meter's fill
                timeCounter += Time.deltaTime;
                searchMeter.fillAmount = timeCounter / timeToSearch;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //  Attempts to retrieve Player component
        var player = other.GetComponent<PlayerControllerTest>();

        //  If the component is found
        if (player)
        {
            //  The player is within interactable range
            canInteract = true;

            //  Set our player equal to the one we found
            this.player = player;

            //  Activate our Canvas UI Element
            interactableUIElement.SetActive(canInteract);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //  Attempts to retrieve Player component
        var player = other.GetComponent<PlayerControllerTest>();

        //  If the component is found
        if (player)
        {
            //  The player has left the interactable range
            canInteract = false;

            //  Reset our player
            this.player = null;

            //  Disable our Canvas UI Element
            interactableUIElement.SetActive(canInteract);
            searchBar.SetActive(canInteract);
            searchMeter.fillAmount = 0.0f;
            timeCounter = 0.0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //  Attempts to retrieve Player component
        var player = other.GetComponent<PlayerControllerTest>();

        //  If the component is found
        if (player)
        {
            //  The player is within interactable range
            canInteract = true;

            //  Set our player equal to the one we found
            this.player = player;

            //  Activate our Canvas UI Element
            interactableUIElement.SetActive(canInteract);
        }
    }
}
