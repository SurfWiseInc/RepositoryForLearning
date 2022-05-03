using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
//using Unity.Mathematics;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] [Tooltip("The DarkRift client to communicate on.")]
    private UnityClient client;

    [SerializeField] [Tooltip("The controllable player prefab.")]
    private GameObject controllablePlayer;

    [SerializeField] [Tooltip("The network controllable player prefab.")]
    private GameObject networkPlayer;

    [SerializeField] [Tooltip("The network player manager.")]
    private NetworkPlayerManager networkPlayerManager;
    // Start is called before the first frame update
    void Awake()
    {
        if (client == null)
        {
            Debug.LogError("Client unassigned in PlayerSpawner");
        }

        if (controllablePlayer == null)
        {
            Debug.LogError("Controllable Prefab unassigned in PlayerSpawner.");
        }

        if (networkPlayer == null)
        {
            Debug.LogError("Network Prefab unassigned in PlayerSpawner");
            
            Application.Quit();
        }

        client.MessageReceived += MessageRecieved;
    }

    void MessageRecieved(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            if (message.Tag == (ushort)Tags.Tag.SPAWN_PLAYER)
                SpawnPlayer(sender,e);
            //else if (message.Tag == Tags.DespawnPlayerTag)
             //   DespawnPlayer(sender,e);
        }
    }

    void SpawnPlayer(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        using (DarkRiftReader reader = message.GetReader())
        {
            if (message.Tag == (ushort) Tags.Tag.SPAWN_PLAYER)
            {
                while (reader.Position < reader.Length)
                {
                    Debug.Log("Reader Length = " + reader.Length);
                    ushort id = reader.ReadUInt16();
                    Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    GameObject obj;
                    if (id == client.ID)
                    {
                        obj = Instantiate(controllablePlayer, position, Quaternion.identity);

                        PlayerModel player = obj.GetComponent<PlayerModel>();
                        player.Client = client;
                    }
                    else
                    {
                        obj = Instantiate(networkPlayer, position, Quaternion.identity);
                    }

                    PlayerObject playerObj = obj.GetComponent<PlayerObject>();
                    
                    networkPlayerManager.Add(id,playerObj);
                }
            }
        }
    }

    void DespawnPlayer(object sender, MessageReceivedEventArgs e)
    {
        using(Message message = e.GetMessage())
        using (DarkRiftReader reader = message.GetReader())
            networkPlayerManager.DestroyPlayer(reader.ReadUInt16());
    }
}
