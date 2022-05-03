using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;
    
    [SerializeField] [Tooltip("Sensitivity of the mouse.")]
    private float mouseSensitivity = 10f;
    


    private float xRotation = 0f;
    private float yRotation = 0f;

    private PlayerObject playerObject;

    private void Awake()
    {
        playerObject = GetComponent<PlayerObject>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        MouseLook();
    }
    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * playerObject.speed * Time.deltaTime);
    }
    
    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        yRotation += mouseX;
        
        transform.localRotation = Quaternion.Euler(xRotation,yRotation,0f);
    }
    
}
