using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour,IDie
{
    [SerializeField] private int _PlayerHealth;

    public void Die()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            collision.gameObject.SetActive(false);
            _PlayerHealth--;
            if (_PlayerHealth <= 0)
                Die();
        }
    }
}
