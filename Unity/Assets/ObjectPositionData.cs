using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ObjectPositionData", menuName = "ScriptableObject/ObjectPositionData")]
public class ObjectPositionData : ScriptableObject
{
    public List<Vector3> positions = new List<Vector3>();
    public bool isUpdating = false;

    public void AddPosition(Vector3 position){
        positions.Add(position);
    }

    public Vector3 GetPosition(int index){
        //return positions[positions.Count - 1 - index];
        return positions[index];
    }

    public void ClearPositions(){
        positions.Clear();
    }
}