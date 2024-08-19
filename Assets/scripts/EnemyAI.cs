using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public Animator animator;

    //sound management
    [SerializeField] AudioSource enemyShootSound;

    public EnemyCountDisplayer enemyCountDisplayer;

    //enemy health system
    private int maxHealth =5;
    private int currentHealth;
    public healthDisplay enemyHealthBar;

    //enemy attack instantiation
    [SerializeField] private float spawnHeight = 1f;
    [SerializeField] private float spawnDistance = 1f;
    //[SerializeField] private float speed = 20f;

    //patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks = 5f;
    bool alreadyAttacked;
    public GameObject enemyChargeBall;
    private float speed = 20f;


    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemyCountDisplayer = FindObjectOfType<EnemyCountDisplayer>();
        currentHealth = maxHealth;
        enemyHealthBar.setMaxHealth(maxHealth);
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();

        }

        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();

        }
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            animator.SetFloat("Speed", 1.5f);
            animator.SetFloat("MotionSpeed", 1.5f);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //WalkPoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate a random point in the range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        { walkPointSet = true; }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetFloat("Speed", 2);
        animator.SetFloat("MotionSpeed", 2);
    }

    private void AttackPlayer()
    {
        //making sure that the player doesn't move
        agent.SetDestination(transform.position);
        animator.SetFloat("Speed", 0);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // my attacking code
            Debug.Log("HI");
            Vector3 spawnPosition = transform.position + transform.forward * spawnDistance + Vector3.up * spawnHeight;

            var chargeSphere = Instantiate(enemyChargeBall, spawnPosition, Quaternion.identity);
            chargeSphere.GetComponent<Rigidbody>().velocity = (player.transform.position + Vector3.up * spawnHeight - spawnPosition).normalized * speed;
            Destroy(chargeSphere, 5);

            enemyShootSound.Play();
            animator.SetTrigger("shooting");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void enemyDeath()
    {
        animator.SetTrigger("death");
        Destroy(gameObject,2.5f);
        enemyCountDisplayer.enemyCount -= 1;
        enemyCountDisplayer.UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball"))
        {
            currentHealth = currentHealth - 2;
            enemyHealthBar.setHealth(currentHealth);

            if (currentHealth <0)
            {
                enemyDeath();
            }
        }
    }
}
