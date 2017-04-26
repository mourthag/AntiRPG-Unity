using UnityEngine;
using System.Collections;

/// <summary>
/// The first room of a level
/// The player starts here
/// </summary>
[RoomInfo(StartRoom = true, Levels = new int[]{0})]
public class StartingRoom : Room {

    public override void AddSpecials()
    {
        //set Connection points
        LeftConnectionPoint = Parent.Position + Vector3.left * (Mathf.FloorToInt(Size[0] / 2));
        RightConnectionPoint = Parent.Position + Vector3.right * (Mathf.FloorToInt(Size[0] / 2));
        TopConnectionPoint = Parent.Position + Vector3.forward * (Mathf.FloorToInt(Size[1] / 2));
        BottomConnectionPoint = Parent.Position + Vector3.back * (Mathf.FloorToInt(Size[1] / 2));
    }
}
