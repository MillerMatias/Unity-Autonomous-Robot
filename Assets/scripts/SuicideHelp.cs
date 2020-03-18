using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideHelp : MonoBehaviour {

    

    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		


	}


    void OnCollisionEnter(Collision collisionobject)
    {
        if (collisionobject.gameObject.tag == "Target")
        {
            Destroy(gameObject);

        }
    }


}
