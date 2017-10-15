using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ComplexitySlider : MonoBehaviour
    {

        [SerializeField] private Text sliderText;

        public int value;
        
        private Slider mSlider;
        
        void Start()
        {
            mSlider = GetComponent<Slider>();
            
            sliderText.text = mSlider.value.ToString();
        }

        public void OnValue()
        {
            value = (int) mSlider.value;
            sliderText.text = mSlider.value.ToString();
        }
    }
}