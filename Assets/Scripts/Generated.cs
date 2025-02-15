using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generated : MonoBehaviour
{
    public GameObject[] platformPrefabsPlats;
    public GameObject[] platformPrefabsAvantTroue;
    public GameObject[] platformPrefabsApresTroue;
    public GameObject platformPrefabsMonter;
    public GameObject platformPrefabsDescente;
    public GameObject Trou;
    
    public GameObject Strawberry;
    public float ChanceSpawnStrawberry = 1f; 

    private Vector3 lastPosition;
    private GameObject lastPlatform;
    private Transform frontParent;
    private bool devoirDescendre;
    private bool trouCree;
    private bool devoirApresTrou;

    private int maxRange = 20;
    private float nextIncreaseTime = 0f;
    public float positionX = 11.8F;

    void Start()
    {
        frontParent = GameObject.Find("Front")?.transform;
        if (frontParent == null)
        {
            Debug.LogError("Le GameObject 'Front' est introuvable !");
            return;
        }

        lastPosition = new Vector3(0, -2.5f, 0);
        StartCoroutine(SpawnObjectIn0Seconds());
    }

    IEnumerator SpawnObjectIn0Seconds()
    {
        while (true)
        {
            GenerateStrawberry();
             GeneratePlatforms();
            
            if (Time.timeSinceLevelLoad >= nextIncreaseTime)
            {
                maxRange += 4; // Augmente de 4 toutes les minutes
                nextIncreaseTime = Time.timeSinceLevelLoad + 60f; // Prochaine augmentation dans 60 secondes
            }
            yield return new WaitForSeconds(Mouvement.globalTime); // Laisse un peu de temps entre chaque vérification
        }
    }

    void GeneratePlatforms()
    {
        GameObject platformPrefab = ChoosePlatform();
        Vector3 spawnPosition = lastPosition;
        
        spawnPosition.z = 0f;
        spawnPosition.x = positionX;
        if (platformPrefab == platformPrefabsMonter || platformPrefab == platformPrefabsDescente)
        {
            spawnPosition.y = -1.45f;
        }
        else
        {
            spawnPosition.y = -2.45f;
        }

        GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity, frontParent);
        lastPlatform = newPlatform;

    }
    GameObject ChoosePlatform()
    {
        int numberPlatformId = Random.Range(1, maxRange);

        GameObject selectedPlatform = platformPrefabsPlats[0];

        if (trouCree)
        {
            selectedPlatform = Trou;
            trouCree = false;
            devoirApresTrou = true;
        }
        else if (devoirApresTrou)
        {

            selectedPlatform = platformPrefabsApresTroue.Length > 0 ? platformPrefabsApresTroue[Random.Range(0, platformPrefabsApresTroue.Length)] : null;
            devoirApresTrou = false;
        }
        else if (numberPlatformId <= 15) 
        {
            selectedPlatform = platformPrefabsPlats.Length > 0 ? platformPrefabsPlats[Random.Range(0, platformPrefabsPlats.Length)] : null;
        }
        else if (numberPlatformId == 17)
        {
            if (!devoirDescendre)
            {
                selectedPlatform = platformPrefabsMonter;
                devoirDescendre = true;
            }
            else
            {
                selectedPlatform = platformPrefabsDescente;
                devoirDescendre = false;
            }
        }
        else if (numberPlatformId >= 18)
        {
            if (!devoirApresTrou)
            {
                selectedPlatform = platformPrefabsAvantTroue.Length > 0 ? platformPrefabsAvantTroue[Random.Range(0, platformPrefabsAvantTroue.Length)] : null;
                //trouCree = Random.value < 0.2f; // Diminue encore la probabilité des trous (20%)
                trouCree = true;
            }
            else
            {
                selectedPlatform = platformPrefabsApresTroue.Length > 0 ? platformPrefabsApresTroue[Random.Range(0, platformPrefabsApresTroue.Length)] : null;
                trouCree = false;
            }
        }

        return selectedPlatform;
    }

    void GenerateStrawberry()
    {
        if (Random.value <= ChanceSpawnStrawberry)
        {
            Vector3 strawberryPosition = new Vector3(10f,Random.Range(-1f, 10f),0f);
            Instantiate(Strawberry, strawberryPosition, Quaternion.identity, frontParent);
        }
    }
}
