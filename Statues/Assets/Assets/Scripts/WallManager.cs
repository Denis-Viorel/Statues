using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject wall;
    LayerMask layerMask;

    private List<GameObject> spawnedWalls = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                GameObject targetHit = hit.transform.gameObject;
                Vector3 hitPos = hit.point;

                if (targetHit.tag.Equals("Wall"))
                {
                    Destroy(targetHit);
                }
                else if (targetHit != null)
                {
                    spawnedWalls.Add(Instantiate(wall, hitPos, Quaternion.identity));
                }
            }
        }
    }

    public void ClearWalls()
    {
        foreach (GameObject wall in spawnedWalls)
        {
            Destroy(wall);
        }
        spawnedWalls.Clear();
    }
}
