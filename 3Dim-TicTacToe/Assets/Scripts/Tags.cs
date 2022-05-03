using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tags
{
    public enum Tag
    {
        SET_NAME,
        SPAWN_PLAYER,
        POSITION_AND_ORIENTATION_UPDATE_TAG
    }
    public static readonly ushort SpawnPlayerTag = 0;
    public static readonly ushort PositionUpdateTag = 1;
    public static readonly ushort DespawnPlayerTag = 2;
}
