using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Ally : MonoBehaviour
{
    [Header("Ally Settings")]
    public Transform contubernium; // Reference to the Contubernium
    public float followDistance = 1.5f; // Distance to maintain from the Contubernium
    public float followSpeed = 2.5f; // Speed of the ally
    public int row; // Row of the ally in the formation
    public int positionInRow; // Position in the row

    [Header("AI Components")]
    public Animator animator; // Animator for animations
    public AIPath aiPath; // A* pathfinding component
    public AIDestinationSetter aiDestination; // A* destination setter

    private Vector3 previousPosition; // Store previous position for movement detection

    private void Awake()
    {
        previousPosition = transform.position;
        aiPath = GetComponent<AIPath>();
        aiDestination = GetComponent<AIDestinationSetter>();
        aiPath.maxSpeed = followSpeed;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, contubernium.position) > followDistance)
        {
            aiDestination.target = contubernium; // Set the target to the Contubernium
            aiPath.maxSpeed = followSpeed; // Set follow speed
        }
        else
        {
            aiPath.maxSpeed = 0; // Stop moving when close enough
        }

        OnMove(); // Check movement status
    }

    public void SetFacingDirection(Vector3 contuberniumScale)
    {
        // Ensure the ally faces the same direction as the Contubernium
        if (contuberniumScale.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Face right
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); // Face left
        }
    }

    public void OnMove()
    {
        float distanceMoved = Vector3.Distance(transform.position, previousPosition);
        
        // Check if the object has moved more than the threshold value
        if (distanceMoved > 0.01f)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        previousPosition = transform.position;
    }
}
