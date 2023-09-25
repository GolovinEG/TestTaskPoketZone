using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxHealth;
    public float speed;
    public float attackDamage;
    public float attackCooldown;

    protected bool canAttack = true;

    private float health;
    // Start is called before the first frame update
    virtual protected void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual protected void Move(Vector3 movementInput)
    {
        transform.Translate(movementInput * Time.deltaTime * speed);
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
            return;
        }
        Transform lifebar = transform.GetChild(0); //Get the lifebar
        lifebar.localScale = new Vector3(health / maxHealth, lifebar.localScale.y); //Reduce the lifebar
        lifebar.localPosition = new Vector3(lifebar.localScale.x / 2 - 0.5f, lifebar.localPosition.y); //Make sure the left edge of the lifebar remains the same
    }

    virtual protected void Die()
    {
        Destroy(gameObject);
    }
    protected IEnumerator Attack(Character target)
    {
        target.Damage(attackDamage);
        canAttack = false; //Starts cooldown
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true; //Ends cooldown
    }
}
