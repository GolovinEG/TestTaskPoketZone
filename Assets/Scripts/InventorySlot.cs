using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    private GameManager game;

    private void Awake()
    {
        game = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void RevealDelete(RectTransform slotDeleteButton) //Need to call the function inderectly to be able to create an inventory slot prefab
    {
        game.RevealDelete(slotDeleteButton);
    }

    public void DeleteItem(RectTransform slot) //Need to call the function inderectly to be able to create an inventory slot prefab
    {
        game.DeleteItem(slot);
    }
}
