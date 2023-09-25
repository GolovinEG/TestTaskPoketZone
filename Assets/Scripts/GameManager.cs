using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public RectTransform[] itemPrefabs = new RectTransform[3]; //Inventory icon to spawn. A list of ItemType to inventory icon would be preferable
    public Monster monsterPrefab;
    public int inventorySize;
    public InventorySlot inventorySlotPrefab; //The InventorySlot type is used to limit prefabs that can be inputed

    private readonly List<ItemType> inventory = new List<ItemType>();                   //Types of items in each inventory slot
    private readonly List<RectTransform> inventorySlots = new List<RectTransform>();    //Inventory slots
    private readonly List<int> inventoryCount = new List<int>();                        //The amount of item in each inventory slot
    private readonly List<RectTransform> inventoryIcons = new List<RectTransform>();    //Inventory item icons
    private RectTransform slotToDelete = null;                                          //Determine which button to reveal/hide for item deletion
    //Might be preferable to transport all the inventory management to a seperate script once the project gets more complicated

    // Start is called before the first frame update
    private void Start()
    {
        RectTransform inventoryTransform = GameObject.Find("Inventory").GetComponent<RectTransform>();
        for (int i = 0; i < inventorySize; i++) //Would be preferable to spwan inventory slots based on a maxInventory public field 
        {
            inventorySlots.Add(Instantiate<RectTransform>(inventorySlotPrefab.GetComponent<RectTransform>())); //Spawn an inventory slot
            inventorySlots[i].SetParent(inventoryTransform, false);
            inventorySlots[i].anchoredPosition -= new Vector2(80 * i, 0); //Shift the inventory slot based on its number
            inventory.Add(ItemType.Nothing); //Inventory starts empty
            inventoryCount.Add(0);
            inventoryIcons.Add(new RectTransform());
        }
        string path = Application.persistentDataPath + "/Inventory.json"; //Would be preferable to keep data in a harder to dit by user file
        if (File.Exists(path)) //If save data is found
            Load(path); //Load data
        for (int i = 0; i < 3; i++)
            SpawnMonster();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && slotToDelete != null)
            StartCoroutine("SetDelete", slotToDelete); //Hides the delete button on any click
    }

    public void AddItem(Item item, out bool isItemPickedup)
    {
        isItemPickedup = true; //Assume that the item will be picked up
        if (inventory.Contains(item.type)) //Inventory already contains this type of item
        {
            int index = inventory.FindIndex((ItemType itemType) => itemType == item.type); //Gets the inventory slot index for this item type
            inventoryCount[index]++;
            inventorySlots[index].GetChild(0).GetComponent<TMP_Text>().text = inventoryCount[index].ToString(); //Update the displayed item amount since it's now higher than 1
        }
        else if (inventory.Contains(ItemType.Nothing)) //Inventory doesn't contain this type AND there is an empty inventory slot
        {
            int index = inventory.FindIndex((ItemType itemType) => itemType == ItemType.Nothing); //Get the index of the first empty inventory slot
            inventory[index] = item.type;    //Assigns item type to the inventory slot
            inventoryCount[index] = 1;  //Text isn't update since there is only 1 item
            inventoryIcons[index] = Instantiate<RectTransform>(item.icon); //Spwans the item icon
            inventoryIcons[index].SetParent(inventorySlots[index], false);
        }
        else //If the item isn't picked up
            isItemPickedup = false; //It remains on the field
    }

    public void RevealDelete(RectTransform slotDeleteButton)
    {
        StartCoroutine(SetDelete(slotDeleteButton)); //Reveal the delete button
    }

    private IEnumerator SetDelete(RectTransform slotDeleteButton)
    {        
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0)); //Delay to avoid disabling deleting too early
        slotDeleteButton.GetComponent<Image>().enabled = slotToDelete == null; //Reveals/hides the delete button
        slotToDelete = slotToDelete != null ? null : slotDeleteButton; //Updates the targeted button
    }

    public void DeleteItem(RectTransform slot)
    {
        int index = inventorySlots.FindIndex((RectTransform rectTransform) => rectTransform == slot); //Gets the inventory slot
        if (inventory[index] != ItemType.Nothing)
        {
            inventory[index] = ItemType.Nothing;                    //Empties the inventory slot
            inventoryCount[index] = 0;                              //Removes all items from inventory slot
            slot.GetChild(0).GetComponent<TMP_Text>().text = "-";   //Updates the amount text, "-" is used insetead of leaving it blank for testing purposes
            Destroy(inventoryIcons[index].gameObject);              //Removes the item's inventory icon
        }
    }

    private void SpawnMonster()
    {
        Monster monster = Instantiate<Monster>(monsterPrefab); //Spawns a monster
        monster.transform.position = new Vector3(Random.Range(5f, 10f), Random.Range(-5f, 5f)); //Puts monster at a random location to the right of the player
    }

    public Record.InventoryData GetInventoryData()
    {
        Record.InventoryData inventoryData = new Record.InventoryData(inventorySize); //Creates SlotData for each slot
        for (int i = 0; i < inventorySize; i++)
            inventoryData.slotData[i] = new Record.SlotData(inventory[i], inventoryCount[i]); //Fills SlotData
        return inventoryData;
    }

    private void Load(string path)
    {
        Record.InventoryData inventoryData = JsonUtility.FromJson<Record.InventoryData>(File.ReadAllText(path)); //Retrives InventoryData
        for (int i = 0; i < Mathf.Min(inventorySize, inventoryData.slotData.Length); i++)
        {
            inventory[i] = inventoryData.slotData[i].itemType;
            inventoryCount[i] = inventoryData.slotData[i].itemCount;
            if (inventoryCount[i] > 1)
                inventorySlots[i].GetChild(0).GetComponent<TMP_Text>().text = inventoryCount[i].ToString(); //Updates the displayed item amount if it's higher than 1
            if (inventory[i] != ItemType.Nothing)
            {
                inventoryIcons[i] = Instantiate<RectTransform>(itemPrefabs[(int)inventory[i]]); //Spwans the item icon
                inventoryIcons[i].SetParent(inventorySlots[i], false);
            }
        }
    }
}
