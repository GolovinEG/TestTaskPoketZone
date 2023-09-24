using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Nothing = -1,
    Makarov,
    AK,
    Bulletproof
}

public class Item : MonoBehaviour
{
    public ItemType type;

    private GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            manager.AddItem(type);
            Destroy(gameObject);
        }
    }
}
