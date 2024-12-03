using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterArea : MonoBehaviour
{

    public Vector3 flowDirection = new Vector3(1, 0, 0); // Direction of current
    public float flowStrength = 1.0f; // Speed of flow

    private Vector3 normalizedFlowDirection;

    public ParticleSystem splashEffect;


    void Start()
    {
        normalizedFlowDirection = flowDirection.normalized;
    }

    void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(normalizedFlowDirection * flowStrength, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            if (splashEffect != null)
            {
                splashEffect.Play();
            }
            Debug.Log("Splash effect triggered!");
        }
    }
}
