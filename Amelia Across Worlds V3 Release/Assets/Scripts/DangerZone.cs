using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    public GameObject dangerVisuals;

    //DANGER ZONE DETECTION
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SmolAmeModelSeafoamboi")
        {
            dangerVisuals.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "SmolAmeModelSeafoamboi")
        {
            dangerVisuals.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SmolAmeModelSeafoamboi")
        {
            dangerVisuals.SetActive(false);
        }
    }
}
