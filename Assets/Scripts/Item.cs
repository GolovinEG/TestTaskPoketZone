using UnityEngine;

public enum ItemType
{
    Nothing = -1, //For empty inventory slots
    Makarov,
    AK,
    Bulletproof,
    Helmet
}

public class Item : MonoBehaviour
{
    public ItemType type;
    public RectTransform icon; //A list of ItemType to inventory icon would be preferable

    private GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            bool isItemPickedup;
            manager.AddItem(this, out isItemPickedup);
            if (isItemPickedup) //Checks if item was added to inventory
                Destroy(gameObject); //Removes item
        }
    }
}
