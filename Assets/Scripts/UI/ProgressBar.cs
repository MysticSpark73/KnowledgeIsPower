using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _fill;

        public void SetProgress(float current, float max)
        {
            float value = Mathf.Min(Mathf.Max(current, 0), max);
            _fill.fillAmount = value / max;
        }
    }
}