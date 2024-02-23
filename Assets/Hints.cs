using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hints : MonoBehaviour
{
    public GameObject GO;

    public void trigger()
    {
        if (GO.activeInHierarchy == false)
        {
            GO.SetActive(true);
        }
    }
}
