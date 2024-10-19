using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class Contubernium : MonoBehaviour
{
    [Header("Contubernium Settings")]
    public Transform player; // Reference to the player
    public float followDistance = 5f; // Distance to maintain from the player
    public float followSpeed = 3f; // Speed of the Contubernium
    public List<Ally> allies; // Array to hold the allies
    public int rowDepth = 3; // Number of rows in the formation
    public float allySpacing = 1.2f; // Spacing between allies
    public float rowOffset = -0.3f; // Negative offset for depth
    [Header("Contubernium Status")]
    public bool isFollowing = true;
    public bool isAttacking = false;
    public bool isMarch = true;
    public bool isBattle = false;
    public bool isCombatMode = false;
    [Header("Command Cooldowns")]
    public float commandCooldownDuration = 3f; // Duration for command cooldown
    private float lastCommandTime; 
    [Header("A* Pathfinding")]
    [SerializeField] public AIPath aiPath; // A* pathfinding component
    [SerializeField] public AIDestinationSetter aiDestination;
    [SerializeField] public GameObject tempTargetContainer;
    public static Contubernium Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            // If not, set this as the instance and make it persistent
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Found more than one Contubernium Manager in the scene");
            Destroy(gameObject);
        }
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

        if (distanceToPlayer > followDistance && isFollowing)
        {
            // Move the Contubernium towards the player
            aiPath.maxSpeed = followSpeed; // Adjust speed for pathfinding
        }
        else
        {
            aiPath.maxSpeed = 0; // Stop moving when close enough
        }
        if(PlayerController.GetInstance().isHolding){
            foreach (Transform child in gameObject.transform)
            {
                Ally ally = child.gameObject.GetComponent<Ally>();
                if (ally != null && !ally.isDead)
                {
                    ally.isHolding = true;
                }
            }
        }else{
            foreach (Transform child in gameObject.transform)
            {
                Ally ally = child.gameObject.GetComponent<Ally>();
                if (ally != null && !ally.isDead)
                {
                    ally.isHolding = false;
                }
            }
        }
        // Update allies' positions in formation
        PositionAlliesInFormation();
    }

    public void ToggleFollow()
    {
        if (Time.time < lastCommandTime + commandCooldownDuration){
            PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Command Cooldown!");
            return;
        };

        isFollowing = !isFollowing;
        lastCommandTime = Time.time; // Update the last command time
        PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Centurio is " + (Contubernium.Instance.isFollowing ? "Following" : "Holding"));
    }

    public void OrderAttack()
    {
        if (Time.time < lastCommandTime + commandCooldownDuration){
            PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Command Cooldown!");
            return;
        };
        isFollowing = false;
        isAttacking = true;
        lastCommandTime = Time.time; // Update the last command time
        PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Attack!!!");
    }

    public void MarchFormation()
    {
        if (Time.time < lastCommandTime + commandCooldownDuration){
            PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Command Cooldown!");
            return;
        };
        isMarch = true;
        isBattle = false;
        rowDepth = 3;
        allySpacing = 1.3f;
        lastCommandTime = Time.time; // Update the last command time
        PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "March Formation!!!");
    }

    public void BattleFormation()
    {
        if (Time.time < lastCommandTime + commandCooldownDuration){
            PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Command Cooldown!");
            return;
        };
        isBattle = true;
        isMarch = false;
        rowDepth = 5;
        allySpacing = 1.5f;
        lastCommandTime = Time.time; // Update the last command time
        PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Battle Formation!!!");
        
    }

    public void ToggleCombatMode()
    {
        if (Time.time < lastCommandTime + commandCooldownDuration){
            PlayerUIManager.GetInstance().SpawnMessage(MType.Info, "Command Cooldown!");
            return;
        };
        isCombatMode = !isCombatMode; // Toggle combat mode
        lastCommandTime = Time.time; // Update the last command time
        foreach (Transform child in gameObject.transform)
        {
            Ally ally = child.gameObject.GetComponent<Ally>();
            if (ally != null && !ally.isDead)
            {
                ally.CombatToggle();
            }
        }
        PlayerUIManager.GetInstance().SpawnMessage(MType.Info, Contubernium.Instance.isCombatMode ? "Swords out!!!" : "Sheathe your weapons!!!");
    }


    public void HandleAllyDeath(Ally deadAlly)
    {
        // Remove the dead ally from the list
        allies.Remove(deadAlly);

        // Reposition the remaining allies in the formation
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

        // Create a list to track which positions are filled
        List<bool> filledPositions = new List<bool>(new bool[allyCount]);

        for (int i = 0; i < allyCount; i++)
        {
            int row = i / width; // Calculate row number
            int column = i % width; // Calculate position in the row
            allies[i].isJavelin = (column < 3);

            // Check if this position is already filled by an ally
            if (!filledPositions[i])
            {
                // Calculate target position for the ally relative to the Contubernium
                Vector3 targetPosition = basePosition 
                                        - new Vector3(column * allySpacing * xOffset, 0, row * allySpacing) 
                                        + new Vector3(row * rowOffset * xOffset, 0, 0); // Adjust for depth and offset

                // Assign the target position for each ally to follow
                allies[i].SetTargetPosition(targetPosition);
                filledPositions[i] = true; // Mark this position as filled

                // Set sprite sorting order based on depth
                int sortingOrder = (rowDepth - 1 - row) * 10;  // Calculate sorting order based on row
                allies[i].SetSpriteOrder(sortingOrder);
            }
        }

        // Fill the gaps in the ranks with the nearest allies
        for (int i = 0; i < allyCount; i++)
        {
            if (!filledPositions[i])
            {
                int closestAllyIndex = FindNearestAlly(i, filledPositions);
                if (closestAllyIndex != -1)
                {
                    // Calculate target position for the ally to fill the gap
                    Vector3 targetPosition = basePosition 
                                            - new Vector3((i % width) * allySpacing * xOffset, 0, (i / width) * allySpacing) 
                                            + new Vector3((i / width) * rowOffset * xOffset, 0, 0); // Adjust for depth and offset
                    
                    // Move the closest ally to fill the gap
                    allies[closestAllyIndex].SetTargetPosition(targetPosition);
                    filledPositions[i] = true; // Mark this position as filled
                }
            }
        }
    }

    private int FindNearestAlly(int gapIndex, List<bool> filledPositions)
    {
        float closestDistance = float.MaxValue;
        int closestAllyIndex = -1;

        // Calculate the position of the gap in world coordinates
        int row = gapIndex / Mathf.CeilToInt((float)allies.Count / rowDepth);
        int column = gapIndex % Mathf.CeilToInt((float)allies.Count / rowDepth);
        Vector3 gapPosition = transform.position 
                            - new Vector3(column * allySpacing * (transform.localScale.x > 0 ? 1 : -1), 0, row * allySpacing) 
                            + new Vector3(row * rowOffset * (transform.localScale.x > 0 ? 1 : -1), 0, 0);

        // Find the nearest ally behind the gap
        for (int i = 0; i < allies.Count; i++)
        {
            if (filledPositions[i]) continue; // Skip already filled allies

            float distance = Vector3.Distance(allies[i].transform.position, gapPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestAllyIndex = i;
            }
        }

        return closestAllyIndex; // Return the index of the nearest ally
    }

}
