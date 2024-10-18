using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class Contubernium : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float followDistance = 5f; // Distance to maintain from the player
    public float followSpeed = 3f; // Speed of the Contubernium
    public List<Ally> allies; // Array to hold the allies
    public int rowDepth = 3; // Number of rows in the formation
    public float allySpacing = 1.2f; // Spacing between allies
    public float rowOffset = -0.3f; // Negative offset for depth
    [Header("A* Pathfinding")]
    [SerializeField] public AIPath aiPath; // A* pathfinding component
    [SerializeField] public AIDestinationSetter aiDestination;
    [SerializeField] public GameObject tempTargetContainer;

    private void Awake()
    {
        foreach (Transform child in gameObject.transform)
        {
            Ally ally = child.gameObject.GetComponent<Ally>();
            if (ally != null)
            {
                allies.Add(ally);
            }
        }
    }

    private void Start()
    {
        // Get the player transform (assuming it has the tag "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
        aiDestination.target = player;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > followDistance)
        {
            // Move the Contubernium towards the player
            aiPath.maxSpeed = followSpeed; // Adjust speed for pathfinding
        }
        else
        {
            aiPath.maxSpeed = 0; // Stop moving when close enough
        }

        // Update allies' positions in formation
        PositionAlliesInFormation();
    }

    private void PositionAlliesInFormation()
    {
        int allyCount = allies.Count;
        int width = Mathf.CeilToInt((float)allyCount / rowDepth); // Calculate width based on number of allies

        // Determine facing direction for allies
        float xOffset = transform.localScale.x > 0 ? 1 : -1; // Positive for right, negative for left

        // Calculate the base position of the Contubernium for the formation
        Vector3 basePosition = transform.position;

        for (int i = 0; i < allyCount; i++)
        {
            int row = i / width; // Calculate row number
            int column = i % width; // Calculate position in the row

            // Calculate target position for each ally relative to the Contubernium
            Vector3 targetPosition = basePosition 
                                     - new Vector3(column * allySpacing * xOffset, 0, row * allySpacing) 
                                     + new Vector3(row * rowOffset * xOffset, 0, 0); // Adjust for depth and offset

            // Assign the target position for each ally to follow
            allies[i].SetTargetPosition(targetPosition);

            // Set sprite sorting order based on depth
            int sortingOrder = (rowDepth - 1) - row; // Calculate sorting order based on row
            allies[i].SetSpriteOrder(sortingOrder);
        }
    }
}
