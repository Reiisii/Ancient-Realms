using UnityEngine;

public class FormationManager : MonoBehaviour
{
    public Ally[] blockers; // Blockers in each row
    public Ally[] attackers; // Attackers in the second row
    public Ally[] throwers;  // Throwers in the third row
    public Transform[] formationTargets; 
    public static FormationManager Instance { get; private set; }

    private void Awake()
    {
        // Ensure that there's only one instance of the Formation Manager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        PopulateAllies(); // Populate allies on start
        CreateFormationTargets();
    }

    private void PopulateAllies()
    {
        // Find the NPCs GameObject
        GameObject npcs = GameObject.FindGameObjectWithTag("NPCS");
        if (npcs == null)
        {
            Debug.LogError("No NPCS GameObject found with the tag 'NPCS'.");
            return;
        }

        // Get all Ally components in the NPCS GameObject's children
        Ally[] allAllies = npcs.GetComponentsInChildren<Ally>();

        // Initialize arrays for each role
        blockers = new Ally[3]; // Set size as needed
        attackers = new Ally[3]; // Set size as needed
        throwers = new Ally[3];  // Set size as needed

        // Populate the roles based on some criteria (for example, by index)
        for (int i = 0; i < allAllies.Length; i++)
        {
            // Example logic to assign roles
            if (i < blockers.Length)
            {
                blockers[i] = allAllies[i]; // Assign to blockers
                allAllies[i].row = 1; // Set row number
            }
            else if (i < blockers.Length + attackers.Length)
            {
                attackers[i - blockers.Length] = allAllies[i]; // Assign to attackers
                allAllies[i].row = 2; // Set row number
            }
            else if (i < blockers.Length + attackers.Length + throwers.Length)
            {
                throwers[i - blockers.Length - attackers.Length] = allAllies[i]; // Assign to throwers
                allAllies[i].row = 3; // Set row number
            }
        }

        PositionAllies(); // Position allies after populating
    }

    public void PositionAllies()
    {
        // Define the spacing between allies
        float spacing = 1.5f; // Adjust as needed

        // Position the throwers (row 3)
        PositionRow(throwers, 2, spacing);

        // Position the attackers (row 2)
        PositionRow(attackers, 1, spacing);

        // Position the blockers (row 1)
        PositionRow(blockers, 0, spacing);
    }

    private void PositionRow(Ally[] row, float height, float spacing)
    {
        for (int i = 0; i < row.Length; i++)
        {
            if (row[i] != null) // Check if ally exists
            {
                row[i].transform.position = new Vector3(i * spacing, height, 0);
            }
        }
    }

    public void RemoveAlly(Ally ally)
    {
        // Logic to remove the ally and reposition the formation
        for (int i = 0; i < blockers.Length; i++)
        {
            if (blockers[i] == ally)
            {
                blockers[i] = null; // Mark as dead
                break;
            }
        }

        for (int i = 0; i < attackers.Length; i++)
        {
            if (attackers[i] == ally)
            {
                attackers[i] = null; // Mark as dead
                break;
            }
        }

        for (int i = 0; i < throwers.Length; i++)
        {
            if (throwers[i] == ally)
            {
                throwers[i] = null; // Mark as dead
                break;
            }
        }

        PositionAllies(); // Reposition allies after removal
    }
    private void CreateFormationTargets()
    {
        // Define the spacing between allies
        float spacing = 1.5f; // Adjust as needed
        formationTargets = new Transform[blockers.Length + attackers.Length + throwers.Length];

        // Create target positions for each ally based on their role
        for (int i = 0; i < blockers.Length; i++)
        {
            GameObject target = new GameObject($"BlockerTarget_{i}");
            target.transform.position = new Vector3(i * spacing, 0, 0);
            formationTargets[i] = target.transform;
        }
        for (int i = 0; i < attackers.Length; i++)
        {
            GameObject target = new GameObject($"AttackerTarget_{i}");
            target.transform.position = new Vector3(i * spacing, 1, 0);
            formationTargets[blockers.Length + i] = target.transform;
        }
        for (int i = 0; i < throwers.Length; i++)
        {
            GameObject target = new GameObject($"ThrowerTarget_{i}");
            target.transform.position = new Vector3(i * spacing, 2, 0);
            formationTargets[blockers.Length + attackers.Length + i] = target.transform;
        }
    }
    public Transform GetFormationTarget(Ally ally)
    {
        if (ally.row == 1)
            return formationTargets[ally.positionInRow]; // Blockers
        else if (ally.row == 2)
            return formationTargets[blockers.Length + ally.positionInRow]; // Attackers
        else if (ally.row == 3)
            return formationTargets[blockers.Length + attackers.Length + ally.positionInRow]; // Throwers
        return null; // Default case
    }


}
