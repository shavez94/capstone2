using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelcomplete : MonoBehaviour {

    bool onoff = false;

    private void Awake()
    {
        Invoke("gone", 50f);
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("player"))
        {
            FindObjectOfType<gamemanagar>().levelreached(this.transform.parent);
        }
        else
        {
            if (onoff==true)
            {
                aiinput aiinput = other.GetComponent<aiinput>();
                aiinput.reached();
            }

        }
    }
    void gone()
    {
        onoff = true;
    }
}

