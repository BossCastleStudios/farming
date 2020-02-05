using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;

    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return;

        var gamepadLeftStick = gamepad.leftStick.ReadValue();
        Vector3 targetVelocity = new Vector3(gamepadLeftStick.x, 0, gamepadLeftStick.y);
        targetVelocity *= speed;

        transform.position += targetVelocity;

        Vector3 direction = new Vector3(gamepadLeftStick.x, 0, gamepadLeftStick.y);
        if (direction.magnitude > 0.01F)
        {
            direction = Camera.main.transform.TransformDirection(direction);
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}
