using System;
using System.Collections;
using System.Collections.Generic;
using DarkRift.Client.Unity;
using DarkRift.Client;
using DarkRift;
using UnityEngine;


public class NetworkPlayerManager : MonoBehaviour
{
    [SerializeField]
    private UnityClient client;

    public Dictionary<ushort, PlayerObject> networkPlayers = new Dictionary<ushort, PlayerObject>();

    private void Awake()
    {
        client.MessageReceived += MessageReceived;
    }
    
    void MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            if (message.Tag == (ushort)Tags.Tag.POSITION_AND_ORIENTATION_UPDATE_TAG)
            {
                using (DarkRiftReader reader = message.GetReader())
                {
                    Debug.Log("Reader length = " + reader.Length);
                    ushort id = reader.ReadUInt16();
                    Vector3 newPosition = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Quaternion newOrientation = new Quaternion(reader.ReadSingle(), reader.ReadSingle(),
                        reader.ReadSingle(), reader.ReadSingle());

                    if (networkPlayers.ContainsKey(id))
                    {
                        networkPlayers[id].SetMovePosition(newPosition);
                        networkPlayers[id].SetDesiredOrientation(newOrientation);
                        Debug.Log("We move the player: " +id+ " To new positon: "+ newPosition);
                    }
                }
            }
        }
    }

    public void Add(ushort id, PlayerObject player)
    {
        networkPlayers.Add(id, player);
    }
    
    public void DestroyPlayer(ushort id)
    {
        PlayerObject obj = networkPlayers[id];
        Destroy(obj.gameObject);
        networkPlayers.Remove(id);
    }
}
