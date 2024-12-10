using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AddParticle : MonoBehaviourPunCallbacks
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
        //プレイヤー2が壊せる壁に触ったときは発生させない（「プレイヤー2以外」または「壊せる壁以外」なら発生）
        if (PhotonNetwork.NickName != "Player2" || this.gameObject.CompareTag("Movable") == false)
        {
            particle.Play();
        }
    }

    public void DeleteEffect(){
       particle.Stop();
    }
}
