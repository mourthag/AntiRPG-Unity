using UnityEngine;
using System.Collections;
/// <summary>
/// A class to establish a connection between two rooms
/// </summary>
public class Connection : MonoBehaviour {

    //positions of the start and end of the connection
    private Vector3 _startingPoint;
    private Vector3 _endPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Sets the starting point
    /// </summary>
    /// <param name="point">The point that will be set</param>
    public void SetStartingPoint(Vector3 point)
    {
        _startingPoint = point;
    }

    /// <summary>
    /// Sets the ending point
    /// </summary>
    /// <param name="point">The point that will be set</param>
    public void SetEndingPoint(Vector3 point)
    {
        _endPoint = point;
    }

    /// <summary>
    /// Connect the ending point with the start point
    /// </summary>
    /// <param name="width">Width of the connection (2 is perfect)</param>
    /// <param name="horizontal">Determines wether the connection is horizontal or vertical</param>
    public void Connect(int width, bool horizontal)
    {
        //Check positions are initialized
        if(_startingPoint == Vector3.zero || _startingPoint == _endPoint)
        {
            return;
        }

        //Distances
        var xDistance = Mathf.FloorToInt(_endPoint.x - _startingPoint.x);
        var zDistance = Mathf.FloorToInt(_endPoint.z - _startingPoint.z);
        
        //check direction
        if (horizontal)
        {
            //make it simple
            if (zDistance != 0) return;
            CreateHorizontalLine(_startingPoint, xDistance, width);
            return;
        }
        //vertical
        else
        {
            //make a simple line
            if (xDistance != 0) return;
            CreateVecrticalLine(_startingPoint, zDistance, width);
            return;
        }

    }

    /// <summary>
    /// Creates a horizontal Connection
    /// </summary>
    /// <param name="start">The position this connection starts</param>
    /// <param name="xDistance">Length of the Connection</param>
    /// <param name="width">Width of the Connection(2 is optimal)</param>
    void CreateHorizontalLine(Vector3 start, int xDistance, int width)
    {
        if(xDistance > 0)
        {
            //step by step in xDirection
            for(var i = 0; i <= xDistance; i++)
            {
                //create Floor
                for(var j = -Mathf.FloorToInt(width / 2)-1; j <= width; j++)
                {
                    var obj = (GameObject)Instantiate(TilePrefabs.FloorTile, start + new Vector3(i, 0, j), Quaternion.identity);
                    obj.transform.SetParent(transform);
                }
                //create walls
                var wall1 = (GameObject)Instantiate(TilePrefabs.WallTile, start + new Vector3(i, 1, -Mathf.FloorToInt(width / 2) - 1), Quaternion.identity);
                var wall2 = (GameObject)Instantiate(TilePrefabs.WallTile, start + new Vector3(i, 1, Mathf.FloorToInt(width / 2) + 1), Quaternion.identity);

                wall1.transform.SetParent(transform);
                wall2.transform.SetParent(transform);
            }
        }
        else if(xDistance < 0)
        {
            for (var i = 0; i >= xDistance; i--)
            {
                //create Floor
                for (var j = -Mathf.FloorToInt(width / 2) - 1; j <= width; j++)
                {
                    var obj = (GameObject)Instantiate(TilePrefabs.FloorTile, start + new Vector3(i, 0, j), Quaternion.identity);
                    obj.transform.SetParent(transform);
                }
                //create walls
                var wall1 = (GameObject)Instantiate(TilePrefabs.WallTile, start + new Vector3(i, 1, -Mathf.FloorToInt(width / 2) - 1), Quaternion.identity);
                var wall2 = (GameObject)Instantiate(TilePrefabs.WallTile, start + new Vector3(i, 1, Mathf.FloorToInt(width / 2) + 1), Quaternion.identity);

                wall1.transform.SetParent(transform);
                wall2.transform.SetParent(transform);
            }
        }
    }

    /// <summary>
    /// Creates a vertical connection
    /// </summary>
    /// <param name="start">The position this connection starts</param>
    /// <param name="zDistance">Length of the Connection</param>
    /// <param name="width">Width of the Connection(2 is optimal)</param>
    void CreateVecrticalLine(Vector3 start, int zDistance, int width)
    {
        if(zDistance > 0)
        {
            //step by step in zDirection
            for (var i = 0; i <= zDistance; i++)
            {
                //create floor
                for (var j = -Mathf.FloorToInt(width / 2) - 1; j <= width ; j++)
                {
                    var obj = (GameObject)Instantiate(TilePrefabs.FloorTile, start + new Vector3(j, 0, i), Quaternion.identity);
                    obj.transform.SetParent(transform);
                }
                //create walls
                var wall1 = (GameObject)Instantiate(TilePrefabs.WallTile, start + new Vector3(-Mathf.FloorToInt(width / 2) - 1, 1, i), Quaternion.identity);
                var wall2 = (GameObject)Instantiate(TilePrefabs.WallTile, start + new Vector3(Mathf.FloorToInt(width / 2) + 1, 1, i), Quaternion.identity);

                wall1.transform.SetParent(transform);
                wall2.transform.SetParent(transform);
            }
        }
        else if(zDistance < 0)
        {
            //step by step in zDirection
            for (var i = 0; i >= zDistance; i--)
            {
                //create floor
                for (var j = -Mathf.FloorToInt(width / 2) - 1; j <= width; j++)
                {
                    var obj = (GameObject)Instantiate(TilePrefabs.FloorTile, start + new Vector3(j, 0, i), Quaternion.identity);
                    obj.transform.SetParent(transform);
                }
                //create walls
                var wall1 = (GameObject)Instantiate(TilePrefabs.WallTile, start + new Vector3(-Mathf.FloorToInt(width / 2) - 1, 1, i), Quaternion.identity);
                var wall2 = (GameObject)Instantiate(TilePrefabs.WallTile, start + new Vector3(Mathf.FloorToInt(width / 2) + 1, 1, i), Quaternion.identity);

                wall1.transform.SetParent(transform);
                wall2.transform.SetParent(transform);
            }
        }
    }
}
