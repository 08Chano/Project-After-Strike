using AfterStrike.Class.Terrain;
using AfterStrike.Enum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AfterStrike.Manager
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField]
        private List<TerrainResource> TerrainResource = new List<TerrainResource>();

        public Sprite ReturnTerrainSprite(TerrainType terrainType)
        {
            return TerrainResource.FirstOrDefault(x => x.TerrainType == terrainType).TerrainSprite;
        }
    }
}