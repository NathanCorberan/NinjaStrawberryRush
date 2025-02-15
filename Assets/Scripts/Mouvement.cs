using UnityEngine;

public class Mouvement : MonoBehaviour
{
    public static float globalSpeed = 5f; // Vitesse unique pour tous les objets
    public static float globalTime; // Vitesse unique pour tous les objets
    private Animator animator;
    //private GameObject gameObject;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // D�placement vers la gauche avec une vitesse globale
        transform.position += Vector3.left * globalSpeed * Time.deltaTime;
        globalTime = 1 / globalSpeed;

        // V�rifier si l'objet est sorti de l'�cran
        if (transform.position.x < Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f)
        {
            Destroy(gameObject);
        }
    }

    public void HideStrawberry()
    {
        Destroy(gameObject);
    }
}


/*


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generate : MonoBehaviour
{
    public GameObject[] platformPrefabs; // Assign� dans l'inspecteur
    public float platformWidth = 2.0f; // Largeur d'une plateforme
    public float perlinScale = 0.1f; // �chelle du bruit de Perlin
    public Transform playerTransform; // R�f�rence au joueur (si besoin)

    private Vector3 lastPosition;
    private GameObject lastPlatform;
    private Transform frontParent; // R�f�rence au parent "Front"
    private bool isGenerating = true; // Pour arr�ter si besoin

    void Start()
    {
        frontParent = GameObject.Find("Front")?.transform; // Trouve le parent "Front"
        if (frontParent == null)
        {
            Debug.LogError("Le GameObject 'Front' est introuvable !");
            return;
        }

        lastPosition = new Vector3(0, 0.1f, 0); // Position de d�part
        StartCoroutine(GeneratePlatforms()); // Lance la g�n�ration infinie
    }

    IEnumerator GeneratePlatforms()
    {
        int j = 0;
        while (isGenerating)
        {
            float perlinValue = Mathf.PerlinNoise(j * perlinScale, 0);
            bool isGap = perlinValue > 0.7f; // D�termine si un trou est pr�sent

            if (isGap)
            {
                lastPosition.x += platformWidth; // Ajoute un espace pour le trou
                j++;
                yield return null; // Passe au frame suivant pour �viter de bloquer Unity
                continue;
            }

            GameObject platformPrefab = ChoosePlatform(perlinValue);
            if (platformPrefab == null) continue;

            Vector3 spawnPosition = lastPosition;
            spawnPosition.y = platformPrefab.name.StartsWith("Ile") ? 2.0f : 0.1f;
            spawnPosition.z = -1;

            Instantiate(platformPrefab, spawnPosition, Quaternion.identity, frontParent);
            lastPlatform = platformPrefab;
            lastPosition.x += platformWidth;

            j++;
            yield return null; // Passe au frame suivant pour �viter un freeze
        }
    }

    GameObject ChoosePlatform(float perlinValue)
    {
        List<GameObject> possiblePlatforms = new List<GameObject>();

        // Classer les plateformes par type
        List<GameObject> groundPlatforms = new List<GameObject>();
        List<GameObject> islandPlatforms = new List<GameObject>();

        foreach (GameObject prefab in platformPrefabs)
        {
            if (prefab.name.StartsWith("Ile"))
                islandPlatforms.Add(prefab);
            else
                groundPlatforms.Add(prefab);
        }

        // S�lectionner un type de plateforme en fonction du bruit de Perlin
        if (perlinValue < 0.3f)
        {
            possiblePlatforms.AddRange(groundPlatforms);
        }
        else if (perlinValue < 0.6f)
        {
            possiblePlatforms.AddRange(islandPlatforms);
        }
        else
        {
            possiblePlatforms.AddRange(groundPlatforms);
        }

        // V�rification des contraintes : un "Sol Monter" ne peut pas �tre suivi d'un "Sol Descente"
        if (lastPlatform != null && lastPlatform.name.Contains("Monter"))
        {
            possiblePlatforms.RemoveAll(p => p.name.Contains("Descente"));
        }
        if (lastPlatform != null && lastPlatform.name.Contains("Descente"))
        {
            possiblePlatforms.RemoveAll(p => p.name.Contains("Monter"));
        }

        return possiblePlatforms.Count > 0 ? possiblePlatforms[Random.Range(0, possiblePlatforms.Count)] : null;
    }

    public void StopGeneration()
    {
        isGenerating = false; // Arr�te la g�n�ration
    }

    public void ResumeGeneration()
    {
        if (!isGenerating)
        {
            isGenerating = true;
            StartCoroutine(GeneratePlatforms());
        }
    }
}
*/


/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generate : MonoBehaviour
{
    public GameObject[] platformPrefabs; // Assign� dans l'inspecteur
    public float platformWidth = 2.0f; // Largeur d'une plateforme
    public float perlinScale = 0.1f; // �chelle du bruit de Perlin
    public Transform playerTransform; // R�f�rence au joueur (si besoin)

    private Vector3 lastPosition;
    private GameObject lastPlatform;
    private Transform frontParent; // R�f�rence au parent "Front"
    private bool isGenerating = true; // Pour arr�ter si besoin

    private bool lastPlatformWasMonter = false; // Indique si la derni�re plateforme �tait une mont�e
    private bool lastPlatformWasDescente = false;

    void Start()
    {
        frontParent = GameObject.Find("Front")?.transform; // Trouve le parent "Front"
        if (frontParent == null)
        {
            Debug.LogError("Le GameObject 'Front' est introuvable !");
            return;
        }

        lastPosition = new Vector3(0, 0.1f, 0); // Position de d�part
        StartCoroutine(GeneratePlatforms()); // Lance la g�n�ration infinie
    }

    IEnumerator GeneratePlatforms()
    {
        int j = 0;
        while (isGenerating)
        {
            float perlinValue = Mathf.PerlinNoise(j * perlinScale, 0);
            bool isGap = perlinValue > 0.7f; // D�termine si un trou est pr�sent

            if (isGap)
            {
                lastPosition.x += platformWidth; // Ajoute un espace pour le trou
                j++;
                yield return null; // Passe au frame suivant pour �viter de bloquer Unity
                continue;
            }

            GameObject platformPrefab = ChoosePlatform();
            if (platformPrefab == null) continue;

            Vector3 spawnPosition = lastPosition;
            spawnPosition.y = platformPrefab.name.StartsWith("Ile") ? 2.0f : 0.1f;
            spawnPosition.z = -1;

            Instantiate(platformPrefab, spawnPosition, Quaternion.identity, frontParent);
            lastPlatform = platformPrefab;
            lastPosition.x += platformWidth;

            j++;
            yield return null; // Passe au frame suivant pour �viter un freeze
        }
    }

    GameObject ChoosePlatform()
    {
        List<GameObject> possiblePlatforms = new List<GameObject>();
        List<GameObject> groundPlatforms = new List<GameObject>();
        List<GameObject> islandPlatforms = new List<GameObject>();

        foreach (GameObject prefab in platformPrefabs)
        {
            if (prefab.name.StartsWith("Ile"))
                islandPlatforms.Add(prefab);
            else
                groundPlatforms.Add(prefab);
        }

        // Si la derni�re plateforme �tait une mont�e, on impose un sol 1-3
        if (lastPlatformWasMonter)
        {
            possiblePlatforms.AddRange(groundPlatforms.FindAll(p => p.name.Contains("1-3")));
            lastPlatformWasMonter = false; // R�initialiser la mont�e
        }
        // Si la derni�re plateforme �tait une descente, on ne peut pas mettre de mont�e
        else if (lastPlatformWasDescente)
        {
            possiblePlatforms.AddRange(groundPlatforms.FindAll(p => p.name.Contains("1-3"))); // 1-3 ou trou
            possiblePlatforms.AddRange(groundPlatforms.FindAll(p => p.name.Contains("Descente")));
            lastPlatformWasDescente = false; // R�initialiser la descente
        }
        else
        {
            // Ajout d'un sol ou d'une �le avec les r�gles de probabilit�
            if (Random.value < 0.02f && islandPlatforms.Count > 0)
            {
                possiblePlatforms.AddRange(islandPlatforms);
            }
            else
            {
                possiblePlatforms.AddRange(groundPlatforms);
            }
        }

        // Choisir un trou (2% de chance)
        if (Random.value < 0.02f)
        {
            possiblePlatforms.AddRange(groundPlatforms.FindAll(p => p.name.Contains("Trou")));
        }

        // Emp�che la g�n�ration de la descente si une mont�e n'a pas �t� effectu�e avant
        if (lastPlatform != null && lastPlatform.name.Contains("Monter"))
        {
            possiblePlatforms.RemoveAll(p => p.name.Contains("Descente"));
        }

        // Emp�che une mont�e si une descente a d�j� �t� g�n�r�e avant
        if (lastPlatform != null && lastPlatform.name.Contains("Descente"))
        {
            possiblePlatforms.RemoveAll(p => p.name.Contains("Monter"));
        }

        // Choisir une plateforme parmi les possibilit�s
        GameObject selectedPlatform = possiblePlatforms.Count > 0 ? possiblePlatforms[Random.Range(0, possiblePlatforms.Count)] : null;

        // Si une plateforme "Monter" est choisie, la prochaine doit �tre une descente
        if (selectedPlatform != null && selectedPlatform.name.Contains("Monter"))
        {
            lastPlatformWasMonter = true;
        }

        // Si une plateforme "Descente" est choisie, la prochaine doit �tre une mont�e
        if (selectedPlatform != null && selectedPlatform.name.Contains("Descente"))
        {
            lastPlatformWasDescente = true;
        }

        return selectedPlatform;
    }

    public void StopGeneration()
    {
        isGenerating = false; // Arr�te la g�n�ration
    }

    public void ResumeGeneration()
    {
        if (!isGenerating)
        {
            isGenerating = true;
            StartCoroutine(GeneratePlatforms());
        }
    }
}*/