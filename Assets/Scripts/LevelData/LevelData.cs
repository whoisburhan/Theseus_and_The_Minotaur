using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level No - 1", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    public int PlayerStartingPositionIndex;
    public int EnemyStartingPositionIndex;
    public int ExitPositionIndex;

    public List<GridElementBarrier> GridElementBarriers;

}

[Serializable]
public class GridElementBarrier
{
    public int GridElementIndex;
    public bool[] Paths = new bool[4];
}