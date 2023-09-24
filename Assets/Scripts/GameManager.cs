using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public RectTransform[] itemPrefabs = new RectTransform[3];
    public Monster monsterPrefab;

    private List<ItemType> inventory = new List<ItemType>();
    private List<RectTransform> inventorySlots = new List<RectTransform>();
    private int[] inventoryCount = { 0, 0, 0 };
    private RectTransform[] inventoryIcons = new RectTransform[3];
    private bool isDeleting = false;
    // Start is called before the first frame update
    void Start()
    {
        Transform inventoryTransform = GameObject.Find("Inventory").transform;
        for (int i = 0; i < inventoryTransform.childCount; i++)
        {
            inventorySlots.Add((RectTransform)inventoryTransform.GetChild(i));
            inventory.Add(ItemType.Nothing);
        }
        for (int i = 0; i < 3; i++)
            SpawnMonster();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isDeleting)
            StartCoroutine("SetDelete", false);
    }

    public void AddItem(ItemType item)
    {
        if (inventory.Contains(item))
        {
            int index = inventory.FindIndex((ItemType itemType) => itemType == item);
            inventoryCount[index]++;
            inventorySlots[index].GetChild(0).GetComponent<TMP_Text>().text = inventoryCount[index].ToString();
        }
        else
        {
            int index = inventory.FindIndex((ItemType itemType) => itemType == ItemType.Nothing);
            inventory[index] = item;
            inventoryCount[index] = 1;
            inventoryIcons[index] = Instantiate<RectTransform>(itemPrefabs[(int)item]);
            inventoryIcons[index].SetParent(inventorySlots[index], false);
        }
    }

    public void OnClickDelete()
    {
        StartCoroutine("SetDelete", true);
    }

    private IEnumerator SetDelete(bool state)
    {
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0)); //Delay to avoid disabling deleting too early
        isDeleting = state;
        for (int i = 0; i < inventorySlots.Count; i++)
            if (inventory[i] != ItemType.Nothing)
                inventorySlots[i].GetComponent<Button>().enabled = state;
    }

    public void DeleteItem(RectTransform slot)
    {
        int index = inventorySlots.FindIndex((RectTransform rectTransform) => rectTransform == slot);
        inventory[index] = ItemType.Nothing;
        inventoryCount[index] = 0;
        slot.GetChild(0).GetComponent<TMP_Text>().text = "-";
        Destroy(inventoryIcons[index].gameObject);
        slot.GetComponent<Button>().enabled = false;
    }

    private void SpawnMonster()
    {
        Monster monster = Instantiate<Monster>(monsterPrefab);
        monster.transform.position = new Vector3(Random.Range(5f, 10f), Random.Range(-5f, 5f));
    }
}
