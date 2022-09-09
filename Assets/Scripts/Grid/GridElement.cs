using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Theseus_and_Minotaur
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GridElement : MonoBehaviour
    {
        [SerializeField] private Transform[] barriers = new Transform[4];
        public bool[] Paths = new bool[4] { false, false, false, false }; // 0 - Left, 1 - Right, 2 - Up, 3 - Down

        private SpriteRenderer sr;

        private void OnEnable()
        {
            if (!sr) sr = GetComponent<SpriteRenderer>();
            Reset();
        }

        // Update Specific Grid Element Color
        public void UpdateGridElementColor(Color color)
        {
            sr.color = color;
        }

        public void UpdatePathBarrierVisuals()
        {
            for (int i = 0; i < Paths.Length; i++)
            {
                if (barriers[i] != null)
                {
                    barriers[i].transform.gameObject.SetActive(Paths[i]);
                }
            }
        }

        public void Reset()
        {
            Paths = new bool[4] { false, false, false, false };
            UpdatePathBarrierVisuals();
        }
    }
}
