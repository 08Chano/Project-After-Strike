using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AfterStrike.UI
{
    public class SlidersController : BaseUIFunctions
    {
        public string DisplayText { get => m_DisplayText; set => m_DisplayText = value; }
        public Color FillColour { get => m_FillColour; set => m_FillColour = value; }
        public int MaxValue { get => m_MaxValue; set => m_MaxValue = value; }
        public int MinValue { get => m_MinValue; set => m_MinValue = value; }

        /// <summary>
        /// Displays/Outputs the current percentage.
        /// TODO: Animate the bar filling from "Zero to Current" value.
        /// </summary>
        public int CurrentValue
        {
            get => (int)m_Slider.value; set
            {
                m_Slider.value = value;
                Sync();
            }
        }

        public bool ShowPreciseValue { get => m_ShowPreciseValue; set => m_ShowPreciseValue = value; }
        public bool ShowAsPercentage { get => m_ShowAsPercentage; set => m_ShowAsPercentage = value; }

        [SerializeField] private bool m_ShowDisplayText;
        [SerializeField] private bool m_ShowPreciseValue;
        [SerializeField] private bool m_ShowAsPercentage;
        [SerializeField] private string m_DisplayText;
        [SerializeField] private int m_MaxValue;
        [SerializeField] private int m_MinValue;
        [SerializeField] private Color m_FillColour;
        [SerializeField] private Slider m_Slider;
        [SerializeField] private Image m_FillRect;
        [SerializeField] private TextMeshProUGUI m_DisplayTextObject;
        [SerializeField] private TextMeshProUGUI m_CurrentValueObject;

        public void UpdateSliderValues(int curValue, int maxValue)
        {
            m_MaxValue = maxValue;

            if (curValue < m_Slider.minValue || curValue > maxValue)
            {
                LogError(this.GetType(), "Current Value Set is out of range");
            }
            else
            {
                m_Slider.value = curValue;
            }

            Sync();
        }

        protected override void Sync()
        {
            base.Sync();

            SyncFillColour();
            SyncDisplaytext();
            SyncValuestext();
        }

        private void SyncFillColour()
        {
            m_FillRect.color = m_FillColour;
        }

        private void SyncDisplaytext()
        {
            m_DisplayTextObject.gameObject.SetActive(m_ShowDisplayText);
            m_DisplayTextObject.text = DisplayText;
        }

        private void SyncValuestext()
        {
            m_Slider.minValue = MinValue;
            m_Slider.maxValue = MaxValue;

            m_CurrentValueObject.gameObject.SetActive(ShowPreciseValue);

            if (!ShowPreciseValue)
                return;

            if (ShowPreciseValue && !ShowAsPercentage)
                m_CurrentValueObject.text = m_Slider.value.ToString();
            else if (ShowPreciseValue && ShowAsPercentage)
                m_CurrentValueObject.text = $"{Mathf.RoundToInt((m_Slider.value / m_Slider.maxValue) * 100)}%";
        }
    }
}