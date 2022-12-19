using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerController : MonoBehaviour
{
    [SerializeField] private Transform _PlayerTransform;
    [SerializeField] private NavMeshAgent _Agent;
    [SerializeField] private float _Distance;
    [SerializeField] private float _ShootingDistance;
    public float _RandomX;
    private bool _IsChasing;
    private Vector3 _ChaseDestination;
    private void Awake()
    {
        _RandomX = Random.RandomRange(-1.2f, 3.5f);
    }
    private void OnEnable()
    {
        GameActions.instance._StartShootAction += SetShootingDistance;
    }
    private void OnDisable()
    {
        GameActions.instance._StartShootAction -= SetShootingDistance;
    }
    private void SetShootingDistance()
    {
        _Distance = _ShootingDistance;
    }
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
        {
            _ChaseDestination = new Vector3(_RandomX, _PlayerTransform.position.y, _PlayerTransform.position.z - _Distance);
            _Agent.SetDestination(_ChaseDestination);
        }
            
    }
}
