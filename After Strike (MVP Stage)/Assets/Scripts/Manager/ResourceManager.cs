using AfterStrike.Enum;
using AfterStrike.Resources;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AfterStrike.Manager
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField]
        private List<TerrainResourceLibrary> TerrainResource = new List<TerrainResourceLibrary>();

        [SerializeField]
        private List<FactionResourcesLibrary> FactionLibrary = new List<FactionResourcesLibrary>();

        //public Sprite ReturnTerrainSprite(TerrainType terrainType)
        //{
        //    return TerrainResource.FirstOrDefault(x => x.TerrainType == terrainType).Tile;
        //}
    }
}