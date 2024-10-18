using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Ally : MonoBehaviour
{
    [Header("Ally Settings")]
    public Transform contubernium; // Reference to the Contubernium
    private Vector3 targetPosition; 
    public float followDistance = 1.5f; // Distance to maintain from the Contubernium
    public float followSpeed = 2.5f; // Speed of the ally
    public int row; // Row of the ally in the formation
    public int positionInRow; // Position in the row

    [Header("AI Components")]
    public Animator animator; // Animator for animations
    public AIPath aiPath; // A* pathfinding component
    public AIDestinationSetter aiDestination; // A* destination setter
    public SpriteRenderer spriteRenderer;
    
    private Vector3 previousPosition; // Store previous position for movement detection
    private Vector3 originalScale; // Store the original scale of the ally

    private GameObject tempTargetContainer;

    private void Awake()
    {
        previousPosition = transform.position;
        aiPath = GetComponent<AIPath>();
        aiDestination = GetComponent<AIDestinationSetter>();
        aiPath.maxSpeed = followSpeed;
        tempTargetContainer = contubernium.gameObject.GetComponent<Contubernium>().tempTargetContainer;
        
        // Store the original scale
        originalScale = transform.localScale;
    }

    private void Update()
    {
        aiDestination.target = CreateTempTarget(targetPosition);
        aiPath.maxSpeed = followSpeed;

        OnMove();
    }

    private Transform CreateTempTarget(Vector3 position)
    {
        GameObject tempTarget = new GameObject("TempTarget");
        tempTarget.transform.position = position;
        tempTarget.transform.parent = tempTargetContainer.transform; // Set parent to "TempTargets"
        return tempTarget.transform;
    }

    public void OnMove()
    {
        float distanceMoved = Vector3.Distance(transform.position, previousPosition);
        
        // Check if the object has moved more than the threshold value
        if (distanceMoved > 0.01f)
        {
            animator.SetBool("isMoving", true);

            // Determine movement direction and flip ally if necessary
            if (transform.position.x < targetPosition.x) // Moving to the right
            {
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); // Face right
            }
            else if (transform.position.x > targetPosition.x) // Moving to the left
            {
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); // Face left
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        previousPosition = transform.position;
    }


    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    public void SetSpriteOrder(int order)
    {
        spriteRenderer.sortingOrder = order;
    }
}
