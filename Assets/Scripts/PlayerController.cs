using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public AudioSource walkingSfx;

    private Rigidbody _rigidbody;
    private bool isWalking = false;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isWalking && !walkingSfx.isPlaying)
        {
            walkingSfx.Play();
        }
        else if (!isWalking && walkingSfx.isPlaying)
        {
            walkingSfx.Stop();
        }
    }

    void FixedUpdate()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return;

        var gamepadLeftStick = gamepad.leftStick.ReadValue();
        Vector3 targetVelocity = new Vector3(gamepadLeftStick.x, 0, gamepadLeftStick.y);
        targetVelocity *= speed;

        _rigidbody.velocity = targetVelocity;

        isWalking = gamepad.leftStick.IsActuated();

            Vector3 direction = new Vector3(gamepadLeftStick.x, 0, gamepadLeftStick.y);
        if (direction.magnitude > 0.01F)
        {
            direction = Camera.main.transform.TransformDirection(direction);
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}
