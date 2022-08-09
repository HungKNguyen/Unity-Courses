using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    AudioSource audioSource;
    Rigidbody rigidbody;
    [SerializeField] AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter()
    {
        audioSource.PlayOneShot(clip, rigidbody.velocity.magnitude * 0.5f);
    }
}
