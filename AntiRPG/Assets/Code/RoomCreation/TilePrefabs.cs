using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TilePrefabs : MonoBehaviour {

    private static TilePrefabs _instance;

    [SerializeField]
    private GameObject _floorTile;
    [SerializeField]
    private GameObject _wallTile;

    public TilePrefabs(GameObject wallTile)
    {
        _wallTile = wallTile;
    }

    public static GameObject FloorTile
    {
        get
        {
            return _instance._floorTile;
        }
    }

    public static GameObject WallTile
    {
        get
        {
            return _instance._wallTile;
        }
    }

    void Awake()
    {
        _instance = this;
    }


}
