using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INY : MonoBehaviour
{
    public GameObject GO;
    // Start is called before the first frame update
    public void trigger()
    {
        if (GO.activeInHierarchy == true)
        {
            GO.SetActive(false);
        }
    }
}
