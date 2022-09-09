using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Theseus_and_Minotaur
{
    public class Grid : MonoBehaviour
    {
        int heigth, width;

        [HideInInspector] public List<GridElement> GridElements = new List<GridElement>();


        //Constructor to Create the grid
        // One time call in entire game cycle
        public Grid(int width, int heigth, GameObject gameElementPrefab)
        {
            this.heigth = heigth;
            this.width = width;

            for (int i = 0; i < heigth; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var go = Instantiate(gameElementPrefab);    // Heavy call :(

                    Vector3 gridElementPosition = new Vector3(j, i);
                    go.transform.position = gridElementPosition;

                    GridElement gridElement = go.GetComponent<GridElement>();

                    if (gridElement)
                        GridElements.Add(gridElement);
                    else
                    {
                        Debug.LogError("GridElement Prefab not have GeidElement Component....");
                        GridElements.Add(go.AddComponent<GridElement>());
                    }
                }
            }

            // Update Camera Position
            Camera.main.transform.position = new Vector3((float)width / 2 - 0.5f, (float)heigth / 2 - 0.5f, -10f);
        }


        // Get the GridElement position
        public Vector3 GetGridElementPosition(int index)
        {
            return new Vector3(index % width, index / width);
        }


        // Updating Grid Color
        public void UpdateGridVisualColor(Color color1, Color color2)
        {
            for (int i = 0; i < GridElements.Count; i++)
            {
                GridElements[i].UpdateGridElementColor(i % 2 == 0 ? color1 : color2);
            }
        }

        public void Reset()
        {
            foreach(var gridElement in GridElements)
            {
                gridElement.Reset();
            }
        }
    }
}