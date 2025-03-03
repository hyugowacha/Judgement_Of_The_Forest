using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraCtrl : MonoBehaviour
{
    CinemachineFreeLook playerCamera;
    float scrollSpeed = 1500f;

    private void Awake()
    {
        CinemachineCore.GetInputAxis = ScrollControl;
    }
    void Start()
    {
        playerCamera = this.GetComponent<CinemachineFreeLook>();
        
    }

    public float ScrollControl(string axis)
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        playerCamera.m_Lens.FieldOfView += scrollWheel * Time.deltaTime * scrollSpeed;

        return 0;
    }


    void Update()
    {
        
    }
}
