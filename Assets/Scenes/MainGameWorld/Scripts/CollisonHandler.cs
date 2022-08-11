using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisonHandler : MonoBehaviour
{
    [SerializeField]
    private Text popup;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            Debug.Log("Bumped into building");
            
        }
    }

    
}
