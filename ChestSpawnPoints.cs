using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnPoints : MonoBehaviour
{
    [SerializeField]
    protected float debugDrawRadius = 1.0f;

    public Transform target;
    float speed = 10.0f;

    void Start()
    {
        transform.LookAt(target);
        Debug.DrawLine(transform.position, target.position, Color.red);
    }
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
    }
}
