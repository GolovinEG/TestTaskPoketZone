using UnityEngine;

public enum MonsterType
{
    Zomie,
    Flesh
}

public class Monster : Character
{
    public float attackRange = 1;
    public Transform[] monsterPrefabs = new Transform[2];
    public Item[] itemDrops = new Item[3];

    private Transform sprite;
    public Vector3 PlayerDistance { get; set; } = Vector3.zero;

    private void Awake()
    {
        sprite = Instantiate<Transform>(monsterPrefabs[Random.Range(0, 2)]); //Spwans graphics
        sprite.SetParent(transform, false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerDistance = (collision.transform.position - transform.position); //Gets the distance to player to determine movement direction and attack availability
            Move(PlayerDistance.normalized * speed * Time.deltaTime); //Over twice as slow with the same speed
            if (PlayerDistance.magnitude <= attackRange && canAttack) //When in range and off cooldown
            {
                StartCoroutine(Attack(collision.GetComponent<Player>())); //Attack player
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            player.Targets.Add(this); //Add to target list for targeting shots
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            player.Targets.Remove(this); //Remove from target list
            PlayerDistance = Vector3.zero;
        }
    }

    protected override void Die()
    {
        Item item = Instantiate<Item>(itemDrops[Random.Range(0, 3)]); //Item drop
        item.transform.position = transform.position;
        base.Die();
    }
}
