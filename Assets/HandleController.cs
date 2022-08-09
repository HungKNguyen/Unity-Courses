using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleController : MonoBehaviour
{
    bool activated;
    Quaternion active = Quaternion.Euler(0,0,0);
    Quaternion inacive = Quaternion.Euler(0,50,0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activated) {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, active, 0.5f);
        } else {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, inacive, 0.5f);
        }
    }

    public void toggleActivated() {
        activated = !activated;
    }
}
