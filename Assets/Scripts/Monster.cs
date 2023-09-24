using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Zomie,
    Flesh
}

public class Monster : Character
{
    public float attackCooldown = 3;
    public float attackDamage = 10;
    public float attackRange = 1;
    public Transform[] monsterPrefabs = new Transform[2];
    public Item[] itemDrops = new Item[3];

    private bool canAttack = true;
    private Transform sprite;
    // Start is called before the first frame update
    protected override void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake() //Spwans graphics
    {
        sprite = Instantiate<Transform>(monsterPrefabs[Random.Range(0, 2)]);
        sprite.SetParent(transform, false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Vector3 playerDistance = (collision.transform.position - transform.position); //Gets the distance to player to determine movement direction and attack availability
            Move(playerDistance.normalized * speed * Time.deltaTime); //Over twice as slow with the same speed
            if (playerDistance.magnitude <= attackRange && canAttack)
            {
                StartCoroutine("Attack", collision.GetComponent<Player>());
            }
        }
    }

    private IEnumerator Attack(Player player)
    {
        player.Damage(attackDamage);
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    protected override void Die()
    {
        Item item = Instantiate<Item>(itemDrops[Random.Range(0, 3)]);
        item.transform.position = transform.position;
        base.Die();
    }
}
