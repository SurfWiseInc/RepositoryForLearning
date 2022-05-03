using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift;
using UnityEngine;
using DarkRift.Client.Unity;
using DarkRift.Client;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] [Tooltip("The distance we can move before we send a position update")]
    private float moveDistance = 0.05f;
    
    public UnityClient Client { get; set; }
    

    private Vector3 lastPosition;
    private Quaternion lastOrientation;

    private void Awake()
    {
        lastPosition = transform.position;
        lastOrientation = transform.rotation;
        //Client
    }

    private void Update()
    {
        if ((Vector3.Distance(transform.position, lastPosition) > moveDistance) || Quaternion.Angle(transform.rotation, lastOrientation) > 0.01f)
        {
            using (DarkRiftWriter positionUpdateWriter = DarkRiftWriter.Create())
            {
                
                positionUpdateWriter.Write(Client.ID);
                positionUpdateWriter.Write(transform.position.x);
                positionUpdateWriter.Write(transform.position.y);
                positionUpdateWriter.Write(transform.position.z);
                positionUpdateWriter.Write(transform.rotation.x);
                positionUpdateWriter.Write(transform.rotation.y);
                positionUpdateWriter.Write(transform.rotation.z);
                positionUpdateWriter.Write(transform.rotation.w);

                using (Message positionUpdateMessage = Message.Create((ushort)Tags.Tag.POSITION_AND_ORIENTATION_UPDATE_TAG, positionUpdateWriter))
                {
                    Client.SendMessage(positionUpdateMessage, SendMode.Unreliable);
                    Debug.Log("Client = " + Client.ID);
                }
            }
            lastPosition = transform.position;
            lastOrientation = transform.rotation;
        }
       
    }
}
