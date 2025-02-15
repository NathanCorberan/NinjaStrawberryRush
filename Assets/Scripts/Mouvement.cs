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
        // Déplacement vers la gauche avec une vitesse globale
        transform.position += Vector3.left * globalSpeed * Time.deltaTime;
        globalTime = 1 / globalSpeed;

        // Vérifier si l'objet est sorti de l'écran
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
    public GameObject[] platformPrefabs; // Assigné dans l'inspecteur
    public float platformWidth = 2.0f; // Largeur d'une plateforme
    public float perlinScale = 0.1f; // Échelle du bruit de Perlin
    public Transform playerTransform; // Référence au joueur (si besoin)

    private Vector3 lastPosition;
    private GameObject lastPlatform;
    private Transform frontParent; // Référence au parent "Front"
    private bool isGenerating = true; // Pour arrêter si besoin

    void Start()
    {
        frontParent = GameObject.Find("Front")?.transform; // Trouve le parent "Front"
        if (frontParent == null)
        {
            Debug.LogError("Le GameObject 'Front' est introuvable !");
            return;
        }

        lastPosition = new Vector3(0, 0.1f, 0); // Position de départ
        StartCoroutine(GeneratePlatforms()); // Lance la génération infinie
    }

    IEnumerator GeneratePlatforms()
    {
        int j = 0;
        while (isGenerating)
        {
            float perlinValue = Mathf.PerlinNoise(j * perlinScale, 0);
            bool isGap = perlinValue > 0.7f; // Détermine si un trou est présent

            if (isGap)
            {
                lastPosition.x += platformWidth; // Ajoute un espace pour le trou
                j++;
                yield return null; // Passe au frame suivant pour éviter de bloquer Unity
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
            yield return null; // Passe au frame suivant pour éviter un freeze
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

        // Sélectionner un type de plateforme en fonction du bruit de Perlin
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

        // Vérification des contraintes : un "Sol Monter" ne peut pas être suivi d'un "Sol Descente"
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
        isGenerating = false; // Arrête la génération
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
    public GameObject[] platformPrefabs; // Assigné dans l'inspecteur
    public float platformWidth = 2.0f; // Largeur d'une plateforme
    public float perlinScale = 0.1f; // Échelle du bruit de Perlin
    public Transform playerTransform; // Référence au joueur (si besoin)

    private Vector3 lastPosition;
    private GameObject lastPlatform;
    private Transform frontParent; // Référence au parent "Front"
    private bool isGenerating = true; // Pour arrêter si besoin

    private bool lastPlatformWasMonter = false; // Indique si la dernière plateforme était une montée
    private bool lastPlatformWasDescente = false;

    void Start()
    {
        frontParent = GameObject.Find("Front")?.transform; // Trouve le parent "Front"
        if (frontParent == null)
        {
            Debug.LogError("Le GameObject 'Front' est introuvable !");
            return;
        }

        lastPosition = new Vector3(0, 0.1f, 0); // Position de départ
        StartCoroutine(GeneratePlatforms()); // Lance la génération infinie
    }

    IEnumerator GeneratePlatforms()
    {
        int j = 0;
        while (isGenerating)
        {
            float perlinValue = Mathf.PerlinNoise(j * perlinScale, 0);
            bool isGap = perlinValue > 0.7f; // Détermine si un trou est présent

            if (isGap)
            {
                lastPosition.x += platformWidth; // Ajoute un espace pour le trou
                j++;
                yield return null; // Passe au frame suivant pour éviter de bloquer Unity
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
            yield return null; // Passe au frame suivant pour éviter un freeze
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

        // Si la dernière plateforme était une montée, on impose un sol 1-3
        if (lastPlatformWasMonter)
        {
            possiblePlatforms.AddRange(groundPlatforms.FindAll(p => p.name.Contains("1-3")));
            lastPlatformWasMonter = false; // Réinitialiser la montée
        }
        // Si la dernière plateforme était une descente, on ne peut pas mettre de montée
        else if (lastPlatformWasDescente)
        {
            possiblePlatforms.AddRange(groundPlatforms.FindAll(p => p.name.Contains("1-3"))); // 1-3 ou trou
            possiblePlatforms.AddRange(groundPlatforms.FindAll(p => p.name.Contains("Descente")));
            lastPlatformWasDescente = false; // Réinitialiser la descente
        }
        else
        {
            // Ajout d'un sol ou d'une île avec les règles de probabilité
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

        // Empêche la génération de la descente si une montée n'a pas été effectuée avant
        if (lastPlatform != null && lastPlatform.name.Contains("Monter"))
        {
            possiblePlatforms.RemoveAll(p => p.name.Contains("Descente"));
        }

        // Empêche une montée si une descente a déjà été générée avant
        if (lastPlatform != null && lastPlatform.name.Contains("Descente"))
        {
            possiblePlatforms.RemoveAll(p => p.name.Contains("Monter"));
        }

        // Choisir une plateforme parmi les possibilités
        GameObject selectedPlatform = possiblePlatforms.Count > 0 ? possiblePlatforms[Random.Range(0, possiblePlatforms.Count)] : null;

        // Si une plateforme "Monter" est choisie, la prochaine doit être une descente
        if (selectedPlatform != null && selectedPlatform.name.Contains("Monter"))
        {
            lastPlatformWasMonter = true;
        }

        // Si une plateforme "Descente" est choisie, la prochaine doit être une montée
        if (selectedPlatform != null && selectedPlatform.name.Contains("Descente"))
        {
            lastPlatformWasDescente = true;
        }

        return selectedPlatform;
    }

    public void StopGeneration()
    {
        isGenerating = false; // Arrête la génération
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