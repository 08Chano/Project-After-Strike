using System;
using UnityEngine;

namespace AfterStrike.Resources
{
    [Serializable]
    public class TerrainResourceLibrary
    {
        public Sprite Portrait { get => m_Portrait; set => m_Portrait = value; }

        public Sprite Tile { get => m_Tile; set => m_Tile = value; }

        [SerializeField]
        private Sprite m_Portrait;

        [SerializeField]
        private Sprite m_Tile;
    }
}
