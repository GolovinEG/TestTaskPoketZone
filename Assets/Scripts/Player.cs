using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : Character
{
    public int ammo;

    private Transform playerCamera;
    private Joystick joystick;
    private TMP_Text ammoCounter;

    public List<Monster> Targets { get; set; } = new List<Monster>();
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        playerCamera = GameObject.Find("Main Camera").transform;
        joystick = GameObject.Find("Joystick").GetComponent<Joystick>();
        ammoCounter = GameObject.Find("AmmoCounter").GetComponent<TMP_Text>();
        ammoCounter.text = ammo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Move(joystick.InputDirection);
    }

    override protected void Move(Vector3 movementInput)
    {
        base.Move(movementInput);
        playerCamera.position = new Vector3(transform.position.x, playerCamera.position.y, playerCamera.position.z); //The camera moves along with the player horizontally
    }

    public void Shoot()
    {
        if (Targets.Count > 0) //Ammo and canAttack aren't checked for testing purposes.
        {
            ammo--;
            ammoCounter.text = ammo.ToString();
            Monster monster = Targets[0]; //Picks the first monster in list
            for (int i = 1; i < Targets.Count; i++)
                if (Targets[i].PlayerDistance.magnitude < monster.PlayerDistance.magnitude) //Compares distance
                    monster = Targets[i]; //Gets the closer monster
            StartCoroutine(Attack(monster)); //Shoot at the closest monster in list
        }
    }
}
