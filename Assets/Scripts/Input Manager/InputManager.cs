using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Theseus_and_Minotaur
{

    public class InputManager : MonoBehaviour
    {
        public bool CanUseMovementInput = true;
        public Action<string> InputAction;

        private void Update()
        {
            if (CanUseMovementInput)
            {
                MovementInput();
            }

            GameFunctionalityInput();
        }

        private void MovementInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))    ///Left
            {
                InputAction?.Invoke("L");
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))   //Right
            {
                InputAction?.Invoke("R");
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))  // Up
            {
                InputAction?.Invoke("U");
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))    // Down
            {
                InputAction?.Invoke("D");
            }

            if (Input.GetKeyDown(KeyCode.W))    // Wait
            {
                InputAction?.Invoke("W");
            }
        }

        private void GameFunctionalityInput()
        {
            if (Input.GetKeyDown(KeyCode.N))    // Next Maze
            {
                InputAction?.Invoke("N");
            }

            if (Input.GetKeyDown(KeyCode.P))    // Previous Maze
            {
                InputAction?.Invoke("P");
            }

            if (Input.GetKeyDown(KeyCode.R))    // Restart Level
            {
                InputAction?.Invoke("A");
            }

            if (Input.GetKeyDown(KeyCode.H))    // Hint/ Help
            {
                InputAction?.Invoke("H");
            }
        }
    }
}