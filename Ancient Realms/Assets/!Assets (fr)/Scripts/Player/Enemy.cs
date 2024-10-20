using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Ink;
using Pathfinding;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stat")]
    private float maxHP = 100f;
    public float currentHP = 100f;
    public string id = "";
    public bool isDead = false;
    public bool isDummy = false;
    public int level = 1;
    public int tier = 1;
    public int equipmentLevel = 1;
    private float maxStamina = 70f;
    public float stamina = 70f;
    private float walkSpeed = 3.2f;
    private float runSpeed = 5f;
    public float staminaDepletionRate = 20f;
    private float staminaRegenRate = 10f;
    private float attackRange = 0.5f;
    private float damage = 1;
    private float armor = 0;
    public float detectionRadius = 50f;
    [Header("Enemy Controller")]
    public bool invulnerable = false;
    public bool canMove = true;
    public bool IsMoving = false;
    public bool isCombatMode = false;
    public bool isRunning = false;
    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool isEquipping = false;
    public List<int> equipments;
    public List<EquipmentSO> equippedSOs;
    private List<Ally> enemiesInRange = new List<Ally>();
    private Vector3 previousPosition;
    public LayerMask enemyLayer;
    [Header("Enemy Components")]
    [SerializeField] public Animator animator;
    [SerializeField] public AIPath aiPath;
    [SerializeField] public AIDestinationSetter aiDestination;
    [SerializeField] public SpriteRenderer mainHolster;
    [SerializeField] public SpriteRenderer hand;
    [SerializeField] public SpriteRenderer forearm;
    [SerializeField] public SpriteRenderer mainSlot;
    [SerializeField] public SpriteRenderer shieldSlotFront;
    [SerializeField] public SpriteRenderer shieldSlotBack;
    [SerializeField] public SpriteRenderer shieldSlotHandle;
    [SerializeField] public Transform attackPoint;
    public bool _isFacingRight = true;
    public bool IsFacingRight 
    { 
        get 
        {
            // Enemy is facing right if the localScale.x is positive
            return transform.localScale.x > 0;
        }
        private set 
        {
            // Flip only if the desired facing direction is different from the current one
            if ((value && transform.localScale.x < 0) || (!value && transform.localScale.x > 0))
            {
                // Flip the local scale by multiplying x by -1
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void Awake(){
        previousPosition = transform.position;
        CalculateStatsForCurrentLevel();
        if(isDummy) return;
        aiPath.maxSpeed = walkSpeed;
    }
    private void Update(){
        GameObject nearestTarget = FindNearestTargetWithinRadius();
        if (nearestTarget != null)
        {
            // Set the target for the AI Destination Setter
            aiDestination.target = nearestTarget.transform;
            if (nearestTarget != null)
            {
                isCombatMode = true;
                // Set the target for the AI Destination Setter
                aiDestination.target = nearestTarget.transform;

                // Check the distance to the target
                float distanceToTarget = Vector3.Distance(transform.position, nearestTarget.transform.position);
                if(!canMove) {
                    aiPath.maxSpeed = 0;
                    return;
                };
                if(distanceToTarget <= 0.5f){
                    aiPath.maxSpeed = 0;
                }else if(distanceToTarget <= 12f){
                    aiPath.maxSpeed = 2.3f;
                }else if (distanceToTarget > 20f)
                {
                    // If the distance is greater than the threshold, the enemy should run
                    isRunning = true;
                    aiPath.maxSpeed = runSpeed;
                }
                else
                {
                    // If the distance is less than or equal to the threshold, the enemy should walk
                    isRunning = false;
                    aiPath.maxSpeed = walkSpeed;
                }

                // Update the animator
                animator.SetBool("isRunning", isRunning);
            }   
            else
            {
                isCombatMode = false;
                aiDestination.target = null;
            }

            // Call to flip the direction if needed
            SetFacingDirection(aiPath.desiredVelocity);

        }   
        else
        {
            Debug.Log("No target found within range.");
        }
    }
    public void FixedUpdate(){
        OnMove();
        if(aiPath.desiredVelocity.x >= 0.01f){
            transform.localScale *= new Vector2(-1, 1);
        }
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
                // Set the "isMoving" parameter to false to stop the animation
                IsMoving = false;
                animator.SetBool("isMoving", false);
            }

        previousPosition = transform.position;
    }
    public void TakeDamage(float damage, GoalTypeEnum goal){
        List<QuestSO> quest = PlayerStats.GetInstance().activeQuests.ToList();
        foreach(QuestSO q in quest){
        if(q.goals[q.currentGoal].goalType == GoalTypeEnum.Damage && q.goals[q.currentGoal].targetCharacters.Contains(id)){
                QuestManager.GetInstance().UpdateDamageGoal(damage);
            } 
        }
        if(isDummy){
            foreach(QuestSO q in quest){
            if(q.goals[q.currentGoal].goalType == GoalTypeEnum.HitMelee && GoalTypeEnum.HitMelee == goal && q.goals[q.currentGoal].targetCharacters.Contains(id)){
                    QuestManager.GetInstance().UpdateHitMeleeGoal();
                }
            }
            foreach(QuestSO q in quest){
            if(q.goals[q.currentGoal].goalType == GoalTypeEnum.HitAny  && GoalTypeEnum.HitAny == goal && q.goals[q.currentGoal].targetCharacters.Contains(id)){
                    QuestManager.GetInstance().UpdateHitAnyGoal();
                } 
            }
            foreach(QuestSO q in quest){
            if(q.goals[q.currentGoal].goalType == GoalTypeEnum.HitJavelin && GoalTypeEnum.HitJavelin == goal && q.goals[q.currentGoal].targetCharacters.Contains(id)){
                    QuestManager.GetInstance().UpdateHitJavelinGoal();
                } 
            }
        }else{
            if(invulnerable) return;
            else{
                currentHP -= damage;
                if(currentHP <= 0){
                    isDead = true;
                    animator.Play("Death");
                    gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                    foreach(QuestSO q in quest){
                    if(q.goals[q.currentGoal].goalType == GoalTypeEnum.Kill && q.goals[q.currentGoal].targetCharacters.Contains(id)){
                            QuestManager.GetInstance().UpdateKillGoal();
                        } 
                    }
                    foreach(QuestSO q in quest){
                    if(q.goals[q.currentGoal].goalType == GoalTypeEnum.HitMelee && GoalTypeEnum.HitMelee == goal && q.goals[q.currentGoal].targetCharacters.Contains(id)){
                            QuestManager.GetInstance().UpdateHitMeleeGoal();
                        }
                    }
                    foreach(QuestSO q in quest){
                    if(q.goals[q.currentGoal].goalType == GoalTypeEnum.HitAny  && GoalTypeEnum.HitAny == goal && q.goals[q.currentGoal].targetCharacters.Contains(id)){
                            QuestManager.GetInstance().UpdateHitAnyGoal();
                        } 
                    }
                    foreach(QuestSO q in quest){
                    if(q.goals[q.currentGoal].goalType == GoalTypeEnum.HitJavelin && GoalTypeEnum.HitJavelin == goal && q.goals[q.currentGoal].targetCharacters.Contains(id)){
                            QuestManager.GetInstance().UpdateHitJavelinGoal();
                        } 
                    }
                }
            }

        }
        
    }
    public void TakeDamage(float damage){
            float newDamage = damage - armor;
            if(invulnerable){
                return;
            }else if(currentHP > 0 && isBlocking && !isDead){
                
                currentHP -= newDamage - (newDamage * 0.5f);
                if(currentHP <= 0){
                    isDead = true;
                    animator.Play("Death");
                    gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                }
            }else if(currentHP > 0 && !isDead){
                currentHP -= newDamage;
                if(currentHP <= 0){
                    isDead = true;
                    animator.Play("Death");
                    gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                }
            }
        
    }
    GameObject FindNearestTargetWithinRadius()
    {
        GameObject nearestTarget = null;
        float nearestDistance = detectionRadius;

        // Search for objects with tag "Player", "Ally", and "Enemy"
        List<GameObject> potentialTargets = new List<GameObject>();
        
        potentialTargets.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        potentialTargets.AddRange(GameObject.FindGameObjectsWithTag("Ally"));
        foreach (GameObject target in potentialTargets)
        {
            // Skip if the target is this enemy itself to avoid self-targeting
            if (target == gameObject) continue;

            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance <= nearestDistance)
            {
                nearestTarget = target;
                nearestDistance = distance;
            }
        }

        return nearestTarget;
    }
    void ApplyDamage(){
        Collider2D [] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        bool hasPlayer = false;
        foreach(Collider2D enemy in hitEnemies){

            if(enemy.GetComponent<Ally>() != null){
                enemiesInRange.Add(enemy.GetComponent<Ally>());
            }
            if(enemy.GetComponent<PlayerController>() != null){
                hasPlayer = true;
            }
        }
        if(!hasPlayer){
            Ally targetEnemy = enemiesInRange.OrderByDescending(enemy => enemy.currentHP).FirstOrDefault();
            targetEnemy.TakeDamage(damage);
        }else{
            PlayerController.GetInstance().TakeDamage(damage);
        }
        
    }
    public void MoveFront()
    {
        // Get the character's current position
        Vector2 cP = gameObject.transform.position;

        // Define the movement distance (adjust as needed)
        float moveDistance = 0.3f;

        // Calculate the new position based on the direction the character is facing
        Vector2 moveDirection = IsFacingRight ? Vector2.right : Vector2.left;

        Vector2 newPosition = cP + moveDirection * moveDistance;

        // Define the duration for the smooth movement (adjust as needed)
        float moveDuration = 0.2f;

        // Use DOTween to smoothly move the character to the new position
        gameObject.transform.DOMove(newPosition, moveDuration).SetEase(Ease.OutQuad);
        
    }
    private void RegenStamina(){
        if (!isBlocking && !isAttacking)
        {
            if(isCombatMode && IsMoving) {
                stamina = Mathf.Max(0, stamina - (staminaDepletionRate * 0.25f) * Time.deltaTime);
                return;
            }else if(isCombatMode){
                stamina = Mathf.Min(maxStamina, stamina + (staminaRegenRate - (staminaRegenRate * 0.50f)) * Time.deltaTime);
            }else{
                stamina = Mathf.Min(maxStamina, stamina + staminaRegenRate * Time.deltaTime);
            }
        }
    }
    public void MoveBackStun()
    {
        // Get the character's current position
        Vector2 cP = gameObject.transform.position;

        // Define the movement distance (adjust as needed)
        float moveDistance = -0.4f;

        // Calculate the new position based on the direction the character is facing
        Vector2 moveDirection = IsFacingRight ? Vector2.right : Vector2.left;

        Vector2 newPosition = cP + moveDirection * moveDistance;

        // Define the duration for the smooth movement (adjust as needed)
        float moveDuration = 0.2f;
        
        canMove = false;
        // Use DOTween to smoothly move the character to the new position
        gameObject.transform.DOMove(newPosition, moveDuration).SetEase(Ease.OutQuad).OnComplete(()=>{
            DOVirtual.DelayedCall(3f, () => 
            {
                canMove = true;
            });
        });
    }
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (isAttacking) return; // Don't flip while attacking

        // Face right if the movement is to the right (positive x) and currently facing left
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true; // Flip to face right
        }
        // Face left if the movement is to the left (negative x) and currently facing right
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false; // Flip to face left
        }
    }
    public void InitializeEquipments(){
        List<EquipmentSO> newList = new List<EquipmentSO>();
        foreach(int equipment in equipments){
            EquipmentSO equipmentSO = AccountManager.Instance.equipments.Where(eq => eq.equipmentId == equipment).FirstOrDefault();
            EquipmentSO copiedEquipmentSO = equipmentSO.CreateCopy(equipment, tier, equipmentLevel);
            newList.Add(copiedEquipmentSO);
        }
        equippedSOs = newList;
        int j = 0;
        float sumArmor = 0;
        damage = 1;
        mainHolster.sprite = equippedSOs[4].front;
        mainSlot.sprite = equippedSOs[4].front;
        shieldSlotFront.sprite = equippedSOs[5].front;
        shieldSlotBack.sprite = equippedSOs[5].back;
        foreach(EquipmentSO equipment in equippedSOs){
            if(equipment && equipment.equipmentType == EquipmentEnum.Armor){
                sumArmor += equipment.baseArmor;
            }
            if(equipment && equipment.equipmentType == EquipmentEnum.Weapon && (equipment.weaponType == WeaponType.Sword ||  equipment.weaponType == WeaponType.Spear) && j == 4){
                damage = equipment.baseDamage;
                attackRange = equipment.attackRange;
            }
            j++;
        }
        armor = sumArmor;
    }
    private void CalculateStatsForCurrentLevel()
    {
        // Calculate the stats based on the current level without incrementing it
        maxHP = 100f * Mathf.Pow(1.02f, level - 1);
        currentHP = maxHP;
        maxStamina = 70f * Mathf.Pow(1.03f, level - 1); // Assuming initial maxStamina is 70
        staminaRegenRate = 10f * Mathf.Pow(1.03f, level - 1); // Assuming initial staminaRegenRate is 10
    }
}
