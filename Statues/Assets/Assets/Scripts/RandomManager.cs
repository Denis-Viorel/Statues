using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomManager : MonoBehaviour
{
    [SerializeField] private float minx = -8f;
    [SerializeField] private float minz = 18f;
    [SerializeField] private float maxx = 12f;
    [SerializeField] private float maxz = 40f;
    [SerializeField] private float y = -2.73f;
    [SerializeField] private GameObject wolfTrap;
    [SerializeField] private GameObject spikeTrap;

    [SerializeField] private int nrTraps = 5;

    private List<Vector2> placedTraps = new List<Vector2>();

    [SerializeField] private float minxAgent = -11.4f;
    [SerializeField] private float minzAgent = 43f;
    [SerializeField] private float maxxAgent = 11.61f;
    [SerializeField] private float maxzAgent = 49.93f;
    [SerializeField] private float yAgent = -6.31f;
    [SerializeField] private GameObject Agent;

    private List<Vector2> placedAgents = new List<Vector2>();

    [SerializeField] private int nrAgents = 20;
    
    // Start is called before the first frame update
    void Start()
    {
        SettingTraps();
        SettingsAgents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SettingTraps(){
        for( int i=1; i<=nrTraps; i++ )
        {
            Vector2 randomPosition;
            bool validPosition;
            int maxAttempts = 100;
            int attempts = 0;

            do 
           {
                randomPosition = new Vector2(Random.Range(minx, maxx), Random.Range(minz, maxz));

                validPosition = true;
                for (int j = 0; j < placedTraps.Count; j++)
                {
                    if (Vector2.Distance(randomPosition, placedTraps[j]) < 4)
                    {
                        validPosition = false;
                        break;
                    }
                }

                attempts++;
                if (attempts >= maxAttempts)
                {
                    break;
                }
            } while (!validPosition);

            if (validPosition){
                Vector3 spawnPosition = new Vector3(randomPosition.x, y, randomPosition.y);
                GameObject selectedPrefab = (Random.value > 0.5f) ? spikeTrap : wolfTrap;
                Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                placedTraps.Add(randomPosition);
            }
            
        }
    }

    void SettingsAgents(){
        for( int i=1; i<=nrAgents; i++ )
        {
            Vector2 randomPosition;
            bool validPosition;
            int maxAttempts = 100;
            int attempts = 0;

            do 
           {
                randomPosition = new Vector2(Random.Range(minxAgent, maxxAgent), Random.Range(minzAgent, maxzAgent));

                validPosition = true;
                for (int j = 0; j < placedAgents.Count; j++)
                {
                    if (Vector2.Distance(randomPosition, placedAgents[j]) < 1)
                    {
                        validPosition = false;
                        break;
                    }
                }

                attempts++;
                if (attempts >= maxAttempts)
                {
                    break;
                }
            } while (!validPosition);

            if (validPosition){
                Vector3 spawnPosition = new Vector3(randomPosition.x, yAgent, randomPosition.y);
                GameObject selectedPrefab = Agent;
                Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                placedAgents.Add(randomPosition);
            }
            
        }
    }
}
