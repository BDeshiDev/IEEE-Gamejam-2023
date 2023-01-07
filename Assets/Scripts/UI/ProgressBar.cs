using Combat;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] Slider slider;
        public Slider Slider => slider;

        private ResourceComponent target;
        [SerializeField] private RectTransform rectTransform;
        public RectTransform RectTransform => rectTransform;
        
        
     

        private void OnDestroy()
        {
            cleanup();
        }

        public virtual void updateFromRatio(ResourceComponent resourceComponent)
        {
            slider.value = resourceComponent.Ratio;
        }
        
        public virtual void updateFromRatio(float ratio)
        {
            slider.value = ratio;
        }
        
        
        public void init(ResourceComponent r)
        {
            if(target != null){
                target.RatioChanged -= updateFromRatio;
            }

            target = r;
            target.RatioChanged += updateFromRatio;
            updateFromRatio(r);
        }

   

        public void cleanup() {
            if (target != null)
            {
                target.RatioChanged -= updateFromRatio;
            }
        }
    }
}
