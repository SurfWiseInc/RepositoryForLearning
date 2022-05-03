using DarkRift.Server;

using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToePlugin.Models
{
    public class PlayerModel
    {
       // public readonly IClient Client;
        public readonly string Name;
        
        public ushort ID { get; set; }
       
        //Position
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        
        //Orientation Eulers
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
      
         
        //Orientation Quaternion
        public float QW { get; set; }
        public float QX { get; set; }
        public float QY { get; set; }
        public float QZ { get; set; }

        public PlayerModel(ushort ID, float x, float y, float z)
        {
            this.ID = ID;
            X = x;
            Y = y;
            Z = z;
        }
        public PlayerModel(ushort ID, float x, float y, float z, float yaw, float pitch, float roll)
        {
            this.ID = ID;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            Roll = roll;
        }
        public PlayerModel(ushort ID, float x, float y, float z, float qw, float qx, float qy, float qz )
        {
            this.ID = ID;
            X = x;
            Y = y;
            Z = z;
            QW = qw;
            QX = qx;
            QY = qy;
        }
    }
}