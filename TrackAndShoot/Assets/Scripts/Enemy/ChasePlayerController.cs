using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerController : MonoBehaviour
{
    [SerializeField] private Transform _PlayerTransform;
    [SerializeField] private NavMeshAgent _Agent;
    private bool _IsChasing;
    private void Update()
    {
        Chase();
    }
    private void Chase()
    {
        if (!_IsChasing)
        {
            if(transform.position.z < _PlayerTransform.position.z)
            {
                _IsChasing = true;
                GetComponent<Animator>().SetBool("Run", true);
            }
        }
        else
            _Agent.SetDestination(_PlayerTransform.position);
            
    }
}
