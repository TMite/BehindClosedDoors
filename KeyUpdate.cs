using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyUpdate : MonoBehaviour
{
    private TextMeshProUGUI Key;
    public int keys;

    void Awake()
    {
        Key = GetComponent<TextMeshProUGUI>();
        keys = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Key.text = "x" + keys.ToString();
        //keys++;
    }
}
