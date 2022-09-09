using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Theseus_and_Minotaur
{
    public class UIManager : MonoBehaviour
    {
        public Action<int> UpdateBoardVisuals;

        [SerializeField] private Text levelText;
        [SerializeField] private Text comentryText;

        [SerializeField] private Button[] themes;


        private void Start()
        {
            InitButtons();
        }

        public void UpdateLevelText(int levelNo)
        {
            levelText.text = $"Level No - " + levelNo;
        }

        public void UpdateComentryText(string _text)
        {
            comentryText.text = _text;
        }

        private void InitButtons()
        {
            if (themes != null)
            {
                themes[0].onClick.AddListener(() => { UpdateBoardVisuals?.Invoke(0); UpdateComentryText("Theme Color Changed"); });

                themes[1].onClick.AddListener(() => { UpdateBoardVisuals?.Invoke(1); UpdateComentryText("Theme Color Changed"); });

                themes[2].onClick.AddListener(() => { UpdateBoardVisuals?.Invoke(2); UpdateComentryText("Theme Color Changed"); }); 
            }
        }
    }
}