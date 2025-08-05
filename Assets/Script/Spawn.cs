using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Animal
{
    public string nameBicho;
    public GameObject prefabBicho;
    public int quantidade; 
}

public class Spawn : MonoBehaviour
{
    public List<Animal> animais; 

    private BoxCollider areaSpawn; 

    void Start()
    {
        areaSpawn = GetComponent<BoxCollider>();
        if (areaSpawn == null)
        {
            Debug.LogError("Este objeto precisa de um BoxCollider!");
            return;
        }

        foreach (Animal animal in animais)
        {
            for (int i = 0; i < animal.quantidade; i++)
            {
                Vector3 spawnPos = GetRandomPositionInCollider(areaSpawn);
                Instantiate(animal.prefabBicho, spawnPos, Quaternion.identity);
            }
        }
    }

    Vector3 GetRandomPositionInCollider(BoxCollider collider)
    {
        Vector3 center = collider.transform.TransformPoint(collider.center);
        Vector3 size = collider.size;

        float x = Random.Range(-size.x / 2f, size.x / 2f);
        float y = Random.Range(-size.y / 2f, size.y / 2f);
        float z = Random.Range(-size.z / 2f, size.z / 2f);

        // Aplica a rotação/posição do objeto pai corretamente
        Vector3 localPos = new Vector3(x, y, z);
        return collider.transform.TransformPoint(localPos + collider.center);
    }

}
