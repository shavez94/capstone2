using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraon : MonoBehaviour
{
    public GameObject cameraoff;
    public GameObject cameraplayer;
    public GameObject player;
    public GameObject gamecanva;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void on()
    {
        Debug.Log("1");
        cameraplayer.SetActive(true);
        cameraoff.SetActive(false);
        gamecanva.SetActive(true);

    }
    public void playerr()
    {
        player.SetActive(true);
    }
}
