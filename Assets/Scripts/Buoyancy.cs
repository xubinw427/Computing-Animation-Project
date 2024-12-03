using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    [Header("Water Settings")]
    public float waterLevel = -1;               // Y position of water surface
    public Transform waterSurface;             // Optional water surface reference
    public float buoyancyForce = 9.8f;         // Upward force
    public float maxBuoyancyMultiplier = 10f;  // Clamp for maximum buoyancy

    [Header("Drag Settings")]
    public float dragInWater = 1.5f;           // Water drag
    public float angularDragInWater = 2.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Dynamically update water level if a water surface is assigned
        if (waterSurface != null)
        {
            waterLevel = waterSurface.position.y;
        }
    }

    void FixedUpdate()
    {
        if (transform.position.y < waterLevel) // Check if submerged
        {
            float depth = waterLevel - transform.position.y;
            float buoyantForce = Mathf.Clamp(buoyancyForce * depth, 0, buoyancyForce * maxBuoyancyMultiplier);

            // Apply upward buoyant force
            rb.AddForce(Vector3.up * buoyantForce, ForceMode.Acceleration);

            // Apply stabilization torque
            Vector3 stabilizationTorque = -rb.angularVelocity * angularDragInWater;
            rb.AddTorque(stabilizationTorque);

            // Apply drag to reduce motion
            rb.drag = dragInWater;
            rb.angularDrag = angularDragInWater;
        }
        else
        {
            // Reset drag values when not in water
            rb.drag = 0;
            rb.angularDrag = 0.05f;
        }
    }
}

