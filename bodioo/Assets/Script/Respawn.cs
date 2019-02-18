using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    // Start is called before the first frame updat
    public Vector3 respawnPoint;
    private Transform rb;
    
    public Quaternion rotation;
    

    void Start()
    {
       


        
    }


    void Update()
    {
        
        
        if (transform.position.y < -5)
        {
            transform.SetPositionAndRotation(respawnPoint, rotation);

            gameObject.SetActive(false);
            gameObject.SetActive(true);
            
        }
    }
}
