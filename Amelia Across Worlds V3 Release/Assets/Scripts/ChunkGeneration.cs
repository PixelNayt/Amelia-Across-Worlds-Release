using System.Collections;
using UnityEngine;

public class ChunkGeneration : MonoBehaviour
{
    //A Vector 2 is a representation of vector points, here it represents and only holds positions X and Y, Vector3 represents 3D points using XYZ coords.
    public Vector2 chunks; 
    public Vector2 chunkResolution;

    public Material terrainMaterial;

    public GameObject water;
    public float waterLevel; 
    public GameObject[] trees;
    public float treeRandomness;
    public float treeThreshold;

    public int chunksChunkLoaded;

    public int seed;

    //Random Seed Generator
    private void Awake()
    {
        seed = Random.Range(0, 99999);
        Random.InitState(seed);
    }

    private void Start()
    {
        waterLevel = Mathf.PerlinNoise(seed, seed) * 50;
        StartCoroutine(GenerateChunks());

        //Spawns Water at the center of the map, Almost similar to the Midpoint Formula (G9 Math IKZ)
        GameObject current = Instantiate(water, new Vector3((128 * chunks.x) / 2, waterLevel, (128 * chunks.y) / 2), Quaternion.identity);
        current.transform.localScale = new Vector3(16 * chunks.x, 128, 16 * chunks.y);
    }

    //What is a Coroutine >>> It is essentially like a void but we can do more stuff
    //We add a coroutine here because we want to add a delay between each chunk of terrain we make for PEFORMANCE reasons
    public IEnumerator GenerateChunks()
    {
        //This loops through the x and z of the amount of chunks we want, essentially making a 2D grid of chunks then creates a chunk object (Terrain),
        //then sets its position, then there is a delay for each generation or "waits"
        for (int i = 0, x = 0; x < chunks.x; x++)
        {
            for (int z = 0; z < chunks.y; z++)
            {
                //Create the chunk object (128 spacing, just makes the world larger, you can change this value to whatever u want) NAMING + Add Components
                GameObject current = new GameObject("TerrainChunk" + new Vector2(x * 128, z * 128), typeof(TerrainGeneration), typeof(MeshRenderer), typeof(MeshCollider), typeof(MeshFilter));

                //parents created object to the tranform which is our game object, prevents unity editor clutter
                current.transform.parent = transform;

                //Set Position of chunk object
                //This also keeps the chunks the same size no matter what, 128 is the size of each chunk
                current.transform.position = new Vector3(x * (chunkResolution.x) * (128 / chunkResolution.x), 0f, z * (chunkResolution.y) * (128 / chunkResolution.y)); 
                i++;

                //Loads chunk per row based on the set value in the editor
                if (i == chunksChunkLoaded)
                {
                    i = 0;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
            }
        }
    }
}
