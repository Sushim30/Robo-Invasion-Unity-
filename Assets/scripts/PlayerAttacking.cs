using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttacking : MonoBehaviour
{
    [SerializeField] private GameObject powerSphere;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource healSound;
    [SerializeField] private AudioSource deathSound;

    //accessing the other scripts 
    public ThirdPersonController personController;
    public int temporarySpeed = 10;

    //attacking element spawn
    public float spawnDistance=1f;
    public float spawnHeight=1f;
    public Transform cam;

    //health system
    public healthDisplay healthDisplay;
    public int maxHealth=100;
    public int currentHealth;

    private Vector3 spawnPosition;
    private float speed = 20f;
    private bool ballPresence = false;

    public Animator anim;

    public EnemyCountDisplayer enemyCountDisplayer;

    public void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthDisplay.setMaxHealth(maxHealth);
    }
    public Color raycolor = Color.red;
    public void Attack()
    {
        if (!ballPresence /*&& !gameIsPaused*/)
        {
            anim.SetTrigger("shooting");
            spawnPosition = transform.position + transform.forward * spawnDistance + Vector3.up * spawnHeight;
            var chargeSphere = Instantiate(powerSphere, spawnPosition, Quaternion.identity);
            chargeSphere.GetComponent<Rigidbody>().velocity = cam.transform.forward * speed;
            ballPresence = true;
            Timer timer = new Timer(boolChanger, null, 3000, Timeout.Infinite);
            Destroy(chargeSphere, 3);
            shootSound.Play();
            
        }
    }

    
    private void boolChanger(object state)
    {
        ballPresence = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball"))
        {
            currentHealth -= 15;
            healthDisplay.setHealth(currentHealth);
            deathSound.Play();
            if (currentHealth < 0) death();
        }

        if (other.CompareTag("health") && currentHealth < 100)
        {
            healSound.Play();
            currentHealth += 45;
            if (currentHealth > 100)
            {
                currentHealth = 100;
                healthDisplay.setHealth(currentHealth);
            }
        }
        if (other.CompareTag("sprint"))
        {
            personController.SprintSpeed += temporarySpeed;
            personController.MoveSpeed += temporarySpeed;
            Invoke("normalSpeed", 5f);
        }
    }

    public void normalSpeed()
    {
        personController.SprintSpeed -= temporarySpeed;
        personController.MoveSpeed -= temporarySpeed;
    }

    private void death()
    {
        anim.SetTrigger("death");
        Invoke("sceneChanger", 1f);
        deathSound.Play();
    }

    private void sceneChanger()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !ballPresence)
        {
            anim.SetTrigger("shooting");
            Invoke("Attack", 0.5f);
        }
        spawnPosition = transform.position + transform.forward * spawnDistance + Vector3.up * spawnHeight;
        Debug.DrawRay(spawnPosition, cam.transform.forward *700f, raycolor);
    }
}
