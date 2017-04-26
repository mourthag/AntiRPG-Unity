using UnityEngine;
using System.Collections;
using Debug = System.Diagnostics.Debug;

[RoomInfo(Levels = new int[] { 0 })]
public class TestRoom : Room
{
    public int Radius { get; set; }

    public override void AddSpecials()
    {
        //set Connection points
        Debug.Assert(Size != null, "Size must be initialized");
        LeftConnectionPoint = Parent.Position + Vector3.left*(Mathf.FloorToInt(Size[0]/2));
        RightConnectionPoint = Parent.Position + Vector3.right*(Mathf.FloorToInt(Size[0]/2));
        TopConnectionPoint = Parent.Position + Vector3.forward*(Mathf.FloorToInt(Size[1]/2));
        BottomConnectionPoint = Parent.Position + Vector3.back*(Mathf.FloorToInt(Size[1]/2));

        for (var i = 0; i < Size[0]; i++)
        {
            for (var j = 0; j < Size[1]; j++)
            {
                if (i == j || i == j + 1 || i == j - 1)
                    SetFloorLayerTile(i, j, null);
            }
        }
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
