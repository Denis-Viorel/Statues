using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WallManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject wall;
    LayerMask layerMask;
    [SerializeField] public int nrWalls;
    private int initNrWalls = 3;
    [SerializeField] private int currentWalls = 0;

    public static event Action OnMouseClick;
    [SerializeField] private TextMeshProUGUI wallText;

    private List<GameObject> spawnedWalls = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        nrWalls = initNrWalls;
        wallText ??= GameObject.Find("InfoTextWall")?.GetComponent<TextMeshProUGUI>();
        layerMask = LayerMask.GetMask("Ground");
        TextWall();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Time.timeScale == 1)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                GameObject targetHit = hit.transform.gameObject;
                Vector3 hitPos = hit.point;

                if (targetHit.tag.Equals("Wall"))
                {
                    currentWalls--;
                    OnMouseClick?.Invoke();
                    Destroy(targetHit);
                }
                else if ( targetHit != null && currentWalls < nrWalls )
                {
                    currentWalls++;
                    OnMouseClick?.Invoke();
                    spawnedWalls.Add(Instantiate(wall, hitPos, Quaternion.identity));
                }
            }
        }
    }

    public void ClearWalls()
    {
        currentWalls = 0;
        TextWall();
        foreach (GameObject wall in spawnedWalls)
        {
            Destroy(wall);
        }
        spawnedWalls.Clear();
    }

    private void TextWall()
    {
        wallText.text = currentWalls + "/" + nrWalls;
    }

    private void OnEnable()
    {
        OnMouseClick += TextWall;
    }

    private void OnDisable()
    {
        OnMouseClick -= TextWall;
    }

    public static void TriggerClickEvent()
    {
        OnMouseClick?.Invoke();
    }


    public void setNrWalls(int numberWalls)
    {
        nrWalls = initNrWalls + numberWalls;
    }
}
