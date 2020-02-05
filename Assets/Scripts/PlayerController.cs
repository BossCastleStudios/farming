﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public AudioSource walkingSfx;
    public int gatherSpeed = 15;

    public bool CanGatherResources { get; private set; }
    public bool IsGathering { get; private set; }
    public int WoodResource { get; private set; }

    private ResourceSource nearbySource;
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

        if (CanGatherResources)
        {
            if (!IsGathering && Gamepad.current.aButton.isPressed)
            {
                IsGathering = true;
                StartCoroutine(GatherResources(nearbySource));
            }
        }
        else if (IsGathering)
        {
            IsGathering = false;
        }
    }

    private IEnumerator GatherResources(ResourceSource source)
    {
        while (source.resourceAmount > 0)
        {
            int before = source.resourceAmount;
            source.resourceAmount -= gatherSpeed;
            source.resourceAmount = Mathf.Max(0, source.resourceAmount);
            WoodResource += before - source.resourceAmount;
            yield return new WaitForSeconds(1);
        }

        IsGathering = false;
        CanGatherResources = false;
    }

    void FixedUpdate()
    {
        if (IsGathering) return;

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

    void OnTriggerEnter(Collider other)
    {
        ResourceSource source = other.GetComponent<ResourceSource>();
        if (source != null && source.resourceAmount > 0)
        {
            CanGatherResources = true;
            nearbySource = source;
        }
    }

    void OnTriggerStay(Collider other)
    {

    }

    void OnTriggerExit(Collider other)
    {
        ResourceSource source = other.GetComponent<ResourceSource>();
        if (source != null)
        {
            CanGatherResources = false;
            nearbySource = null;
        }
    }
}
