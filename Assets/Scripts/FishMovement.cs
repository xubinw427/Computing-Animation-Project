using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 1f;
    public float turnSpeed = 2f;
    public float directionChangeInterval = 3f;
    [Header("Boundary Settings")]
    public GameObject waterArea;
    private BoxCollider waterAreaCollider;
    private Vector3 targetDirection;
    private float timeSinceLastChange;
    private Bounds waterBounds;
    private float speedMultiplier = 1f;

    void Start()
    {
        // Get the BoxCollider and cache bounds
        if (waterArea != null)
        {
            waterAreaCollider = waterArea.GetComponent<BoxCollider>();
            if (waterAreaCollider == null)
            {
                Debug.LogError("WaterArea does not have a BoxCollider!");
                enabled = false;
                return;
            }
            waterBounds = waterAreaCollider.bounds;
        }
        else
        {
            Debug.LogError("WaterArea GameObject is not assigned!");
            enabled = false;
            return;
        }

        SetNewTargetDirection();
        StartCoroutine(RandomizeSpeed());
    }

    void Update()
    {
        timeSinceLastChange += Time.deltaTime;

        if (IsNearBoundary() || timeSinceLastChange >= directionChangeInterval)
        {
            SetNewTargetDirection();
            timeSinceLastChange = 0f;
        }

        SmoothlyTurn();
        transform.Translate(Vector3.forward * speed * speedMultiplier * Time.deltaTime);
        ClampPositionToBoundary();
    }

    private void SetNewTargetDirection()
    {
        if (IsNearBoundary())
        {
            // Turn away from boundary
            Vector3 awayFromBoundary = (waterBounds.center - transform.position).normalized;
            targetDirection = awayFromBoundary + new Vector3(
                Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f)
            ).normalized;
        }
        else
        {
            // Random direction
            targetDirection = new Vector3(
                Random.Range(-1f, 1f),
                0,
                Random.Range(-1f, 1f)
            ).normalized;
        }
    }

    private void SmoothlyTurn()
    {
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }

    private bool IsNearBoundary()
    {
        float buffer = 2f;
        return transform.position.x <= waterBounds.min.x + buffer || transform.position.x >= waterBounds.max.x - buffer ||
               transform.position.y <= waterBounds.min.y + buffer || transform.position.y >= waterBounds.max.y - buffer ||
               transform.position.z <= waterBounds.min.z + buffer || transform.position.z >= waterBounds.max.z - buffer;
    }

    private void ClampPositionToBoundary()
    {
        float buffer = 3f;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, waterBounds.min.x + buffer, waterBounds.max.x - buffer),
            Mathf.Clamp(transform.position.y, waterBounds.min.y + buffer, waterBounds.max.y - buffer),
            Mathf.Clamp(transform.position.z, waterBounds.min.z + buffer, waterBounds.max.z - buffer)
        );
    }

    private IEnumerator RandomizeSpeed()
    {
        while (true)
        {
            speedMultiplier = Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }

    private void OnDrawGizmos()
    {
        if (waterArea != null && waterAreaCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(waterAreaCollider.bounds.center, waterAreaCollider.bounds.size);
        }
    }
}