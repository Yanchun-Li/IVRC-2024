using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioControllerforPlayer2 : MonoBehaviour
{
     [SerializeField] private List<AudioSource> myaudioSources = new List<AudioSource>();
     [SerializeField] private List<AudioSource> otheraudioSources = new List<AudioSource>();
     [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();
    //[SerializeField] private AudioSource audioBreath;
    [SerializeField] private string myaudioSourceTag;
    [SerializeField] private string otheraudioSourceTag;
    private List<Vector3> audioPositionList = new List<Vector3>();
    private Vector3 myPosition;
    private Quaternion myRotation;
    private List<bool> audiostoplist= new List<bool>();
    [SerializeField] private GameObject avatar;

    private Vector3 Position;
    private bool accessOtherScene;
    private Coroutine getPosition;
    
    private Vector3 OriginalPosition;

    public ObjectPositionData otherpositionData;
    public ObjectRotationData otherrotationData;
    public ObjectPositionData mypositionData;
    public ObjectRotationData myrotationData;
    public int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        accessOtherScene = false;
        FindAudioSources();
        mypositionData.ClearPositions();
        myrotationData.ClearRotations();
        foreach (var audio in myaudioSources)
        {
            if (audio != null)
            {
                audiostoplist.Add(false);
            }
        }
        //audioBreath.Stop();
        myPosition = this.transform.position;
        OriginalPosition = this.transform.position;
    }

    private void FindAudioSources()
    {
        myaudioSources = new List<AudioSource>();
        otheraudioSources = new List<AudioSource>();
        // タグ付けされた全ての GameObjects を見つける
        GameObject[] audioSourceObjects = GameObject.FindGameObjectsWithTag(myaudioSourceTag);
        GameObject[] otheraudioSourceObjects = GameObject.FindGameObjectsWithTag(otheraudioSourceTag);

        // 見つかった各 GameObject から AudioSource コンポーネントを取得し、リストに追加
        foreach (GameObject obj in audioSourceObjects)
        {
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                myaudioSources.Add(audioSource);
            }
        }
        foreach (GameObject obj in otheraudioSourceObjects)
        {
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                otheraudioSources.Add(audioSource);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        audioSources = myaudioSources;

        audioPositionList.Clear();
        foreach (var audio in audioSources)
        {
            if (audio != null)
            {
                audioPositionList.Add(audio.transform.position);
            }
            //audio.Play();
        }
        myPosition = this.transform.position;

        List<int> indexlist=calcDistance(audioPositionList, myPosition);
        audiomute(indexlist,audioSources);
        
        this.transform.position = myPosition;
        this.transform.rotation = myRotation;
        //Debug.Log($"my position is {this.transform.position}");
        //Debug.Log($"my position is {this.transform.localPosition} and avatar position is {avatar.transform.position}");
        //Debug.Log($"my position is {this.transform.localRotation} and avatar position is {avatar.transform.rotation}");
    }

    private List<int> calcDistance(List<Vector3> audio,Vector3 origin)
    {
        var list = new List<float>();
        var indexlist = new List<int>();
        for(int i=0;i<9;i++){
            float dis = Vector3.Distance(audio[i],origin);
            list.Add(dis);
        }
        
        //Keyが距離でValueが音源番号
        var sorted = list.Select((x,i) => new KeyValuePair<float,int>(x,i)).OrderBy(x => x.Key);

        foreach(KeyValuePair<float,int> item in sorted)
        {
            //Debug.Log(item.Key+" "+item.Value);
            indexlist.Add(item.Value);
        } 
        return indexlist;
    }

    private void audiomute(List<int> indexlist, List<AudioSource> audioSources){
        //近く2つは音を鳴らし、7つは音を止める（他は関与しない）
        for(int i =0;i<2;i++){
            if (audiostoplist[indexlist[i]]){
                audioSources[indexlist[i]].Play();
            }
            audiostoplist[indexlist[i]] = false;
        }
        for(int i=2;i<9;i++){
            audioSources[indexlist[i]].Stop();
            audiostoplist[indexlist[i]] = true;
        }
        //Debug.Log("audio"+indexlist[0]+" "+"and"+" "+"audio"+indexlist[1]+" "+"play");
    }
}
