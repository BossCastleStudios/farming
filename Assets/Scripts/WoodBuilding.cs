using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.bosscastlestudios.farming
{
    public class WoodBuilding : MonoBehaviour
    {
        public int m_MaxCapacity => PlayerResourceRepository.Instance.maxCapacity;

        public int CurrentResourceAmount
        {
            get => PlayerResourceRepository.Instance.currentAmount;
            private set => PlayerResourceRepository.Instance.currentAmount = value;
        }

        void Awake()
        {
            CurrentResourceAmount = 0;
        }

        public int PlaceWood(int wood)
        {
            if (wood < 0) throw new System.Exception("Cannot add negative wood.");

            int capacityBeforeAdding = CurrentResourceAmount;
            CurrentResourceAmount = Mathf.Min(m_MaxCapacity, CurrentResourceAmount + wood);
            return CurrentResourceAmount - capacityBeforeAdding;
        }
    }
}