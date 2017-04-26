using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A static class to handle active blocks
/// </summary>
public static class BlockHandler
{
    //A List of all spawned Blocks
    private static readonly List<Block> ActiveBlocks = new List<Block>();

    public static List<Block> GetActiveBlocks()
    {
        return ActiveBlocks;
    }

    /// <summary>
    /// Adds a Block to the list of spawned blocks
    /// </summary>
    /// <param name="blockToAdd">The Block that will be added</param>
    public static void AddBlock(Block blockToAdd)
    {
        ActiveBlocks.Add(blockToAdd);
    }

    /// <summary>
    /// Get a random Block from the list 
    /// </summary>
    /// <returns>A Block from the list of spawned Blocks</returns>
    public static Block GetRandomBlock()
    {
        var random = Random.Range(0, ActiveBlocks.Count);
        return ActiveBlocks[random];
    }

    /// <summary>
    /// Removes a Block from the list of active Blocks
    /// </summary>
    /// <param name="blockToRemove">The Block that will be removed</param>
    public static void RemoveBlock(Block blockToRemove)
    {
        if (ActiveBlocks.Contains(blockToRemove))
            ActiveBlocks.Remove(blockToRemove);
    }

    /// <summary>
    /// Clears the list of active Blocks
    /// </summary>
    public static void RemoveAllBlocks()
    {
        ActiveBlocks.Clear();
    }

    /// <summary>
    /// Checks if there is a Block on the given position
    /// </summary>
    /// <param name="position">The position that may lay inside of a block</param>
    /// <returns>The Block that contains this position</returns>
    public static Block FindBlockByPosition(Vector3 position)
    {
        foreach(Block cur in ActiveBlocks)
        {
            var existingBlockRekt = new Rect(cur.Position.x, cur.Position.z, cur.Size[0], cur.Size[1]);
            var flatPos = new Vector2(position.x, position.z);
            if (existingBlockRekt.Contains(flatPos))
            {
                return cur;
            }

        }

        return null;
    }
}
