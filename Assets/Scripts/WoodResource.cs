using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.bosscastlestudios.farming
{
    public class WoodResource : MonoBehaviour
    {
        public Slider woodSlider;
        public TextMeshProUGUI text;

        void Update()
        {
            var curr = PlayerResourceRepository.Instance.currentAmount;
            var max = PlayerResourceRepository.Instance.maxCapacity;

            woodSlider.value = curr / (float)max;
            text.text = $"Wood: {curr:###,###,###,##0}/{max:###,###,###,##0}";
        }
    }
}