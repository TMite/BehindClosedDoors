using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestRotation : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        Debug.DrawLine(transform.position, target.position, Color.red);
    }
}
