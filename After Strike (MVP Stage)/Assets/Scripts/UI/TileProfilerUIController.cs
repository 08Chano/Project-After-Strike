using AfterStrike.Class.Unit;
using AfterStrike.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AfterStrike.UI
{
    public class TileProfilerUIController : BaseUIFunctions
    {
        [Header("Visuals")]
        [SerializeField] private Image m_TerrainProfileImage;
        [SerializeField] private TextMeshProUGUI m_TerrainProfileName;
        [SerializeField] private Image m_UnitProfileImage;

        [Header("Containers")]
        [SerializeField] private GameObject m_CaptureMeterContainer;
        [SerializeField] private GameObject m_UnitStatsContainer;
        [SerializeField] private GameObject m_StatsContainer1;
        [SerializeField] private GameObject m_StatsContainer2;

        [Header("Unit Stats 1")]
        [SerializeField] private SlidersController m_CaptureMeter;

        [Header("Unit Stats 1")]
        [SerializeField] private SlidersController m_HealthMeter;
        [SerializeField] private SlidersController m_FuelMeter;
        [SerializeField] private SlidersController m_AmmoMeter;

        [Header("Unit Stats 2")]
        [SerializeField] private SlidersController m_MovementMeter;
        [SerializeField] private SlidersController m_AttackMeter;
        [SerializeField] private SlidersController m_DefenceMeter;

        private bool m_IsShowingPrimaryStats = true;

        public void UpdateTerrainPreviewWindow(TerrainProperty terrain)
        {
            m_TerrainProfileImage.sprite = terrain.MainSpriteRenderer.sprite;
            m_TerrainProfileName.text = terrain.name;

            if (terrain.IsCapturable)
            {
                if (terrain.CapturePower > 0)
                {
                    // Fix Here
                    RaycastHit UnitDetection;
                    int UnitMask = 1 << 8;

                    if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out UnitDetection, 10, UnitMask))
                    {
                        if (UnitDetection.collider.tag.Contains("Faction") && UnitDetection.collider.GetComponent<UnitAttributes>() != null)
                        {
                            //Change the silder's fore-colour to capturing Unit's Faction Colour
                            DisplayCaptureBar(true, terrain.CapturePower,
                                UnitDetection.collider.GetComponent<UnitAttributes>().FactionSided.GetComponent<Faction>().inGameID_Col);
                        }
                    }
                }
                else
                {
                    m_CaptureMeter.CurrentValue = 200;

                    //Changes name to match it's been taken or not
                    if (terrain.Heldby != FactionType.Neutral)
                    {
                        m_TerrainProfileName.text = $"{terrain.Heldby}'s {terrain.name}";
                        m_TerrainProfileImage.color = terrain.MainSpriteRenderer.color;
                    }
                    else
                    {
                        m_TerrainProfileName.text = $"Neutral {terrain.name}";
                        m_TerrainProfileImage.color = Color.white;
                    }
                }
            }
        }

        public void UpdateUnitPreviewWindow(Sprite unitSprite = null)
        {
            m_UnitProfileImage.gameObject.SetActive(unitSprite == null);

            if (unitSprite != null)
                m_UnitProfileImage.sprite = unitSprite;
        }

        public void DisplayCaptureBar(bool enableDisplay, int capturePercentage = 0, Color captureColour = new Color())
        {
            m_CaptureMeterContainer.SetActive(enableDisplay);

            if (!enableDisplay)
                return;

            m_CaptureMeter.CurrentValue = capturePercentage;
            m_CaptureMeter.FillColour = captureColour;
        }

        public void ToggleUnitStatsView(bool isEnabled)
        {
            m_UnitStatsContainer.SetActive(isEnabled);
        }    

        public void ToggleStatsView()
        {
            m_IsShowingPrimaryStats = !m_IsShowingPrimaryStats;

            m_StatsContainer1.SetActive(m_IsShowingPrimaryStats);
            m_StatsContainer2.SetActive(!m_IsShowingPrimaryStats);
        }

        /// <summary>
        /// Sets the Unit Stats values under Tile Preview.
        /// </summary>
        public void UpdateUnitStats(UnitAttributes previewUnit)
        {
            m_HealthMeter.CurrentValue = previewUnit.HealthPool;
            m_FuelMeter.UpdateSliderValues(previewUnit.FuelPool, previewUnit.MaxFuelPool);
            m_AmmoMeter.UpdateSliderValues(previewUnit.Ammo, previewUnit.MaxAmmo);

            m_MovementMeter.CurrentValue = previewUnit.Movement;
            m_AttackMeter.CurrentValue = previewUnit.Attack;
            m_DefenceMeter.CurrentValue = previewUnit.Defence;
        }

        private void Awake()
        {
            
        }
    }
}