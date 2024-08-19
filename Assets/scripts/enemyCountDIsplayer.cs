using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyCountDisplayer : MonoBehaviour
{
    public Text enemyCountDisplayer;
    public int enemyCount;
    public Text won;
    

    private void Start()
    {
        UpdateEnemyCount();
    }

    // Method to find all enemies in the scene and update the count
    private void UpdateEnemyCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        enemyCount = enemies.Length;
        UpdateUI();
    }

    // Update UI to display the current enemy count
    public void UpdateUI()
    {
        if (enemyCount == 0)
        {
            won.text = "LEVEL CLEARED!! YOU WON";
            Invoke("sceneChanger", 2f);
        }
        enemyCountDisplayer.text = "No. of Enemies Left: " + enemyCount.ToString();
    }
    
    private void sceneChanger()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
}
