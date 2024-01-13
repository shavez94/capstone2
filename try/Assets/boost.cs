using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boost : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            FindObjectOfType<carcontoller>().booston();
        }
        if (other.gameObject.tag == "ai")
        {
            aicontroller aicontroller = other.GetComponent<aicontroller>();
            aicontroller.booston();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            FindObjectOfType<carcontoller>().bootoff();
        }
        else if (other.gameObject.tag == "ai")
        {
            aicontroller aicontroller = other.GetComponent<aicontroller>();
            aicontroller.bootoff();
        }
    }
}
