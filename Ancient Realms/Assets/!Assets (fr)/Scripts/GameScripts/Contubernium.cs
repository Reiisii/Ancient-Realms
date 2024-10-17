using Pathfinding;
using UnityEngine;

public class Contubernium : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float followDistance = 5f; // Distance to maintain from the player
    public float followSpeed = 3f; // Speed of the Contubernium
    public Ally[] allies; // Array to hold the allies
    [Header("A* Pathfinding")]
    [SerializeField] public AIPath aiPath; // A* pathfinding component
    [SerializeField] public AIDestinationSetter aiDestination;

    private void Start()
    {
        // Get the player transform (assuming it has the tag "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
        aiDestination.target = player;

        // Initialize the allies array (adjust size as needed)
        allies = new Ally[8];
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > followDistance)
        {
            // Move the Contubernium towards the player
            aiPath.maxSpeed = followSpeed; // Adjust the speed for pathfinding
            SetFacingDirection(); // Update facing direction based on movement
        }
        else
        {
            aiPath.maxSpeed = 0; // Stop moving when close enough
        }
    }

    private void SetFacingDirection()
    {
        // Determine the direction to the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Check if the direction is predominantly left or right
        if (directionToPlayer.x > 0)
        {
            // Player is to the right, face right
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            // Player is to the left, face left
            transform.localScale = new Vector3(-1, 1, 1);
        }

    }
}
