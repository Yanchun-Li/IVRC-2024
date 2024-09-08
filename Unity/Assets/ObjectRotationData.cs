using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "ObjectRotationData", menuName = "ScriptableObject/ObjectRotationData")]
public class ObjectRotationData : ScriptableObject
{
    public List<Quaternion> Rotations = new List<Quaternion>();
    public bool isUpdating = false;

    public void AddRotation(Quaternion Rotation){
        Rotations.Add(Rotation);
    }

    public Quaternion GetRotation(int index){
        //return positions[positions.Count - 1 - index];
        return Rotations[index];
    }

    public void ClearRotations(){
        Rotations.Clear();
    }
    public int LengthRotations(){
        return Rotations.Count();
    }
}