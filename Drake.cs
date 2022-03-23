using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drake : MonoBehaviour
{
    Animator A;
    [SerializeField] Transform destination;

    [SerializeField] bool patrolWait;
    [SerializeField] float waitTime = 3.0f;
    [SerializeField] float switchProb = .2f;
    [SerializeField] float spotRadius = 5.0f;
    [SerializeField] GameObject player;
    [SerializeField] List<Waypoints> patrolPoints;

    [SerializeField] AudioClip steps;
    private myController door;

    NavMeshAgent navMesh;
    int currentIndex;
    bool travelling;
    bool traveled;
    bool waiting;
    bool patrolForward = true;
    bool changed;
    bool spots;
    float waitTimer;

    IEnumerator delay(float duration)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("End");
    }
    IEnumerator waitDelay(float duration)
    {
        waiting = false;
        yield return new WaitForSeconds(duration);
        ChangePatrolPoint();
        SetDestination();

    }
    // Start is called before the first frame update
    void Start()
    {
        A = (Animator)GetComponent("Animator");
        navMesh = this.GetComponent<NavMeshAgent>();

        if (navMesh == null)
        {
            Debug.LogError("No Mesh" + gameObject.name);
        }
        else
        {
            if (patrolPoints != null && patrolPoints.Count >= 2)
            {
                currentIndex = 39;
                travelling = true;
                SetDestination();
            }
            else
            {
                Debug.Log("No Points");
            }
        }
    }

    private void SetDestination()
    {
        if (patrolPoints != null)
        {
            Vector3 target = patrolPoints[currentIndex].transform.position;
            navMesh.SetDestination(target);
            travelling = true;
            traveled = false;
            //Debug.Log("Travelling true");
            //A.SetBool("Walking", true);
            //A.SetBool("Run", false);
            //A.SetBool("Idle", false);
        }


    }
    private void chase()
    {
        Vector3 target = player.transform.position;
        navMesh.speed = 3;
        navMesh.SetDestination(target);
        travelling = true;
        //walking();
        //AudioSource.PlayClipAtPoint(steps, transform.position);
        A.SetBool("Walking", false);
        A.SetBool("Run", true);
        A.SetBool("Idle", false);
    }
    private void walking()
    {
        if (steps != null)
        {
            if (travelling && !spots)
            {
                AudioSource.PlayClipAtPoint(steps, transform.position);
                StartCoroutine(delay(1.0f));
            }
            if (travelling && spots)
            {
                AudioSource.PlayClipAtPoint(steps, transform.position);
                StartCoroutine(delay(.5f));
            }
        }

    }
    private void spotting()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < spotRadius)
        {
            spots = true;
        }
        else
        {
            spots = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        door = other.gameObject.GetComponent<myController>();
        if (other.gameObject.CompareTag("InteractiveObj"))
        {
            door.PlayAnimationM();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        door = other.gameObject.GetComponent<myController>();
        if (other.gameObject.CompareTag("InteractiveObj"))
        {
            door.PlayAnimationM();
        }
    }

    //Update is called once per frame
    void FixedUpdate()
    {
        spotting();
        if (!spots && !changed)
        {

            //Debug.Log(navMesh.remainingDistance);
            if (travelling && navMesh.pathEndPosition == transform.position)
            {
                Debug.Log(navMesh.pathEndPosition);
                traveled = true;
                travelling = false;
                A.SetBool("Walking", false);
                A.SetBool("Run", false);
                A.SetBool("Idle", true);
                Debug.Log("To Idle");

                if (patrolWait)
                {
                    waiting = true;
                    waitTimer = 0f;
                }
                else
                {
                    ChangePatrolPoint();
                    SetDestination();
                }
            }
            if (traveled && navMesh.remainingDistance <= 1.0f)
            {
                //travelling = true;
                //Debug.Log("Travelling 2" + travelling);
                //StartCoroutine(waitDelay(3.0f));
                //print(waitTimer);
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTime)
                {
                    waiting = false;
                    ChangePatrolPoint();
                    SetDestination();
                    //Debug.Log(currentIndex);

                }
            }

            //Debug.Log(navMesh.remainingDistance);

        }
        if (!traveled && changed)
        {
            //Debug.Log(changed);
            A.SetBool("Idle", false);
            A.SetBool("Run", false);
            A.SetBool("Walking", true);
        }
        if (changed && navMesh.remainingDistance >= 10.0f)
        {
            changed = false;
        }

        if (spots)
        {
            chase();
        }
    }
    //void FixedUpdate()
    //{
    //    spotting();
    //    if (!spots)
    //    {
    //        if (travelling && navMesh.remainingDistance <= 1.0f)
    //        {
    //            travelling = false;
    //            Debug.Log("Travelling true");

    //            if (patrolWait)
    //            {
    //                waiting = true;
    //                waitTimer = 0f;
    //                Debug.Log("patrol true");
    //                Debug.Log(waitTimer);
    //            }
    //            else
    //            {
    //                ChangePatrolPoint();
    //                SetDestination();
    //                Debug.Log("patrol false");
    //            }
    //        }
    //        if (waiting)
    //        {
    //            Debug.Log("waiting true");
    //            travelling = false;
    //            //print(waitTimer);
    //            waitTimer += Time.deltaTime;
    //            if (waitTimer >= waitTime)
    //            {
    //                Debug.Log("waittimer >");
    //                waiting = false;
    //                ChangePatrolPoint();
    //                SetDestination();
    //                print(currentIndex);

    //            }
    //        }
    //    }
    //    if (spots)
    //    {
    //        chase();
    //    }
    //}

    private void ChangePatrolPoint()
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) <= switchProb)
        {
            patrolForward = !patrolForward;
        }

        if (patrolForward)
        {
            //currentIndex = (currentIndex + 1) % patrolPoints.Count;
            currentIndex = UnityEngine.Random.Range(0, 46);
            changed = true;
            //print(currentIndex);
        }
        else
        {
            if (--currentIndex < 0)
            {
                currentIndex = patrolPoints.Count - 1;
            }
            //currentIndex = (currentIndex + 1) % patrolPoints.Count;
            //currentIndex = UnityEngine.Random.Range(0, 46);
        }
    }
}
