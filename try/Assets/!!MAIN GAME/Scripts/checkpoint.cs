using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    public GameObject beam;
    public Material red;
    public Material orange_beam;
    private MeshRenderer MeshRenderer, MeshRenderer1;

    private void Awake()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
        MeshRenderer1=beam.GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="player")
        {
            FindObjectOfType<gamemanagar>().checkpoint_Reached(gameObject);
            MeshRenderer.material = red;
            MeshRenderer1.material = orange_beam;
        }
    }

}
