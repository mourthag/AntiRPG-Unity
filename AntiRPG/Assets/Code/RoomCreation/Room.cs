using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The Parentclass for rooms
/// All necessary mehtods for creating and editing rooms are here
/// Subclasses/special rooms need to specify the addSpecials()-method
/// </summary>
public class Room : MonoBehaviour
{
    //parent
    public Block Parent;

    //Center and size in parent
    public Vector3 Position;
    public int[] Size = new int[2];

    //position for connection points on every side
    protected Vector3 LeftConnectionPoint;
    protected Vector3 TopConnectionPoint;
    protected Vector3 RightConnectionPoint;
    protected Vector3 BottomConnectionPoint;


    //Matrix for each layer... later everything is spawned from this
    private GameObject[,] _floorLayer;
    private GameObject[,] _mainLayer;

    //List of all instantiated Tiles of this room
    private readonly List<GameObject> _instantiatedTiles = new List<GameObject>();

    /// <summary>
    /// Fill the floor with the floorTile
    /// </summary>
    public void AddFloor()
    {
        //Initialize the Matrix
        _floorLayer = new GameObject[Size[0], Size[1]];

        //Fill the Matrix with the Prefabs
        for (var i = 0; i < Size[0]; i++)
        {
            for (var j = 0; j < Size[1]; j++)
            {
                _floorLayer.SetValue(TilePrefabs.FloorTile, i, j);
            }
        }
    }

    /// <summary>
    /// Set a specific Tile in the floor Layer
    /// </summary>
    /// <param name="x">X position</param>
    /// <param name="y">Y Position</param>
    /// <param name="tile">The Prefab that will be instanciated</param>    
    public void SetFloorLayerTile(int x, int y, GameObject tile)
    {
        _floorLayer.SetValue(tile, x, y);
    }

    /// <summary>
    /// Set a specific Tile in the main Layer
    /// </summary>
    /// <param name="x">X position</param>
    /// <param name="y">Y Position</param>
    /// <param name="tile">The Prefab that will be instanciated</param>
    public void SetMainLayerTile(int x, int y, GameObject tile)
    {
        _mainLayer.SetValue(tile, x, y);
    }

    /// <summary>
    /// Add the Walls to the main Layer
    /// </summary>
    public void AddWalls()
    {
        //Initialize the mainLayer
        _mainLayer = new GameObject[Size[0], Size[1]];

        //'Left' Wall
        for (var i = 0; i < Size[1]; i++)
        {
            _mainLayer.SetValue(TilePrefabs.WallTile, 0, i);
        }

        //'Top' Wall
        for (var j = 0; j < Size[0]; j++)
        {
            _mainLayer.SetValue(TilePrefabs.WallTile, j, 0);
        }

        //'Right' Wall
        for (var k = 0; k < Size[1]; k++)
        {
            _mainLayer.SetValue(TilePrefabs.WallTile, Size[0] - 1, k);
        }

        //'Bottom' Wall
        for (var l = 0; l < Size[0]; l++)
        {
            _mainLayer.SetValue(TilePrefabs.WallTile, l, Size[1] - 1);
        }
    }

    /// <summary>
    ///     Virtual method for inheriting and making custom rooms
    /// Connection points MUST be set here
    /// </summary>
    public virtual void AddSpecials()
    {

    }

    /// <summary>
    /// Create the room with instantiating the gameobjects in the matrix
    /// </summary>
    public void CreateRoom()
    {
        var truePosition = Parent.Position + Position;

        //Position relative to the center
        var widthOffset = -Size[0] / 2 + truePosition.x;
        var heightOffset = -Size[1] / 2 + truePosition.z;

        //Check if floor is initialized
        if (_floorLayer != null)
        {
            //Create Floor
            var floorLayerParent = new GameObject("FloorLayer");
            floorLayerParent.transform.SetParent(transform);

            for (var i = 0; i < Size[0]; i++)
            {
                for (var j = 0; j < Size[1]; j++)
                {
                    if (_floorLayer[i, j] == null) continue;
                    //Instantiate object and set its parent
                    var obj = (GameObject)Instantiate(_floorLayer[i, j], new Vector3(widthOffset + i, truePosition.y, heightOffset + j), Quaternion.identity);
                    obj.transform.SetParent(floorLayerParent.transform);
                    _instantiatedTiles.Add(obj);
                }
            }
        }
        else
        {
            Debug.Log("Floorlayer not set yet");
        }

        //Check if mainLayer is initialzied
        if (_mainLayer != null)
        {
            //Create mainLayer
            var mainLayerParent = new GameObject("MainLayer");
            mainLayerParent.transform.SetParent(transform);

            for (var i = 0; i < Size[0]; i++)
            {
                for (var j = 0; j < Size[1]; j++)
                {
                    if (_mainLayer[i, j] == null) continue;
                    //Instantiate the object and set its parent
                    var obj = (GameObject)Instantiate(_mainLayer[i, j], new Vector3(widthOffset + i, truePosition.y + 1, heightOffset + j), Quaternion.identity);
                    obj.transform.SetParent(mainLayerParent.transform);
                    _instantiatedTiles.Add(obj);
                }
            }
        }
        else
        {
            Debug.Log("Mainlayer not set yet");
        }
    }

    /// <summary>
    /// Enable connection on a side
    /// </summary>
    /// <param name="direction">The side where it will be connected</param>
    /// <returns>The position of the connection</returns>
    public Vector3 Connect(int direction)
    {
        if (direction == 0)
        {
            DeleteSpawnedTile(LeftConnectionPoint + Vector3.up);
            return LeftConnectionPoint;
        }
        else if (direction == 1)
        {
            DeleteSpawnedTile(TopConnectionPoint + Vector3.up);
            return TopConnectionPoint;
        }
        else if (direction == 2)
        {
            DeleteSpawnedTile(RightConnectionPoint + Vector3.up);
            return RightConnectionPoint;
        }
        else if (direction == 3)
        {
            DeleteSpawnedTile(BottomConnectionPoint + Vector3.up);
            return BottomConnectionPoint;
        }

        return Vector3.zero;
    }

    public void DeleteSpawnedTile(Vector3 position)
    {
        GameObject foundTile = null;
        foreach (var tile in _instantiatedTiles)
        {
            if (tile.transform.position == position)
            {
                foundTile = tile;
            }
        }
        if (foundTile == null) return;
        _instantiatedTiles.Remove(foundTile);
        Destroy(foundTile);
    }
}
