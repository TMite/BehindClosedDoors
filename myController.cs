using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myController : MonoBehaviour
{
    //Animator for the door
    private Animator doorA;

    //By default all doors are closed
    private bool doorOpen = false;

    //On Awake Gets the Animator component of the door
    private void Awake()
    {
        doorA = gameObject.GetComponent<Animator>();
    }

    //Called to play the open and close animation of the door as player
    public void PlayAnimation()
    {
        if(!doorOpen)
        {
            doorA.Play("open_close", 0, 0.0f);
            doorOpen = true;
        }
        else
        {
            doorA.Play("close", 0, 0.0f);
            doorOpen = false;
        }

    }
    //Called to open to when monster comes in contact with with door
    public void PlayAnimationM()
    {
        if (!doorOpen)
        {
            doorA.Play("open_close", 0, 0.0f);
            doorOpen = true;
        }
    }
}
