using UnityEngine;

public class hideableObject : MonoBehaviour
{

    #region Variables

    [Header("Raycast Information")]
    RaycastHit hit;
    public float Maxdistance = 1;
    public LayerMask layerMask;

    [Header("Hiding State?")]
    public bool inHideMode = false;
    public bool release = false;
    public bool exit = false;

    [Header("Positions")]
    public Transform hidingSpot;
    public Transform exitSpot;

    public Vector3 startPoint;
    public Vector3 endPoint;

    [Header("Camera Information")]
    public GameObject player;
    public GameObject playerCamera;
    public GameObject cam;

    [Header("Interact")]
    public GameObject hidePrompt;
    public GameObject leaveHidePrompt;

    float distance;
    float moveLerpTime = 0.5f;
    float rotateLerpTime = 1f;
    float currentMoveLerpTime = 0.5f;
    float currentRotationLerpTime = 1f;

    Quaternion newRotation;

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (inHideMode)
        {
            if (player != null && startPoint != null && endPoint != null && newRotation != null)
            {


                if (currentMoveLerpTime >= moveLerpTime)
                {
                    currentMoveLerpTime = moveLerpTime;
                    currentRotationLerpTime += Time.deltaTime;
                }
                else
                {
                    currentMoveLerpTime += Time.deltaTime;
                    float perc = currentMoveLerpTime / moveLerpTime;

                    player.transform.position = Vector3.Lerp(startPoint, endPoint, perc);

                }

                if (currentRotationLerpTime >= rotateLerpTime)
                {
                    currentRotationLerpTime = rotateLerpTime;
                    playerCamera.SetActive(false);
                    //cam.SetActive(true);
                }
                else
                {
                    currentRotationLerpTime += Time.deltaTime;
                    float perc2 = currentRotationLerpTime / rotateLerpTime;

                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, newRotation, perc2);
                }
            }


        }
        else if (!inHideMode && exit)
        {
            if (player != null && startPoint != null && endPoint != null && newRotation != null)
            {
                player.GetComponent<Rigidbody>().isKinematic = false;

                playerCamera.SetActive(true);
                cam.SetActive(false);

                if (currentMoveLerpTime >= moveLerpTime)
                {
                    currentMoveLerpTime = moveLerpTime;
                    currentRotationLerpTime += Time.deltaTime;
                }
                else
                {
                    currentMoveLerpTime += Time.deltaTime;
                    float perc = currentMoveLerpTime / moveLerpTime;

                    player.transform.position = Vector3.Lerp(startPoint, endPoint, perc);
                }

                if (currentRotationLerpTime >= rotateLerpTime)
                {
                    currentRotationLerpTime = rotateLerpTime;
                    exit = false;
                    
                }
                else
                {
                    currentRotationLerpTime += Time.deltaTime;
                    float perc2 = currentRotationLerpTime / rotateLerpTime;

                    player.transform.rotation = Quaternion.Slerp(player.transform.rotation, newRotation, perc2);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            player = other.gameObject;

            Vector3 raycastPos = new Vector3(player.gameObject.transform.position.x, player.gameObject.transform.position.y, player.gameObject.transform.position.z);

            Debug.DrawRay(raycastPos, player.transform.TransformDirection(Vector3.forward) * Maxdistance, Color.green);

            if (Physics.Raycast(raycastPos, player.transform.TransformDirection(Vector3.forward), out hit, Maxdistance, layerMask))
            {
                if (!inHideMode)
                {
                    hidePrompt.SetActive(true);
                    leaveHidePrompt.SetActive(false);

                    if (Input.GetKey("e") && !release && currentMoveLerpTime >= moveLerpTime && currentRotationLerpTime >= rotateLerpTime && player.GetComponent<PlayerControllerTest>().GetState() is PlayerNormalState)
                    {
                        inHideMode = true;
                        player.GetComponent<PlayerControllerTest>().currentState = new PlayerHidingState(player.GetComponent<PlayerControllerTest>());
                        player.GetComponent<PlayerControllerTest>().SetState(player.GetComponent<PlayerControllerTest>().currentState);
                        other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        playerCamera.SetActive(false);
                        cam.SetActive(true);

                        startPoint = player.transform.position;
                        endPoint = new Vector3(hidingSpot.position.x, player.transform.position.y, hidingSpot.position.z);

                        newRotation = Quaternion.LookRotation(exitSpot.position - hidingSpot.position);
                        newRotation.x = 0.0f;
                        newRotation.z = 0.0f;

                        endPoint = hidingSpot.position;

                        currentMoveLerpTime = 0;
                        currentRotationLerpTime = 0;
                        hidePrompt.SetActive(false);
                        leaveHidePrompt.SetActive(true);
                    }
                    if (!Input.GetKey("e") && release)
                    {
                        release = false;
                    }
                }

                if (inHideMode)
                {
                    hidePrompt.SetActive(false);
                    leaveHidePrompt.SetActive(true);
                    if (!Input.GetKey("e") && !release)
                    {
                        release = true;
                    }

                    if (Input.GetKey("e") && release && currentMoveLerpTime >= moveLerpTime && currentRotationLerpTime >= rotateLerpTime && player.GetComponent<PlayerControllerTest>().GetState() is PlayerHidingState)
                    {
                        exit = true;
                        inHideMode = false;
                        player.GetComponent<PlayerControllerTest>().currentState = new PlayerNormalState(player.GetComponent<PlayerControllerTest>());
                        player.GetComponent<PlayerControllerTest>().SetState(player.GetComponent<PlayerControllerTest>().currentState);

                        currentMoveLerpTime = 0;
                        currentRotationLerpTime = 0;

                        startPoint = player.transform.position;
                        endPoint =  new Vector3(exitSpot.position.x, player.transform.position.y, exitSpot.position.z);

                        newRotation = Quaternion.LookRotation(hidingSpot.position - exitSpot.position);
                        newRotation.x = 0.0f;
                        newRotation.z = 0.0f;

                        endPoint = exitSpot.position;
                        leaveHidePrompt.SetActive(false);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerControllerTest>();

        if (player)
        {
            hidePrompt.SetActive(false);
            leaveHidePrompt.SetActive(false);
        }
    }
}
