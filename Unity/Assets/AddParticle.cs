using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEffect(){
        particle.Play();
    }

    public void DeleteEffect(){
       particle.Stop();
    }
}
