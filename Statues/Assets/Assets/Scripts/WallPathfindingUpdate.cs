using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPathfindingUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var guo = new GraphUpdateObject(GetComponent<Collider>().bounds);
        AstarPath.active.UpdateGraphs(guo);
    }

    private void OnDestroy()
    {
        var guo = new GraphUpdateObject(GetComponent<Collider>().bounds);
        AstarPath.active.UpdateGraphs(guo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
