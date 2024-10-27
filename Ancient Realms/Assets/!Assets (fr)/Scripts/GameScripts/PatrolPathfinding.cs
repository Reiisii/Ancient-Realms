using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

public class PatrolPathfinding : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] public Animator animator; 
    [SerializeField] public AIPath aiPath;
    [SerializeField] public AIDestinationSetter aiDestinationSetter; // Attach the AIDestinationSetter here
    [SerializeField] public Vector3[] patrolPoints; // Array of patrol points (set in the Inspector or via script)
    [SerializeField] public float reachDistance = 0.5f; // Distance threshold to consider the point reached
    [SerializeField] public float waitTimeAtPoint = 3f; // Time to wait at each point
    [SerializeField] private int currentPointIndex = 0;
    [SerializeField] private int renderIndex;
    private Transform patrolTarget; // Internal target used for AI Destination
    public List<int> equipments;
    public List<EquipmentSO> equippedSOs;
    [Header("Patrol Settings")]
    public bool canMove = true;
    public bool IsMoving = false;
    [Header("Sprite Renderer")]
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public SpriteRenderer mainHolster;
    [SerializeField] public SpriteRenderer hand;
    [SerializeField] public SpriteRenderer forearm;
    [SerializeField] public SpriteRenderer mainSlot;
    [SerializeField] public SpriteRenderer shieldSlotFront;
    [SerializeField] public SpriteRenderer shieldSlotBack;
    [SerializeField] public SpriteRenderer shieldSlotHandle;
    [SerializeField] public SpriteRenderer javelinSlot;
    private Vector3 previousPosition;
    private void Awake(){
        previousPosition = transform.position;
        InitializeEquipments();
        SetSpriteOrder(renderIndex);
    }
    private void Start()
    {
        // Create an empty GameObject as the patrol target and place it initially at the first patrol point
        patrolTarget = new GameObject("PatrolTarget").transform;
        patrolTarget.position = patrolPoints[currentPointIndex];
        
        // Assign patrol target to AIDestinationSetter's target
        aiDestinationSetter.target = patrolTarget;
    }

    private void Update()
    {
        if (IsMoving)
        {
            SetFacingDirection(aiPath.velocity); // Face the direction of movement
        }

        // Check if close enough to the current patrol point
        if (Vector3.Distance(transform.position, patrolTarget.position) <= reachDistance)
        {
            StartCoroutine(MoveToNextPoint());
        }

    }
    private void FixedUpdate(){
        OnMove();
    }
    public void OnMove()
    {
        float distanceMoved = Vector3.Distance(transform.position, previousPosition);
            
        // Check if the object has moved more than the threshold value
        if (distanceMoved > 0.01f)
        {
            IsMoving = true;
            animator.SetBool("isMoving", true);
        }
        else
        {
            IsMoving = false;
            animator.SetBool("isMoving", false);
        }

        previousPosition = transform.position;
    }
    private void SetFacingDirection(Vector3 velocity)
    {
        if (velocity.x != 0) // Only flip if moving horizontally
        {
            Vector3 newScale = transform.localScale;
            newScale.x = Mathf.Abs(newScale.x) * Mathf.Sign(velocity.x);
            transform.localScale = newScale;

        }
    }
    public void SetSpriteOrder(int order)
    {
        spriteRenderer.sortingOrder = order;
        mainHolster.sortingOrder = order + 3;
        mainSlot.sortingOrder = order + 3;
        shieldSlotBack.sortingOrder = order - 1;
        shieldSlotFront.sortingOrder = order + 2;
        shieldSlotHandle.sortingOrder = order + 1;
        javelinSlot.sortingOrder = order + 3;
        hand.sortingOrder = order + 4;
        forearm.sortingOrder = order + 4;
    }
    private IEnumerator MoveToNextPoint()
    {
        canMove = false;
        yield return new WaitForSeconds(waitTimeAtPoint);
        
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        patrolTarget.position = patrolPoints[currentPointIndex];
        
        canMove = true;
    }

    public void InitializeEquipments(){
        List<EquipmentSO> newList = new List<EquipmentSO>();
        foreach(int equipment in equipments){
            EquipmentSO equipmentSO = AccountManager.Instance.equipments.Where(eq => eq.equipmentId == equipment).FirstOrDefault();
            EquipmentSO copiedEquipmentSO = equipmentSO.CreateCopy(equipment, 1, 1);
            newList.Add(copiedEquipmentSO);
        }
        equippedSOs = newList;
        mainHolster.sprite = equippedSOs[4].front;
        mainSlot.sprite = equippedSOs[4].front;
        shieldSlotFront.sprite = equippedSOs[5].front;
        shieldSlotBack.sprite = equippedSOs[5].back;
        javelinSlot.sprite = equippedSOs[6].front;
    }
    private void OnDestroy()
    {
        // Clean up the patrol target GameObject when this object is destroyed
        if (patrolTarget != null)
            Destroy(patrolTarget.gameObject);
    }

}
