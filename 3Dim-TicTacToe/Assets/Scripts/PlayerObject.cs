using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerObject : MonoBehaviour
{
    //Just like AgarObject moves the Agars from last position to new position read from the server
    //In the same way PlayerObject will have to move the Player's transform in 3 dim and sphericaly interpolate quaternions
    
    [SerializeField] [Tooltip("The speed that the player will move.")]
    public float speed = 10f;
    
    [SerializeField] [Tooltip("The speed that the player will move.")]
    public float rotationSpeed = 1000f;

    
    public float xRotation = 0f;
    public float yRotation = 0f;
    
    private Vector3 movePosition;
    private Quaternion desiredOrientation;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        movePosition = transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        //if (speed != 0f)
            transform.position = Vector3.MoveTowards(transform.position, movePosition, speed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredOrientation, rotationSpeed * Time.deltaTime);
    }
    

    public void SetMovePosition(Vector3 newPosition)
    {
        movePosition = newPosition;
    }

    public void SetDesiredOrientation(Quaternion newOrientation)
    {
        desiredOrientation = newOrientation;
    }
}
