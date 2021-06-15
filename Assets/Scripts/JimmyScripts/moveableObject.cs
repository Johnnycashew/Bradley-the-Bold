using System.Collections.Generic;
using UnityEngine;

public class moveableObject : MonoBehaviour
{
    #region Variables

    [Header("Raycast Information")]
    RaycastHit hit;
    public float Maxdistance = 1;
    public LayerMask layerMask;

    [Header("Please Label")]
    public List<GameObject> sides;
    public bool release = false;
    public bool inMoveMode = false;

    [Header("UI Elements")]
    public GameObject grabPrompt;
    public GameObject releasePrompt;
    public KeyCode keyUsed;
    
    float radius = 0.1f;
    PlayerControllerTest player;

    #endregion

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            player = other.gameObject.GetComponent<PlayerControllerTest>();

            Vector3 raycastPos = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, other.gameObject.transform.position.z);

            Debug.DrawRay(raycastPos, other.transform.TransformDirection(Vector3.forward) * Maxdistance, Color.green);

            if (Physics.Raycast(raycastPos, other.transform.TransformDirection(Vector3.forward), out hit, Maxdistance, layerMask))
            {
                foreach (GameObject side in sides)
                {
                    if (hit.collider.gameObject == side)
                    {
                        if (!inMoveMode)
                        {                            
                            grabPrompt.SetActive(true);
                            releasePrompt.SetActive(false);
                            

                            if (Input.GetKey(keyUsed) && !release && player.GetState() is PlayerNormalState)
                            {                                
                                Vector3 test = new Vector3(other.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);

                                bool moveableInsideWall = false;
                                Collider[] objs = Physics.OverlapSphere(test, radius);
                                foreach (Collider obj in objs)
                                {
                                    if (obj.gameObject.tag == "Wall")
                                    {
                                        moveableInsideWall = true;
                                        break;
                                    }
                                }

                                Vector3 test2 = new Vector3(side.transform.position.x, other.gameObject.transform.position.y, side.transform.position.z);

                                bool playerInsideWall = false;
                                Collider[] colliders = Physics.OverlapSphere(test2, radius);
                                foreach (Collider obj in colliders)
                                {
                                    if (obj.gameObject.tag == "Wall")
                                    {
                                        playerInsideWall = true;
                                        break;
                                    }
                                }

                                if (playerInsideWall || moveableInsideWall)
                                {
                                    return;
                                }

                                else
                                {
                                    inMoveMode = true;
                                    player.currentState = new PlayerMovingObjState(player);
                                    player.SetState(player.currentState);

                                    other.gameObject.transform.position = new Vector3(side.transform.position.x, other.gameObject.transform.position.y, side.transform.position.z);
                                    other.gameObject.transform.LookAt(new Vector3(this.gameObject.transform.position.x, other.gameObject.transform.position.y, this.gameObject.transform.position.z));
                                }

                            }
                            if (!Input.GetKey(keyUsed) && release)
                            {
                                release = false;
                            }

                            gameObject.transform.parent = null;
                            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        }

                        if (inMoveMode)
                        {
                            grabPrompt.SetActive(false);
                            releasePrompt.SetActive(true);

                            if (!Input.GetKey(keyUsed) && !release)
                            {
                                release = true;
                            }

                            if (Input.GetKey(keyUsed) && release)
                            {
                                inMoveMode = false;

                                gameObject.transform.parent = null;
                                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                                player.currentState = new PlayerNormalState(player);
                                player.SetState(player.currentState);

                                player.movingObjCollider.SetActive(false);
                                player.GetComponent<Animator>().SetBool("pushing", false);
                                player.GetComponent<Animator>().SetBool("pulling", false);

                                return;
                            }

                            gameObject.transform.parent = other.gameObject.transform;
                            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;

                        }
                    }
                }
            }
            else
            {
                grabPrompt.SetActive(false);
                releasePrompt.SetActive(false);
            }
        }             
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            grabPrompt.SetActive(false);
            releasePrompt.SetActive(false);

            if (player.currentState is PlayerMovingObjState)
            {
                release = false;
                inMoveMode = false;
                gameObject.transform.parent = null;

                player.currentState = new PlayerNormalState(player);
                player.SetState(player.currentState);

                player.movingObjCollider.SetActive(false);
                player.GetComponent<Animator>().SetBool("pushing", false);
                player.GetComponent<Animator>().SetBool("pulling", false);

                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}


