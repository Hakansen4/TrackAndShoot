using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class EnemyCollisionController : MonoBehaviour,IDie
{
    [SerializeField] private SkinnedMeshRenderer _Mesh;
    [SerializeField] private Material _DeadMaterial;
    public void Die()
    {
        StartCoroutine(WaitForDestroy());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bumper"))
            Die();
    }
    private IEnumerator WaitForDestroy()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        ChangeColor();
        transform.DOJump((transform.position + 5f * Vector3.forward), 0.6f, 1, 1f);
        GetComponent<Animator>().SetTrigger("Death");
        yield return new WaitForSeconds(1);
        GameActions.instance._EarnMoney?.Invoke();
        gameObject.SetActive(false);
    }
    private void ChangeColor()
    {
        Material[] _Materials = _Mesh.sharedMaterials;
        _Materials[0] = _DeadMaterial;
        _Materials[1] = _DeadMaterial;
        _Mesh.sharedMaterials = _Materials;
    }
}