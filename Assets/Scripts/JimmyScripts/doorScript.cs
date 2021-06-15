using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{

    #region Variables

    public LayerMask openOutLayerMask;
    public LayerMask openInLayerMask;
    public GameObject door;
    public GameObject player;
    public GameObject enemy;
    public GameObject currentTrigger;
    RaycastHit hit;
    public bool open = false;

    #endregion

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.DrawRay(player.transform.position, player.transform.TransformDirection(Vector3.forward) * .5f, Color.red);

            if (Physics.Raycast(player.transform.position, player.transform.TransformDirection(Vector3.forward), out hit, .5f, openOutLayerMask))
            {
                if (!open)
                {
                    Debug.Log("found openOut door");
                    currentTrigger = hit.collider.gameObject;
                    door.GetComponent<Animator>().SetBool("openOut", true);
                    open = true;

                }
            }
            if (Physics.Raycast(player.transform.position, player.transform.TransformDirection(Vector3.forward), out hit, .5f, openInLayerMask))
            {
                if (!open)
                {
                    Debug.Log("found openIn door");
                    currentTrigger = hit.collider.gameObject;
                    door.GetComponent<Animator>().SetBool("openIn", true);
                    open = true;

                }
            }
        }

        if (other.gameObject.name == "Enem")
        {
            Debug.DrawRay(enemy.transform.position, enemy.transform.TransformDirection(Vector3.forward) * .5f, Color.red);

            if (Physics.Raycast(enemy.transform.position, enemy.transform.TransformDirection(Vector3.forward), out hit, .5f, openOutLayerMask))
            {
                if (!open)
                {
                    Debug.Log("found openOut door");
                    currentTrigger = hit.collider.gameObject;
                    door.GetComponent<Animator>().SetBool("openOut", true);
                    open = true;

                }
            }
            if (Physics.Raycast(enemy.transform.position, enemy.transform.TransformDirection(Vector3.forward), out hit, .5f, openInLayerMask))
            {
                if (!open)
                {
                    Debug.Log("found openIn door");
                    currentTrigger = hit.collider.gameObject;
                    door.GetComponent<Animator>().SetBool("openIn", true);
                    open = true;

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.name == "Player")
        {
            open = false;

            if (currentTrigger != null)
            {

                if (door.GetComponent<Animator>().GetBool("openOut") == true)
                {
                    Debug.Log("closed openOut door");

                    door.GetComponent<Animator>().SetBool("openOut", false);
                }
                else if (door.GetComponent<Animator>().GetBool("openIn") == true)
                {
                    Debug.Log("closed openIn door");

                    door.GetComponent<Animator>().SetBool("openIn", false);
                }
            }
        }

        if (other.gameObject.name == "Enem")
        {
            open = false;

            if (currentTrigger != null)
            {

                if (door.GetComponent<Animator>().GetBool("openOut") == true)
                {
                    Debug.Log("closed openOut door");

                    door.GetComponent<Animator>().SetBool("openOut", false);
                }
                else if (door.GetComponent<Animator>().GetBool("openIn") == true)
                {
                    Debug.Log("closed openIn door");

                    door.GetComponent<Animator>().SetBool("openIn", false);
                }
            }
        }
    }
}
