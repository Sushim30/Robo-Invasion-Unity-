using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollision : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }


}
