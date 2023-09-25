using System.IO;
using UnityEngine;

public class Record : MonoBehaviour
{
    [System.Serializable]
    public class InventoryData
    {
        public SlotData[] slotData;

        public InventoryData() { }

        public InventoryData(int slotCount) //Create SlotData for each slot
        {
            slotData = new SlotData[slotCount];
        }
    };

    [System.Serializable]
    public class SlotData
    {
        public ItemType itemType = ItemType.Nothing;
        public int itemCount = 0;

        public SlotData(ItemType item, int count) //Fill SlotData
        {
            itemType = item;
            itemCount = count;
        }
    }

    public void Save() //Call saving with a button for testing purposes
    {
        GameManager game = GameObject.Find("GameManager").GetComponent<GameManager>();
        InventoryData inventoryData = game.GetInventoryData();
        string path = Application.persistentDataPath + "/Inventory.json"; //Would be preferable to keep data in a harder to dit by user file
        File.WriteAllText(path, JsonUtility.ToJson(inventoryData)); //Record the save data
    }
}
