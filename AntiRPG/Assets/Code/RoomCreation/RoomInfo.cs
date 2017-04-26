using System;

[AttributeUsage(AttributeTargets.Class, Inherited =false)]
public class RoomInfo : Attribute {

    //List all possible levels; 0=all
    public int[] Levels = { 0 };
    //is it a start room
    public bool StartRoom = false;
    //is it a boss room
    public bool BossRoom = false;
}
