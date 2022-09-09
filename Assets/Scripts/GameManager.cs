using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Theseus_and_Minotaur
{
    [RequireComponent(typeof(InputManager), typeof(UIManager))]
    public class GameManager : MonoBehaviour
    {
        [Header("Grid Related Issues")]
        [SerializeField] private GameObject gridElementPrefab;
        [Space]
        [SerializeField] private int gridWidth = 15;
        [SerializeField] private int gridHeight = 15;
        [Space]
        [SerializeField] private Color color1;
        [SerializeField] private Color color2;

        [SerializeField] private Color[] colors = new Color[6];
        private Grid grid;

        private InputManager inputManager;
        private UIManager uiManager;

        [Header("Player")]
        [SerializeField] private Transform player;
        private int playerPositionIndex;

        [Header("Enemey")]
        [SerializeField] private Transform enemy;
        private int enemyPositionIndex;
        private int enemySteps = 2;
        private int enemyStepsLeft;

        [Header("Exit")]
        [SerializeField] private Transform exit;
        private int exitPositionIndex;

        [Header("Level Data")]
        [SerializeField] private List<LevelData> levelDatas;
        private int currentLevelDataIndex = 0;


        #region Unity Deafult Functions
        private void Awake()
        {
            //Initaliaze Grid
            InitGrid();

            // Input Mangaer
            InitInputManager();

            // Init UI Manager
            InitUIManager();

        }

        private void Start()
        {
            // Initialize Level Here
            LoadLevel(currentLevelDataIndex);

            //update Grid Barrier


        }

        private void OnEnable()
        {
            inputManager.InputAction += OnInputReceived;    // Observer pattern to subscribe to the action to get the inputs when its triggered in input manager
            uiManager.UpdateBoardVisuals += UpdateBoardVisualTheme;
        }

        private void OnDisable()
        {
            inputManager.InputAction -= OnInputReceived;
            uiManager.UpdateBoardVisuals -= UpdateBoardVisualTheme;
        }
        #endregion

        #region Init Stuff

        //Create Grid
        private void InitGrid()
        {
            grid = new Grid(gridWidth, gridHeight, gridElementPrefab);
           // grid.UpdateGridVisualColor(color1, color2);
            UpdateBoardVisualTheme(0);
        }

        // Initialize Input System
        private void InitInputManager()
        {
            inputManager = GetComponent<InputManager>();
        }

        // Initialize UIManager
        private void InitUIManager()
        {
            uiManager = GetComponent<UIManager>();
        }

        #endregion

        #region Input Receiving stuff
        public void OnInputReceived(string command)
        {
            switch (command)
            {
                case "L":
                case "R":
                case "U":
                case "D":
                case "W":
                    if (!CheckBarrier(command)) // check for the barrier on that direction
                    {
                        var playerMovementSteps = PlayerMovementHelper(command);    // check if player allowed to go that direction
                        if (playerMovementSteps != -999)
                            PlayerMovement(playerMovementSteps);
                    }
                    break;
                case "N":
                case "P":
                case "A": // Again (Reload Level)
                    LoadLevelInput(command);
                    break;
            }
        }

        #endregion

        #region PlayerControl

        private void PlayerMovement(int playerMovementSteps)
        {
            playerPositionIndex += playerMovementSteps;

            // Update Player to the next position
            player.transform.position = Vector3.Lerp(player.transform.position, grid.GetGridElementPosition(playerPositionIndex), 2f);

            uiManager.UpdateComentryText(playerMovementSteps == 0 ? "WAIT" : "Player Make a step");

            // check if player win or not, reached the destination?

            if (CheckForExit())
            {
                // GREAT JOB , Player successfully escaped :)
                Debug.Log("LEVEL COMPLETED");
                uiManager.UpdateComentryText("LEVEL COMPLETED");
                inputManager.CanUseMovementInput = false;

            }

            //Update Minotaur Movement
            else
            {
                EnemeyTurn();
            }
        }

        private int PlayerMovementHelper(string command)
        {
            if (command == "W") return 0;   //wait

            int playerMovementSteps = command == "L" ? -1 : command == "R" ? 1 : command == "U" ? gridWidth : -gridWidth;

            if ((playerMovementSteps + playerPositionIndex >= gridWidth * gridHeight) || (playerMovementSteps + playerPositionIndex < 0)) return -999; // vertical border check

            else if ((playerPositionIndex % gridWidth == gridWidth - 1 && playerMovementSteps == 1) || (playerPositionIndex % gridWidth == 0 && playerMovementSteps == -1)) return -999; //Horizontal Border check

            else return playerMovementSteps;

        }

        // Check for the barrier
        private bool CheckBarrier(string command)
        {
            if (command == "L" && grid.GridElements[playerPositionIndex].Paths[0])
            {
                // Have Barrier in Left Side
                Debug.Log($"Have Barrier in Left Side");
                uiManager.UpdateComentryText("Have Barrier in Left Side");
                return true;
            }
            else if (command == "R" && grid.GridElements[playerPositionIndex].Paths[1])
            {
                // Have Barrier in Right Side
                Debug.Log($"Have Barrier in Right Side");
                uiManager.UpdateComentryText("Have Barrier in Right Side");
                return true;
            }
            else if (command == "U" && grid.GridElements[playerPositionIndex].Paths[2])
            {
                // Have Barrier in Upper Side
                Debug.Log($"Have Barrier in Upper Side");
                uiManager.UpdateComentryText("Have Barrier in Upper Side");
                return true;
            }
            else if (command == "D" && grid.GridElements[playerPositionIndex].Paths[3])
            {
                // Have Barrier in Down Side
                Debug.Log($"Have Barrier in Down Side");
                uiManager.UpdateComentryText("Have Barrier in Down Side");
                return true;
            }
            else
            {
                // No Barrier Found
                Debug.Log($"No Barrier Found");
                uiManager.UpdateComentryText("No Barrier Found");
                return false;
            }
        }
        #endregion

        #region Enemy Movement

        private void EnemeyTurn()
        {
            enemyStepsLeft = enemySteps;

            StartCoroutine(EnmeyMovement());
        }

        IEnumerator EnmeyMovement(float movementDelay = 0.25f)
        {
            while (enemyStepsLeft > 0)
            {
                yield return new WaitForSeconds(movementDelay);

                Vector2 playerPositionInGrid = grid.GetGridElementPosition(playerPositionIndex);

                Vector2 enemyPositionInGrid = grid.GetGridElementPosition(enemyPositionIndex);

                // Minotaur always start walking horizontally first
                // Minotaur make Right Side move
                // But Before that lets check if there have any barrier in right side   [Note: We Don't need to worry about enemy index cross the grid area as its following the players, so it wouldn't]
                // Paths[1] denotes to Right Side Barrier
                if (playerPositionInGrid.x > enemyPositionInGrid.x && !grid.GridElements[enemyPositionIndex].Paths[1])
                {
                    enemyPositionIndex++;
                    enemy.transform.position = grid.GetGridElementPosition(enemyPositionIndex);
                }
                // Minotaur make Left Side move
                // But Before that lets check if there have any barrier in left side
                // Paths[0] denotes to Left Side Barrier
                else if (playerPositionInGrid.x < enemyPositionInGrid.x && !grid.GridElements[enemyPositionIndex].Paths[0])
                {
                    enemyPositionIndex--;
                    enemy.transform.position = grid.GetGridElementPosition(enemyPositionIndex);
                }
                // Player and Enemey horizontal (X - Axis) position are same, so enemy will trouble in vertical axis
                else
                {
                    // Minotaur make Up Side move
                    // But Before that lets check if there have any barrier in up side  
                    // Paths[2] denotes to Up Side Barrier
                    if (playerPositionInGrid.y > enemyPositionInGrid.y && !grid.GridElements[enemyPositionIndex].Paths[2])
                    {
                        enemyPositionIndex += gridWidth;
                        enemy.transform.position = grid.GetGridElementPosition(enemyPositionIndex);
                    }
                    // Minotaur make Down Side move
                    // But Before that lets check if there have any barrier in Down side  
                    // Paths[3] denotes to Up Side Barrier 
                    else if (playerPositionInGrid.y < enemyPositionInGrid.y && !grid.GridElements[enemyPositionIndex].Paths[3])
                    {
                        enemyPositionIndex -= gridWidth;
                        enemy.transform.position = grid.GetGridElementPosition(enemyPositionIndex);
                    }
                }

                if (playerPositionIndex == enemyPositionIndex)
                {
                    // WARNING WARNING WARNING , Player going to die, as Minotaur catch him :(
                    Debug.Log("GAME OVER....");
                    uiManager.UpdateComentryText("GAME OVER !!!");
                    inputManager.CanUseMovementInput = false;
                    player.gameObject.SetActive(false);
                }

                enemyStepsLeft--;

            }
        }

        #endregion

        #region Win

        private bool CheckForExit() // Check for Level Complete
        {
            return playerPositionIndex == exitPositionIndex;
        }

        #endregion 

        #region Update/Init Level Data

        private void LoadLevelInput(string command)
        {
            if (command == "N") currentLevelDataIndex++;
            else if (command == "P") currentLevelDataIndex--;

            currentLevelDataIndex = currentLevelDataIndex < 0 ? levelDatas.Count - 1 : currentLevelDataIndex % levelDatas.Count;

            LoadLevel(currentLevelDataIndex);
        }

        private void LoadLevel(int levelNo)
        {
            if (uiManager != null) uiManager.UpdateLevelText(levelNo + 1); // 0 base index
            uiManager.UpdateComentryText("Game Started....");

            grid.Reset();
            //Activate Player Gameobject
            player.gameObject.SetActive(true);

            // Player Starting Position
            playerPositionIndex = levelDatas[currentLevelDataIndex].PlayerStartingPositionIndex;
            player.transform.position = grid.GetGridElementPosition(playerPositionIndex);

            // Enemy Starting Position
            enemyPositionIndex = levelDatas[currentLevelDataIndex].EnemyStartingPositionIndex;
            enemy.transform.position = grid.GetGridElementPosition(enemyPositionIndex);

            // Exit Point
            exitPositionIndex = levelDatas[currentLevelDataIndex].ExitPositionIndex;
            exit.transform.position = grid.GetGridElementPosition(exitPositionIndex);

            //Update Level Barrier Data 
            UpdateBarrier(levelDatas[levelNo].GridElementBarriers);

            inputManager.CanUseMovementInput = true;
        }


        // Update Level Barrier Data in the Grid
        private void UpdateBarrier(List<GridElementBarrier> nodeList)
        {
            foreach (var node in nodeList)
            {
                // load Barrier data
                for (int i = 0; i < node.Paths.Length; i++)
                {
                    grid.GridElements[node.GridElementIndex].Paths[i] = node.Paths[i];
                }

                // update Visuals
                grid.GridElements[node.GridElementIndex].UpdatePathBarrierVisuals();
            }
        }

        #endregion

        #region Delay

        IEnumerator Delay(Action action, float delayTime = 0.25f)
        {
            yield return new WaitForSeconds(delayTime);
            action?.Invoke();
        }

        #endregion
        
        #region Update Board Visuals

        public void UpdateBoardVisualTheme(int themeNo)
        {
            Debug.Log($"THEME {themeNo}");
            Debug.Log(colors == null);
            Debug.Log(colors.Length);
            grid.UpdateGridVisualColor(colors[((themeNo+1) * 2) - 2],colors[((themeNo+1) * 2) - 1]);
        }

        #endregion
        

    }
}