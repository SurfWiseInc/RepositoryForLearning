using System;
using System.Collections.Generic;
using System.Linq;
using DarkRift.Server;
using DarkRift;
using TicTacToePlugin.Models;
using TicTacToePlugin.Networking;

namespace TicTacToePlugin
{
    public class TicTacToe : Plugin
    {
        public override Version Version => new Version(1, 0, 0);
        public override bool ThreadSafe => false;

        private Dictionary<IClient, PlayerModel> players = new Dictionary<IClient, PlayerModel>();
        
        public TicTacToe(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            ClientManager.ClientConnected += OnClientConnected;
            ClientManager.ClientDisconnected += OnClientDisconnected;
        }

        void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Logger.Log("Hey "+ e.Client.ID , LogType.Info);

            PlayerModel newPlayer = new PlayerModel(
                        e.Client.ID,
                        -5f,
                        0f,
                        0f);

            //Turn player data into writer so that we can send it through network
            using (DarkRiftWriter newPlayerWriter = DarkRiftWriter.Create())
            {
                newPlayerWriter.Write(newPlayer.ID);
                newPlayerWriter.Write(newPlayer.X);
                newPlayerWriter.Write(newPlayer.Y);
                newPlayerWriter.Write(newPlayer.Z);

                //Send the newplayer data over the network
                using (Message newPlayerMessage = Message.Create((ushort) Tags.Tag.SPAWN_PLAYER, newPlayerWriter))
                {
                    foreach (IClient client in ClientManager.GetAllClients().Where(x => x != e.Client))
                    {
                        client.SendMessage(newPlayerMessage, SendMode.Reliable);
                    }
                }
                //Add the player to the players dictionary
                //Note, this should be done at login level, not when the player enters arena
                players.Add(e.Client, newPlayer);

                using (DarkRiftWriter playerWriter = DarkRiftWriter.Create())
                {
                    foreach (PlayerModel player in players.Values)
                    {
                        playerWriter.Write(player.ID);
                        playerWriter.Write(player.X);
                        playerWriter.Write(player.Y);
                        playerWriter.Write(player.Z);
                       // playerWriter.Write(player.QW);
                       // playerWriter.Write(player.QX);
                       // playerWriter.Write(player.QY);
                       // playerWriter.Write(player.QZ);
                       
                    }
                    using (Message playerMessage = Message.Create((ushort)Tags.Tag.SPAWN_PLAYER,playerWriter))
                    {
                        e.Client.SendMessage(playerMessage, SendMode.Reliable);
                    }
                }
                    e.Client.MessageReceived += OnMessageReceived;
            }
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Tag)
            {
                case (ushort)Tags.Tag.SET_NAME:

                    using (Message message = e.GetMessage())
                    {
                        using (DarkRiftReader reader = message.GetReader())
                        {
                           string name =  reader.ReadString();
                           Logger.Info("Hey mr " + name);
                        }
                    }
                    break;
                case (ushort)Tags.Tag.POSITION_AND_ORIENTATION_UPDATE_TAG:

                    using (Message message = e.GetMessage())
                    {
                        using (DarkRiftReader reader = message.GetReader())
                        {
                            float newX = reader.ReadSingle();
                            float newY = reader.ReadSingle();
                            float newZ = reader.ReadSingle();
                            float newQX = reader.ReadSingle();
                            float newQY = reader.ReadSingle();
                            float newQZ = reader.ReadSingle();
                            float newQW = reader.ReadSingle();

                            PlayerModel player = players[e.Client];

                            player.X = newX;
                            player.Y = newY;
                            player.Z = newZ;
                            player.QX = newQX;
                            player.QY = newQY;
                            player.QZ = newQZ;
                            player.QW = newQW;

                            using (DarkRiftWriter writer = DarkRiftWriter.Create())
                            {
                                writer.Write(player.ID);
                                writer.Write(player.X);
                                writer.Write(player.Y);
                                writer.Write(player.Z);
                                writer.Write(player.QX);
                                writer.Write(player.QY);
                                writer.Write(player.QZ);
                                writer.Write(player.QW);
                                Console.Write(" ID = " +player.ID+" X = " +newX+" Y = " +newY+" Z = " +newZ+" QX = " +newQX+" QY = " +newQY+" QZ = " +newQZ+" QW = " +newQW+"\n");
                                //message.Serialize(writer);
                            }

                            //using(Message positionUpdateMessage = Message.Create((ushort)))
                            foreach (IClient client in ClientManager.GetAllClients().Where(x=> x != e.Client))
                            {
                                client.SendMessage(message, SendMode.Unreliable);
                            }
                        }
                    }
                    break;
                
            }
        }

        void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            Logger.Log("Bye "+ e.Client.ID , LogType.Info);
        }
        
        
    }
}