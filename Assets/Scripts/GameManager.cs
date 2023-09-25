using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public RectTransform[] itemPrefabs = new RectTransform[3]; //Inventory icon to spawn
    public Monster monsterPrefab;

    private List<ItemType> inventory = new List<ItemType>();                //Types of items in each inventory slot
    private List<RectTransform> inventorySlots = new List<RectTransform>(); //Inventory slots
    private int[] inventoryCount = { 0, 0, 0 };                             //The amount of item in each inventory slot
    private RectTransform[] inventoryIcons = new RectTransform[3];          //Inventory item icons
    private RectTransform slotToDelete = null;                              //Determine which button to reveal/hide for item deletion
    // Start is called before the first frame update
    void Start()
    {
        Transform inventoryTransform = GameObject.Find("Inventory").transform;
        for (int i = 0; i < inventoryTransform.childCount; i++) //Would be preferable to spwan inventory slots based on a maxInventory public field 
        {
            inventorySlots.Add((RectTransform)inventoryTransform.GetChild(i)); //Finds the inventory slot objects
            inventory.Add(ItemType.Nothing); //Inventory starts empty
        }
        string path = Application.persistentDataPath + "/Inventory.json"; //Would be preferable to keep data in a harder to dit by user file
        if (File.Exists(path))
        {
            Record.InventoryData inventoryData = JsonUtility.FromJson<Record.InventoryData>(File.ReadAllText(path));
            for (int i = 0; i < inventoryTransform.childCount; i++)
            {
                inventory[i] = inventoryData.slots[i].itemType;
                inventoryCount[i] = inventoryData.slots[i].itemCount;
                if (inventoryCount[i] > 1)
                    inventorySlots[i].GetChild(0).GetComponent<TMP_Text>().text = inventoryCount[i].ToString();
                if (inventory[i] != ItemType.Nothing)
                {
                    inventoryIcons[i] = Instantiate<RectTransform>(itemPrefabs[(int)inventory[i]]);
                    inventoryIcons[i].SetParent(inventorySlots[i], false);
                }
            }
        }
        for (int i = 0; i < 3; i++)
            SpawnMonster();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && slotToDelete != null)
            StartCoroutine("SetDelete", slotToDelete); //Hides the delete button on any click
    }

    public void AddItem(ItemType item, out bool isItemPickedup)
    {
        isItemPickedup = true; //Assume that the item will be picked up
        if (inventory.Contains(item)) //Inventory already contains this type of item
        {
            int index = inventory.FindIndex((ItemType itemType) => itemType == item); //Gets the inventory slot index for this item type
            inventoryCount[index]++;
            inventorySlots[index].GetChild(0).GetComponent<TMP_Text>().text = inventoryCount[index].ToString(); //Update the displayed item amount since it's now higher than 1
        }
        else if (inventory.Contains(ItemType.Nothing)) //Inventory doesn't contain this type AND there is an empty inventory slot
        {
            int index = inventory.FindIndex((ItemType itemType) => itemType == ItemType.Nothing); //Get the index of the first empty inventory slot
            inventory[index] = item;    //Assigns item type to inventory slot
            inventoryCount[index] = 1;  //Text isn't update since there is only 1 item
            inventoryIcons[index] = Instantiate<RectTransform>(itemPrefabs[(int)item]); //Spwans item icon
            inventoryIcons[index].SetParent(inventorySlots[index], false);
        }
        else //If item isn't picked up
            isItemPickedup = false; //It remains on the field
    }

    public void RevealDelete(RectTransform slotDeleteButton)
    {
        StartCoroutine("SetDelete", slotDeleteButton);
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
            slot.GetChild(0).GetComponent<TMP_Text>().text = "-";   //Updates amount text, "-" is used insetead of leaving it blank for testing purposes
            Destroy(inventoryIcons[index].gameObject);              //Removes the item's inventory icon
        }
    }

    private void SpawnMonster()
    {
        Monster monster = Instantiate<Monster>(monsterPrefab); //Spwans monster
        monster.transform.position = new Vector3(Random.Range(5f, 10f), Random.Range(-5f, 5f)); //Puts monster at random location to the right of the player
    }

    public void Save() //Call saving with a button for testing purposes
    {
        Transform inventoryTransform = GameObject.Find("Inventory").transform;
        Record.InventoryData inventoryData = new Record.InventoryData();
        inventoryData.slots = new Record.SlotData[inventoryTransform.childCount];
        for (int i = 0; i < inventoryTransform.childCount; i++)
        {
            inventoryData.slots[i] = new Record.SlotData();
            inventoryData.slots[i].itemCount = inventoryCount[i];
            inventoryData.slots[i].itemType = inventory[i];
        }
        string path = Application.persistentDataPath + "/Inventory.json"; //Would be preferable to keep data in a harder to dit by user file
        File.WriteAllText(path, JsonUtility.ToJson(inventoryData));
    }
}
