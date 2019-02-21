using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    // Start is called before the first frame updat
    public Vector3 respawnPoint;
    private Transform rb;
    
    public Quaternion rotation;
    public GameObject o;

    void Start()
    {
       
        

        
    }
    IEnumerator proccesTask()
    {
        yield return new WaitForSeconds(10);
        Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(), o.GetComponent<BoxCollider>(), false);
    }

    void Update()
    {
        
        
        if (transform.position.y < -5)
        {
            
            transform.SetPositionAndRotation(respawnPoint, rotation);
            Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(),o.GetComponent<BoxCollider>());
            
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            StartCoroutine(proccesTask());
           

        }
    }

}
