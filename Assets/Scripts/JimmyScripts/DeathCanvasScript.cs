using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCanvasScript : MonoBehaviour
{
    public GameObject screen;
    public Animator anim;

    bool isActive = false;

    public CinemachineFreeLook cinemachineFreeLook;
    public List<GameObject> cinemachineVCList;

    private float camX;
    private float camY;

    private void Start()
    {
        camX = cinemachineFreeLook.m_XAxis.m_MaxSpeed;
        camY = cinemachineFreeLook.m_YAxis.m_MaxSpeed;
    }

    public void TurnOn()
    {
        screen.SetActive(true);
        anim.SetBool("active", true);
        LockCameras();
    }

    public void TurnOff()
    {
        anim.SetBool("active", false);
        screen.SetActive(true);
    }

    public void UnlockCameras()
    {
        cinemachineFreeLook.m_XAxis.m_MaxSpeed = camX;
        cinemachineFreeLook.m_YAxis.m_MaxSpeed = camY;
    }

    public void LockCameras()
    {
        cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
        cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;

        foreach(GameObject cam in cinemachineVCList)
        {
            cam.SetActive(false);
        }
    }
}
