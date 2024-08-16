using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioController : MonoBehaviour
{
     [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();
    //[SerializeField] private AudioSource audioBreath;
    [SerializeField] private string audioSourceTag;
    private List<Vector3> audioPositionList = new List<Vector3>();
    private Vector3 myPosition;
    private List<bool> audiostoplist= new List<bool>();
    [SerializeField] private GameObject avatar;

    private Vector3 Position;
    private bool accessOtherScene = false;
    private Coroutine getPosition;
    
    private Vector3 OriginalPosition;

    public ObjectPositionData positionData;
    public ObjectRotationData rotationData;
    public int currentIndex = 0;
    

    // Start is called before the first frame update
    void Start()
    {
         FindAudioSources();
        positionData.ClearPositions();
        rotationData.ClearRotations();
        foreach (var audio in audioSources)
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
        // タグ付けされた全ての GameObjects を見つける
        GameObject[] audioSourceObjects = GameObject.FindGameObjectsWithTag(audioSourceTag);

        // 見つかった各 GameObject から AudioSource コンポーネントを取得し、リストに追加
        foreach (GameObject obj in audioSourceObjects)
        {
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSources.Add(audioSource);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        audioPositionList.Clear();
        foreach (var audio in audioSources)
        {
            if (audio != null)
            {
                audioPositionList.Add(audio.transform.position);
            }
            //audio.Play();
        }

        if (Input.GetKey(KeyCode.P)){

            if (accessOtherScene && getPosition != null){
                StopCoroutine(getPosition);
            }

            getPosition = StartCoroutine(Duration(5.0f));
        }

        if(!accessOtherScene){
            //myPosition = OriginalPosition;
            this.transform.position = avatar.transform.position;
            myPosition = this.transform.position;
            //Debug.Log("in this case");
        }
        List<int> indexlist=calcDistance(audioPositionList, myPosition);
        audiomute(indexlist,audioSources);
        //Debug.Log(myPosition);
        //myPosition = this.transform.position;
        this.transform.position=myPosition;
        //Debug.Log($"my position is {this.transform.position}");
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

    private IEnumerator Duration(float duration){
        accessOtherScene = true;
        float startTime = Time.time;
        //audioBreath.Play();

        while (Time.time - startTime < duration){
            //Position = SavePositionCopy.positioncopy;
            //InvokeRepeating("ReadNextPosition",0f,0.1f);
            Vector3 Position = positionData.GetPosition(currentIndex);
            Quaternion Rotation = rotationData.GetRotation(currentIndex);
            //audioBreath.transform.position = Position;
            //Debug.Log($"position at index {currentIndex}: {Position}");
            currentIndex++;
            //Position = position;
            myPosition = Position;
            //Debug.Log($"myposition is {myPosition}");
            //Debug.Log("Get Position is "+Position.x+" "+Position.y+" "+Position.z);
            if (currentIndex>= positionData.positions.Count){
                currentIndex = 0;
                Debug.Log("currentIndex is clear");
            }
            yield return new WaitForSeconds(0.2f);
        }

        //audioBreath.Stop();
        this.transform.position = avatar.transform.position;
        accessOtherScene = false;
        //Debug.Log("stop access");
    }

    void ReadNextPosition(){
        Vector3 Position = positionData.GetPosition(currentIndex);
        //Debug.Log($"position at index {currentIndex}: {position}");
        Quaternion Rotation = rotationData.GetRotation(currentIndex);
        currentIndex++;

        if (currentIndex>= positionData.positions.Count){
            currentIndex = 0;
            Debug.Log("currentIndex is clear");
        }
    }
}
