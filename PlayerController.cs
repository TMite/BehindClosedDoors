using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] public bool lockCursor = true;
    [SerializeField] public float speed = 5.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] float sprint = 100.0f;
    [SerializeField] [Range(0.0f, 0.5f)] float smoothTime = 0.25f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.05f;
    [SerializeField] AudioClip Ambient;

    [SerializeField] List<ChestSpawnPoints> chestPoints;
    [SerializeField] public int chestNum = 37;

    [SerializeField] GameObject Key;
    [SerializeField] GameObject Alarm;

    public chestItem Item;
    public Inventory inventory;

    public ChestSpawnPoints chestRot;
    Vector3 currentDir = Vector3.zero;
    Vector3 currentDirVelocity = Vector3.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    CharacterController controller = null;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    IEnumerator coroutine()
    {
        yield return new WaitForSeconds(3);
        spawnChest();
    }
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //spawnChest();
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        StartCoroutine(coroutine()); 
    }

    // Update is called once per frame
    void Update()
    {

        UpdateMouseLook();
        UpdateMove();
        if (!lockCursor)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
        
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMove()
    {
        //Debug.Log(speed);

        Vector3 targetDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        currentDir = Vector3.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, smoothTime);

        if (controller.isGrounded)
        {
            velocityY = 0.0f;
        }
        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = ((transform.forward * currentDir.z) + (transform.right * currentDir.x)) * speed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
        if(Input.GetKey(KeyCode.LeftShift) && sprint > 0)
        {
           
            velocity = ((transform.forward * currentDir.z) + (transform.right * currentDir.x)) * (speed * 1.5f) + Vector3.up * velocityY;
            controller.Move(velocity * Time.deltaTime);
            sprint -= 5.0f;
            Debug.Log(sprint);
        }
        if(!Input.GetKey(KeyCode.LeftShift) && sprint <= 100.0f)
        {
            sprint += 1.0f;
            Debug.Log(sprint);
        }
            //float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            //float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

            //transform.Translate(x, 0, z);
        
    }

    public GameObject chest;
    void spawnChest()
    {
        for(int i = 0; i < 6; i++)
        {
            if(i < 3) { Item.isKey = true; }
            if (i > 2) { Item.isKey = false; }
            if (Item.isKey == true)
            {

                Debug.Log("Key");
                Item.item = Key;
                int randPos = Random.Range(0, chestNum);
                Vector3 chestSpawn = chestPoints[randPos].transform.position;
                float chestSpawnRot = chestPoints[randPos].transform.eulerAngles.y;
                //chest = new GameObject("Chest");
                GameObject chestInst;               
                //Debug.Log(chestSpawnRot);
                chestInst = Instantiate(chest, chestSpawn, Quaternion.Euler(new Vector3(0f, chestSpawnRot, 0f)));
                chestInst.GetComponentInChildren<chestItem>().isKey = true;
                chestNum--;
                chestPoints.RemoveAt(randPos);
                
            }
            if (Item.isKey == false)
            {
                Debug.Log("Alarm");
                Item.item = Alarm;
                int randPos = Random.Range(0, chestNum);
                Vector3 chestSpawn = chestPoints[randPos].transform.position;
                float chestSpawnRot = chestPoints[randPos].transform.eulerAngles.y;
                //chest = new GameObject("Chest");
                GameObject chestInst;
                //Debug.Log(chestSpawnRot);
                chestInst = Instantiate(chest, chestSpawn, Quaternion.Euler(new Vector3(0f, chestSpawnRot, 0f)));
                chestInst.GetComponentInChildren<chestItem>().isKey = false;
                chestNum--;
                chestPoints.RemoveAt(randPos);
                
            }




        }
    }
}
