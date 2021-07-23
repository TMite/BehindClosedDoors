using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Door : MonoBehaviour
{
    PlayerController isLock;
    [SerializeField] private int rayLength = 5;
    [SerializeField] private LayerMask lmi;
    [SerializeField] private string excludeLayerName = null;

    private myController raycastedObj;


    [SerializeField] private KeyCode openDoorKey = KeyCode.Mouse0;
    [SerializeField] private Image crosshair = null;
    private bool isCrosshairActive;
    private bool doOnce;
    private const string interactableTag = "InteractiveObj";
    private const string ChestTag = "Lock";
    public bool keyConfirmed = false;
    public bool alarmConfirmed = false;

    [SerializeField] public AudioClip Alarm;

    [Tooltip("The player head object ( usually the camera ) which turns with the mouse to look at things")]
    public Transform playerHead;
    [Tooltip("The function we run when we click on an object we detected")]
    public string activateFunction = "ActivateObject";
    [Tooltip("The button we need to press in order to activate the object we are aiming at")]
    public string activateButton = "Fire1";
    public Texture2D activateIcon;
    internal bool showIcon;

    public KeyUpdate KeyNum;
    public chestItem Key;

    public LPLockActivator Activator;


    private void FixedUpdate()
    {
        //Create RaycastHit
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | lmi.value;
        //Get Raycast position based on player head(camera) position
        if(Physics.Raycast(playerHead.position, playerHead.TransformDirection(Vector3.forward), out hit, rayLength, mask))
        {
            showIcon = true;
            //Detects objects with interactableTag
            if(hit.collider.CompareTag(interactableTag))
            {
                if(!doOnce)
                {
                    raycastedObj = hit.collider.gameObject.GetComponent<myController>();
                    CrosshairChange(true);
                }

                isCrosshairActive = true;
                doOnce = true;

                if(Input.GetKeyDown(openDoorKey))
                {
                    raycastedObj.PlayAnimation();
                }
            }
            //Detects objects with chestTag
            if (hit.collider.CompareTag(ChestTag))
            {
                Key = hit.collider.gameObject.GetComponent<chestItem>();
                CrosshairChange(true);
                isCrosshairActive = true;
                if (Input.GetKeyDown(openDoorKey) && !EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log(Key.isKey);
                    // Send a function to the object we are aiming at
                    hit.transform.gameObject.SendMessage(activateFunction, 0, SendMessageOptions.DontRequireReceiver);
                    showIcon = false;
                    
                    
                }
                if(Key.opening)
                {

                    if(Key.isKey)
                    {
                        KeyNum.keys++;
                        Key.opening = false;
                    }

                    if(!Key.isKey)
                    {
                        AudioSource.PlayClipAtPoint(Alarm, transform.position);
                        Key.opening = false;
                    }

                }
                
                

            }
            else if (showIcon == true)
            {
                showIcon = false;
            }

        }
        else
        {
            if(isCrosshairActive)
            {
                CrosshairChange(false);
                doOnce = false;
            }
        }
        
    }
    //Sets opening for chest when chest starts to open
    public void setOpening()
    {
        //Debug.Log("Opening");
        Key.opening = true;

    }

    //Changes color of crosshair based on object hit by ray
    void CrosshairChange(bool on)
    {
        if(on && !doOnce)
        {
            crosshair.color = Color.red;
        }
        else
        {
            crosshair.color = Color.white;
            isCrosshairActive = false;
        }
    }
}
