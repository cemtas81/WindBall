using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{
    private ParticleSystem _particleSystem; 
    // Start is called before the first frame update
    void Start()
    {
       _particleSystem=GetComponentInParent<ParticleSystem>(); 
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _particleSystem.Play();
        }
    }

}
