using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    [System.Serializable]
    public class InventoryData
    {
        public SlotData[] slots;
    };

    [System.Serializable]
    public class SlotData
    {
        public ItemType itemType = ItemType.Nothing;
        public int itemCount = 0;
    }
}
