using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleController : MonoBehaviour
{
    ToggleParticle toggleParticle;
    // Start is called before the first frame update
    void Start()
    {
        toggleParticle = GetComponent<ToggleParticle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Candle") {
            toggleParticle.Play();
        }
    }
}
