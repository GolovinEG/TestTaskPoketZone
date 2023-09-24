using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Joystick joystick;

    private Transform playerCamera;
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        playerCamera = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Move(joystick.InputDirection);
    }

    override protected void Move(Vector3 movementInput)
    {
        base.Move(movementInput);
        playerCamera.position = new Vector3(transform.position.x, playerCamera.position.y, playerCamera.position.z);
    }
}
