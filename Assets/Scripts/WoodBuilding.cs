using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.bosscastlestudios.farming
{
    public class WoodBuilding : MonoBehaviour
    {
        public int m_MaxCapacity = 0;

        public int CurrentResourceAmount { get; private set; }

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