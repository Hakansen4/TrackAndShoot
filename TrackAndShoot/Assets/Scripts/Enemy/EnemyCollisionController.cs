using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionController : MonoBehaviour,IDie
{
    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bumper"))
            Die();
    }
    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(1.5f);
    }
}