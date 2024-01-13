using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class countdown : MonoBehaviour
{
    public GameObject player;
    public GameObject canva_game;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void gamestart()
    {
        PlayerPrefs.SetInt("timeleft", 1);
        player.SetActive(true);
        canva_game.SetActive(true);
        FindObjectOfType<inputmanager>().go();
    }
}
