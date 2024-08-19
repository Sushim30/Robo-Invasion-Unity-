using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPowerup : MonoBehaviour
{
    public GameObject healthItem;
    Vector3 RotationVector = new Vector3(0,5,0);
    public PlayerAttacking playerAttacking;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("enemy") )
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        healthItem.transform.Rotate(RotationVector);

    }
}
