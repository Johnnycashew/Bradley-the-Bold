using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerControllerTest : FSM
{

    #region Variables

    private GameManager gameManager;

    public Transform mainCam;
    public GameObject thirdPersonCam;

    //controller left joystick 
    public float movePositionX = 0.0f;
    public float movePositionY = 0.0f;

    //controller right joystick
    public float rightJoystickX = 0.0f;
    public float rightJoystickY = 0.0f;


    public float moveSpeed = 2.0f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public bool isCrouching;
    public bool cantStand = false;

    //  This mess needs to be converted to States
    //static public bool isMovingObj = false;
    //static public bool isHiding = false;
    //static public bool isClimbing = false;

    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject notebookMenu;

    public LayerMask ledgeLayerMask;
    //public LayerMask wallLayerMask;

    RaycastHit hit;

    public State currentState;

    public GameObject normalCollider;
    public GameObject crouchCollider;
    public GameObject movingObjCollider;
    public GameObject crouchTrigger;


    float lerpTime = 0.25f;
    float currentLerpTime = 0;

    Vector3 startingPos;
    Vector3 endPos;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        mainCam = GameObject.Find("Main Camera").GetComponent<Transform>();
        thirdPersonCam = GameObject.Find("ThirdPersonCamera");

        Cursor.lockState = CursorLockMode.Locked;

        currentState = new PlayerNormalState(this);
        this.SetState(currentState);
    }

    //private void OnCollisionStay(Collision other)
    //{
    //    //  Changed from other.gameObject.tag == "Respawn"
    //    if (other.gameObject.CompareTag("Wall"))
    //    {
    //        moveableObject.playerCollidingWall = true;
    //    }
    //}

    //private void OnCollisionExit(Collision other)
    //{
    //    //  Changed from other.gameObject.tag == "Respawn"
    //    if (other.gameObject.CompareTag("Wall"))
    //    {
    //        moveableObject.playerCollidingWall = false;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall") && isCrouching)
        {
            cantStand = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wall") && isCrouching)
        {
            cantStand = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        cantStand = false;        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState != GameState.Game) return;

        this.GetState().action();      
    }

    public void NormalState()
    {
        //Pause menu logic
        #region UI Input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Instantiate(pauseMenu);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            gameManager.gameState = GameState.Menu;
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Instantiate(notebookMenu);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            gameManager.gameState = GameState.Menu;
        }
        #endregion

        //see if we are moving on x or y axis
        movePositionX = Input.GetAxis("Horizontal");
        movePositionY = Input.GetAxis("Vertical");

        #region Player Movement

        Vector3 ledgeRaycastPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Debug.DrawRay(ledgeRaycastPos, this.transform.TransformDirection(Vector3.forward) * 0.75f, Color.blue);
        //Debug.Log("checking for ledge");

        if (Physics.Raycast(ledgeRaycastPos, this.transform.TransformDirection(Vector3.forward), out hit, 0.75f, ledgeLayerMask))
        {
            //Debug.Log("ledge detected");
            if (Input.GetKeyDown("space"))
            {
                currentLerpTime = 0;               

                GetComponent<Rigidbody>().useGravity = false;

                var dist = hit.collider.gameObject.GetComponentInParent<Renderer>().bounds.size.y * 1.5f;


                startingPos = this.transform.position;
                //endPos = this.transform.position + Vector3.up * Vector3.Distance(new Vector3(0, this.transform.position.y, 0), new Vector3(0, hit.collider.gameObject.transform.position.y + 0.6f, 0));
                //endPos += transform.forward * 0.6f;
                endPos = hit.collider.transform.position + Vector3.up * dist;

                currentState = new PlayerClimbingState(this);
                this.SetState(currentState);

                return;
            }
        }

        if (Input.GetKeyDown("c"))
        {
            if (cantStand)
            {
                return;
            }

            isCrouching = !isCrouching;
            moveSpeed = 0f;
        }

        if (isCrouching)
        {
            normalCollider.SetActive(false);
            crouchCollider.SetActive(true);

            moveSpeed = 1.0f;
            GetComponent<Animator>().SetBool("crouching", true);
        }
        else
        {
            normalCollider.SetActive(true);
            crouchCollider.SetActive(false);

            moveSpeed = 2.0f;
            GetComponent<Animator>().SetBool("crouching", false);
        }

        Vector3 direction = new Vector3(movePositionX, 0, movePositionY).normalized;

        if (direction.magnitude >= 0.1f)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);
            GetComponent<Animator>().SetBool("walking", true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.localEulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;
        }
        else
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);
            GetComponent<Animator>().SetBool("walking", false);
        }
        #endregion
    }
    public void HidingState()
    {
        //turn off animations
        GetComponent<Animator>().SetBool("walking", false);
        GetComponent<Animator>().SetBool("crouching", false);
    }
    public void ClimbingState()
    {
        currentLerpTime += Time.deltaTime;

        float perc = currentLerpTime / lerpTime;

        transform.position = Vector3.Slerp(startingPos, endPos, perc);

        if (currentLerpTime >= lerpTime)
        {
            currentLerpTime = lerpTime;         

            GetComponent<Rigidbody>().useGravity = true;
            //transform.position = transform.position + this.transform.TransformDirection(new Vector3(0, 0, 0.6f));

            currentState = new PlayerNormalState(this);
            this.SetState(currentState);
        }
    }
    public void DeadState()
    {
        GetComponent<Animator>().SetBool("walking", false);
        GetComponent<Animator>().SetBool("crouching", false);
    }

    public void MovingObjState()
    {
        crouchCollider.SetActive(false);
        normalCollider.SetActive(false);
        movingObjCollider.SetActive(true);

        //see if we are moving on x or y axis
        movePositionX = Input.GetAxis("Horizontal");
        movePositionY = Input.GetAxis("Vertical");

        #region Player Object Movement

        moveSpeed = 1.0f;
        isCrouching = false;
        GetComponent<Animator>().SetBool("crouching", false);
        GetComponent<Animator>().SetBool("walking", false);


        Vector3 movement = new Vector3(movePositionX, 0, movePositionY).normalized;

        if(movement.z > 0)
        {
            GetComponent<Animator>().SetBool("pushing", true);
            GetComponent<Animator>().SetBool("pulling", false);
        }
        else if(movement.z < 0)
        {
            GetComponent<Animator>().SetBool("pushing", false);
            GetComponent<Animator>().SetBool("pulling", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("pushing", false);
            GetComponent<Animator>().SetBool("pulling", false);
        }

        transform.Translate(movement * moveSpeed * Time.deltaTime);

        #endregion
    }

}
