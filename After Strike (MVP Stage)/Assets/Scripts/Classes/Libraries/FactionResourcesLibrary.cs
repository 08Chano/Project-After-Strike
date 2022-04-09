using AfterStrike.Enum;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AfterStrike.Resources
{
    [Serializable]
    public class FactionResourcesLibrary
    {
        /// <summary>
        /// Commander Sprite for Portrait.
        /// </summary>
        public Sprite CommanderSprite { get => m_CommanderSprite; set => m_CommanderSprite = value; }

        /// <summary>
        /// Faction Logo.
        /// </summary>
        public Sprite FactionLogo { get => m_FactionLogo; set => m_FactionLogo = value; }

        /// <summary>
        /// Faction Banner.
        /// </summary>
        public Sprite FactionBanner { get => m_FactionBanner; set => m_FactionBanner = value; }

        public UnitSpriteLibrary ReturnUnitSpriteLibrary(UnitType targetUnit) => m_UnitLibraryDict.ContainsKey(targetUnit) ? m_UnitLibraryDict[targetUnit] : null;

        [SerializeField]
        private Sprite m_CommanderSprite;
        [SerializeField]
        private Sprite m_FactionLogo;
        [SerializeField]
        private Sprite m_FactionBanner;

        [SerializeField]
        private Dictionary<UnitType, UnitSpriteLibrary> m_UnitLibraryDict = new Dictionary<UnitType, UnitSpriteLibrary>()
        {
            { UnitType.Infantry, new UnitSpriteLibrary() },
            { UnitType.Specialist, new UnitSpriteLibrary() },
            { UnitType.Scout, new UnitSpriteLibrary() },
            { UnitType.IFV, new UnitSpriteLibrary() },
            { UnitType.APC, new UnitSpriteLibrary() },
            { UnitType.LightTank, new UnitSpriteLibrary() },
            { UnitType.MediumTank, new UnitSpriteLibrary() },
            { UnitType.HeavyTank, new UnitSpriteLibrary() },
            { UnitType.WalkerTank, new UnitSpriteLibrary() },
            { UnitType.AntiAir, new UnitSpriteLibrary() },
            { UnitType.AntiTank, new UnitSpriteLibrary() },
            { UnitType.MobileArtiller, new UnitSpriteLibrary() },
            { UnitType.RocketArtillery, new UnitSpriteLibrary() },
            { UnitType.HeavyArtillery, new UnitSpriteLibrary() },
            { UnitType.LightFighter, new UnitSpriteLibrary() },
            { UnitType.JetFighter, new UnitSpriteLibrary() },
            { UnitType.SurfaceFighter, new UnitSpriteLibrary() },
            { UnitType.GunShip, new UnitSpriteLibrary() },
            { UnitType.Bomber, new UnitSpriteLibrary() },
            { UnitType.StealthBomber, new UnitSpriteLibrary() },
            { UnitType.Helicopter, new UnitSpriteLibrary() },
            { UnitType.Transporter, new UnitSpriteLibrary() },
            { UnitType.Lander, new UnitSpriteLibrary() },
            { UnitType.Cruiser, new UnitSpriteLibrary() },
            { UnitType.Destroyer, new UnitSpriteLibrary() },
            { UnitType.Carrier, new UnitSpriteLibrary() },
            { UnitType.Battleship, new UnitSpriteLibrary() },
            { UnitType.Submarine, new UnitSpriteLibrary() },
        };
    }
}