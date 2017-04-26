using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Debug = System.Diagnostics.Debug;

/// <summary>
/// The main class for room generation
/// works fine for now
/// TODO: connect rooms, outsource Neighboring Code, implement further levels(resetting etc.)
/// </summary>
public class RoomCreator : MonoBehaviour
{
    //default size of all blocks
    [SerializeField]
    private int RoomBlockWidth;
    [SerializeField]
    private int RoomBlockHeight;

    //number of generated rooms
    private int _roomCount;
    //level/layer increases after clearing the area boss. new world will be generated in this case
    private int _level;

    //List of current possible roomTypes
    private readonly List<Type> _currentPossibleTypes = new List<Type>();

    // Use this for initialization
    void Start()
    {
        _level = 5;
        _roomCount = 0;

        //TODO: Implement Master Scene and add this object
        //DontDestroyOnLoad(this);

        //spawn/generate the rooms
        GenerateWorld();
    }

    // Update is called once per frame
    void Update()
    {
        /*//Generate room on Key for testing
        if (Input.GetKeyDown(KeyCode.L))
        {
            //initiate room
            Room cur = Instantiate(RoomPrefab).GetComponent<Room>();

            cur.Position = Vector3.zero;
            cur.Size = new int[2]{ 20, 20};

            //Create Floors,walls and specials
            cur.addFloor();
            cur.addWalls();
            cur.addSpecials();
            
            //set the room into the world
            cur.createRoom();
        }*/
    }


    ///IN DEVELOPMENT
    ///Procedural room generation code

    /// <summary>
    /// Create the world for a level
    /// </summary>
    void GenerateWorld()
    {
        //Get one of the possible startRooms
        SetPossibleRooms(false, true);
        var random = UnityEngine.Random.Range(0, _currentPossibleTypes.Count - 1);
        var startRoomType = _currentPossibleTypes[random];

        //Create starting block & room in center
        var startBlock = CreateBlock( "StartingRoomBlock", new Vector3(0, 0, 0), new int[2] { RoomBlockWidth, RoomBlockHeight});
        var startRoom = CreateRoom(startRoomType, "StartingRoom", startBlock, Vector3.zero, new int[2]{ 15, 15});

        startBlock.Child = startRoom;

        //Chance for a new room
        float chance = 1;
        _roomCount = 1;

        //a new block will be spawned around this block
        var spawnBlock = startBlock;
        SetPossibleRooms(false, false);

        var spawnBossRoom = false;

        //while chance will result in no further spawns
        while (chance > 0 || spawnBossRoom)
        {

            //spawn room
            if (UnityEngine.Random.value < chance || spawnBossRoom)
            {
                //array for all directions that are not blocked by another block
                int[] possibleDirections;
                do
                {
                    //check current spawnBlock(0=left;1=top;2=right;3=bottom)
                    possibleDirections = CheckSurrounding(spawnBlock);
                    //if its blocked get a new block and repeat
                    if (possibleDirections.Length <= 1 || possibleDirections.Length == 0)
                        spawnBlock = BlockHandler.GetRandomBlock();

                } while (possibleDirections.Length <= 1 || possibleDirections.Length == 0);


                //decide direction(0=left;1=top;2=right;3=bottom)
                var direction = possibleDirections[UnityEngine.Random.Range(0,possibleDirections.Length)];

                //position for the new block
                //take pos of spawnBlock and add offset
                var newPos = spawnBlock.Position;
                if (direction == 0)
                {
                    newPos += new Vector3(-RoomBlockWidth, 0, 0);
                }
                else if (direction == 1)
                {
                    newPos += new Vector3(0, 0, RoomBlockHeight);
                }
                else if (direction == 2)
                {
                    newPos += new Vector3(RoomBlockWidth, 0, 0);
                }
                else if (direction == 3)
                {
                    newPos += new Vector3(0, 0, -RoomBlockHeight);
                }

                //make BossRoomSpawn
                if (spawnBossRoom)
                {
                    SetPossibleRooms(true, false);
                }

                //Get one of the possible types
                random = UnityEngine.Random.Range(0, _currentPossibleTypes.Count - 1);
                var roomType = _currentPossibleTypes[random];

                //actually spawn the block and the room itself
                var curBlock = CreateBlock("Block"+_roomCount, newPos, new int[2] { RoomBlockWidth, RoomBlockHeight });
                var curRoom = CreateRoom(roomType, "Room"+_roomCount, curBlock, Vector3.zero, new int[2] { 15, 15 });

                curBlock.Child = curRoom;
                //make sure to add it to the list of blocks
                BlockHandler.AddBlock(curBlock);

                //Fetch a block for the next iteration
                spawnBlock = BlockHandler.GetRandomBlock();
                _roomCount++;
            }
            //decrease chance per loop depending on the current levvel
            //higher level => more rooms
            chance -= (0.1f/_level);

            //in the last run of the loop make it spawn a bossroom
            if (chance <= 0 && !spawnBossRoom)
            {
                spawnBossRoom = true;
            }
            //when bossroom is already spawned
            else if (chance <= 0 && spawnBossRoom)
            {
                spawnBossRoom = false;
            }
        }
        //create the connections
        ConnectAllRooms();
    }

    /// <summary>
    /// Checks if there are Blocks around the given Blocks
    /// </summary>
    /// <param roomName="blockToCheck">The Block of which surronding will be checked</param>
    /// <returns>An Array of directions that are not blocked(0=left;1=top;2=right;3=bottom)</returns>
    private int[] CheckSurrounding(Block blockToCheck)
    {
        //list of not-blocked directions
        var directions = new List<int>();

        //left
        if(BlockHandler.FindBlockByPosition(blockToCheck.Position + new Vector3(-RoomBlockWidth, 0, 0)) == null)
        {
            directions.Add(0);
        }
        //top
        if (BlockHandler.FindBlockByPosition(blockToCheck.Position + new Vector3(0, 0, RoomBlockHeight)) == null)
        {
            directions.Add(1);
        }
        //right
        if (BlockHandler.FindBlockByPosition(blockToCheck.Position + new Vector3(RoomBlockWidth, 0, 0)) == null)
        {
            directions.Add(2);
        }
        //bottom
        if (BlockHandler.FindBlockByPosition(blockToCheck.Position + new Vector3(0, 0, -RoomBlockHeight)) == null)
        {
            directions.Add(3);
        }
        return directions.ToArray();
    }

    /// <summary>
    /// Checks if there are Blocks around the given Blocks
    /// </summary>
    /// <param roomName="blockToCheck">The Block of which surronding will be checked</param>
    /// <returns>An Array of directions that are not blocked(0=left;1=top;2=right;3=bottom)</returns>
    private int[] CheckNeighbours(Block blockToCheck)
    {
        //list of directions with neighbours
        var directions = new List<int>();

        //left
        if (BlockHandler.FindBlockByPosition(blockToCheck.Position + new Vector3(-RoomBlockWidth, 0, 0)) != null)
        {
            directions.Add(0);
        }
        //top
        if (BlockHandler.FindBlockByPosition(blockToCheck.Position + new Vector3(0, 0, RoomBlockHeight)) != null)
        {
            directions.Add(1);
        }
        //right
        if (BlockHandler.FindBlockByPosition(blockToCheck.Position + new Vector3(RoomBlockWidth, 0, 0)) != null)
        {
            directions.Add(2);
        }
        //bottom
        if (BlockHandler.FindBlockByPosition(blockToCheck.Position + new Vector3(0, 0, -RoomBlockHeight)) != null)
        {
            directions.Add(3);
        }
        return directions.ToArray();
    }

    /// <summary>
    /// Creates a block
    /// </summary>
    /// <param roomName="position">Position in world</param>
    /// <param roomName="size">Size in Tiles</param>
    /// <param name="blockName">Name of the block</param>
    /// <returns roomName="Block">The created block</returns>
    private static Block CreateBlock(string blockName, Vector3 position, int[] size)
    {
        var blockGo = new GameObject(blockName);
        var block = blockGo.AddComponent<Block>();
        block.Position = position;
        block.Size = new int[2] { size[0], size[1] };

        BlockHandler.AddBlock(block);
        return block;
    }

    /// <summary>
    /// Spawns a room of a given type to a given position
    /// </summary>
    /// <typeparam roomName="T">roomtype</typeparam>
    /// <param roomName="roomType">Type of the room</param>
    /// <param roomName="roomName">The roomName of the room</param>
    /// <param roomName="parentBlock">The block that will contain this room</param>
    /// <param roomName="position">The position of the center relative to parents position</param>
    /// <param roomName="size">The size of the room to create in Tiles</param>
    /// <returns roomName="Room">The created room</returns>
    private static Room CreateRoom<T>(T roomType, string roomName, Block parentBlock, Vector3 position, int[] size) where T :Type
    {
        if (!roomType.IsSubclassOf(typeof (Room))) return null;

        var crRoom = new GameObject(roomName).AddComponent(roomType) as Room;
        Debug.Assert(crRoom != null, "crRoom != null");
        crRoom.Position = position;
        crRoom.Size = new int[2] { size[0], size[1] };
        crRoom.Parent = parentBlock;

        crRoom.transform.SetParent(parentBlock.transform);

        //Create Floors,walls and specials
        crRoom.AddFloor();
        crRoom.AddWalls();
        crRoom.AddSpecials();

        //set the room into the world
        crRoom.CreateRoom();

        return crRoom;
    }

    /// <summary>
    /// Connect all rooms, that were created for this level
    /// </summary>
    void ConnectAllRooms()
    {
        //List of all Connections, that were set already
        var connectedBlocks = new List<Block[]>();

        //Connect every Block to its neighbours
        foreach(var curBlock in BlockHandler.GetActiveBlocks())
        {
            //target of the connection
            Block targetBlock = null;
            //Check all directions for neighbours
            foreach(var i in CheckNeighbours(curBlock))
            {
                if (i == 0)
                {
                    targetBlock =
                        BlockHandler.FindBlockByPosition(curBlock.Position - new Vector3(-RoomBlockWidth, 0, 0));
                }
                else if (i == 1)
                {
                    targetBlock =
                        BlockHandler.FindBlockByPosition(curBlock.Position - new Vector3(0, 0, RoomBlockHeight));
                }
                else if (i == 2)
                {
                    targetBlock = BlockHandler.FindBlockByPosition(curBlock.Position - new Vector3(RoomBlockWidth, 0, 0));
                }
                else if (i == 3)
                {
                    targetBlock =
                        BlockHandler.FindBlockByPosition(curBlock.Position - new Vector3(0, 0, -RoomBlockHeight));
                }
                //if these two blocks aren't already connected...
                var case1 = connectedBlocks.Contains(new Block[2] { curBlock, targetBlock });
                var case2 = connectedBlocks.Contains(new Block[2] { targetBlock, curBlock });
                if (!targetBlock || (case1 || case2)) continue;
                //connect them and add them to the list
                connectedBlocks.Add(new Block[2] { curBlock, targetBlock });
                ConnectTwoRooms(curBlock.Child, targetBlock.Child, i);
            }

        }
    }

    /// <summary>
    /// Connect two rooms with each other
    /// </summary>
    /// <param roomName="startRoom">The first room</param>
    /// <param roomName="endRoom">The second room</param>
    /// <param roomName="direction">The direction of the connection(0=left,1=top,...)</param>
    void ConnectTwoRooms(Room startRoom, Room endRoom, int direction)
    {
        int i;
        //get the opposite direction
        switch(direction)
        {
            case 0:
                i = 2;
                break;
            case 1:
                i = 3;
                break;
            case 2:
                i = 0;
                break;
            case 3:
                i = 1;
                break;
            default:
                i = 1;
                break;
        }
        //Get the position and delete connection Tiles
        var startPosition = startRoom.Connect(i);
        var endPosition = endRoom.Connect(direction);

        //create the Connection
        var connection = new GameObject(startRoom.name + " - " + endRoom.name).AddComponent<Connection>();

        //initialize it
        connection.SetStartingPoint(startPosition);
        connection.SetEndingPoint(endPosition);

        //build the connection
        connection.Connect(2, (i % 2) == 0);
    }

    /// <summary>
    /// Fill the list with all possible room types
    /// </summary>
    /// <param roomName="isBossRoom">Determines wether only BossRooms are allowed</param>
    /// <param roomName="isStartRoom">Determines wether only StartRooms are allowed</param>
    private void SetPossibleRooms(bool isBossRoom, bool isStartRoom)
    {
        //Get all roomtypes
        var roomTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                         from type in assembly.GetTypes()
                         where typeof(Room).IsAssignableFrom(type)
                         select type).ToList();

        //reset the list
        _currentPossibleTypes.Clear();

        //check all types
        foreach(var roomType in roomTypes)
        {
            //Get their attributes
            var attributes = roomType.GetCustomAttributes(typeof(RoomInfo), false);
            if(attributes.Length == 0)
            {
                continue;
            }
            var roomInfo = attributes[0] as RoomInfo;
            //check if their attributes fit to the requirements
            Debug.Assert(roomInfo != null, "No Room info assigned");
            if (roomInfo.BossRoom == isBossRoom && roomInfo.StartRoom == isStartRoom && (roomInfo.Levels.Contains(_level) || roomInfo.Levels.Contains(0)))
            {
                _currentPossibleTypes.Add(roomType);
            }
        }
    }
}
