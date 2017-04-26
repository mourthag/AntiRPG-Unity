using UnityEngine;
using System.Collections;

/// <summary>
/// A Container for rooms
/// Class contains all necessary methods to connect rooms or blocks
/// The room will be placed somewhere in the Block
/// Blocks will have a fixed size per level
/// </summary>
public class Block : MonoBehaviour {

    //Size in Tiles
    public int[] Size;
    //position of the center
    public Vector3 Position;
    //The room this Block contains
    public Room Child;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(Position, new Vector3(Size[0], 3, Size[1]));
    }
}
