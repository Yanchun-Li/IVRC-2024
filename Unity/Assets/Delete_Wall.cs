using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete_Wall : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticle;
    private Transform particleTransform;
    private Vector3 particlePosition;
    // Start is called before the first frame update
    void Start()
    {
        particlePosition = this.gameObject.transform.position; 
        particleTransform = this.gameObject.transform;  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)){
            //StartCoroutine(Destruction(this.gameObject));
        }
    }

    public void TutorialRemove(){
        StartCoroutine(Destruction(gameObject));
    }

    private IEnumerator Destruction(GameObject wall){
        var particle = Instantiate(explosionParticle, particleTransform);
        particle.transform.position = this.gameObject.transform.position;
        particle.Play();
        yield return new WaitForSeconds(1.2f);
        wall.SetActive(false);
    }
}
