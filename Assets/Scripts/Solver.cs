using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Theseus_and_Minotaur
{
    public class Solver : MonoBehaviour
    {
        Grid grid;
        int playerStartIndex;
        int enemyStartIndex;
        int exitIndex;

        List<int> enemyMovement;
        string solution;

        public Solver(Grid grid, int playerStartIndex, int enemyStartIndex, int exitIndex)
        {
            this.grid = grid;
            this.playerStartIndex = playerStartIndex;
            this.enemyStartIndex = enemyStartIndex;
            this.exitIndex = exitIndex;
        }
        public string GetSoultion()
        {
            return "";
        }

        // public string CheckPath(int index)
        // {
        //     if (index == enemyStartIndex)
        //     {
        //         solution.Remove(solution.Length - 1);
        //         return "";
        //     }

        //     for (int i = 0; i < 4; i++) // Left Right up and down 4 direction
        //     {
                
        //     }
        // }
    }
}