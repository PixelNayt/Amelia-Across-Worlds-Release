using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TerrainGeneration : MonoBehaviour
{
    ChunkGeneration chunkGen; //Creates a reference to the ChunkGeneration script

    //Components to use so we can use their 
    Mesh mesh;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;
    Vector2[] uv;

    private void Start()
    {
        //In the editor, the "ChunkGen" game object has the "Manager" tag
        chunkGen = GameObject.FindGameObjectWithTag("Manager").GetComponent<ChunkGeneration>();
        if (chunkGen == null)
        {
            return;
        }

        //Just setting the components
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        meshRenderer.material = chunkGen.terrainMaterial;

        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        //Meshes are visuals in 3D modelling that consists of vertices, triangles or polygons, that defines a shape in 2D or 3D
        mesh = new Mesh();

        //Size of the vertices or number of vertices //Creates a grid of vertices
        Vector3[] vertices = new Vector3[(int)((chunkGen.chunkResolution.x + 1) * (chunkGen.chunkResolution.y + 1))]; 
        uv = new Vector2[vertices.Length];

        //Meshes are made up of vertices which are points or positions in a grid, the triangles are how meshes are created, create triangles, not set yet
        int[] triangles; 

        for(int i = 0, x = 0; x <= chunkGen.chunkResolution.x; x++)
        {
            for (int z = 0; z <= chunkGen.chunkResolution.y; z++)
            { 
                //Write the data to our vertices
                float y = Noise(x, z, BiomeNoise(x, z));
                vertices[i] = new Vector3(x * (128 / chunkGen.chunkResolution.x), y, z * (128 / chunkGen.chunkResolution.y));

                //Generate trees procedurally with PerlinNoise
                //doesSpawn, uses noise to check locations on our (x, z) vertex points if trees can spawn on the position of the transform object
                float doesSpawn = Mathf.PerlinNoise(x + transform.position.x + chunkGen.seed, z + transform.position.z + chunkGen.seed);
                doesSpawn = Mathf.PerlinNoise((x + transform.position.x) * 0.1f  + chunkGen.seed, z + transform.position.z + chunkGen.seed);

                if (doesSpawn > chunkGen.treeThreshold && y > chunkGen.waterLevel + 15)
                {
                    //Spawn trees only when it is above the water level and 
                    //whatSpawns, again gets the position of our transform objects (chunks), then spawns them on the chunk
                    float whatSpawns = Mathf.PerlinNoise(x + transform.position.x + (chunkGen.seed * 5), z + transform.position.z + (chunkGen.seed * 3));
                    whatSpawns = whatSpawns * chunkGen.trees.Length; //Checks the prefabs in our editor and uses one? of them
                    whatSpawns = Mathf.RoundToInt(whatSpawns); //we can only have a discrete amount of trees
                    GameObject current = Instantiate(chunkGen.trees[(int)whatSpawns], new Vector3(x * (128 / chunkGen.chunkResolution.x) + transform.position.x, y + transform.position.y, z * (128 / chunkGen.chunkResolution.y) + transform.position.z), Quaternion.identity);
                    current.transform.parent = transform;
                }
                
                i++; 
            } 
        }

        //For UV Unwrapping, it allows the objects or meshes we create be given a texture as we are generating multiple triangles to make mesh
        for (int i = 0; i < uv.Length; i++)
        {
            //This basically allows the uv to take the values or positions of our vertices so we can give it accurate textures later
            uv[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        //Array for storing the amount of triangles needed to make squares per chunk, we multiply this by 6 because we need 6 points per square
        //Our squares are made up of two triangles, each triangle has 3 points, since two halves of a triangle gives us 6 points
        //Overall we are just setting our triangles' points
        triangles = new int[(int)(chunkGen.chunkResolution.x * chunkGen.chunkResolution.y * 6)];    

        //These keeps track of our vert and tris (triangles) positions
        int tris = 0;
        int vert = 0;

        //Loop through our chunk resolutions
        for (int x = 0; x < chunkGen.chunkResolution.y; x++)
        {
            for (int z = 0; z < chunkGen.chunkResolution.x; z++)
            {
                //This creates a row of squares by setting the points of each vertex point on a triangle clockwise (Vertex points Ref: 0,1,2, 2,1,3)
                //Note to Pixel: draw or illustrate if asked to visualize it
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = (int)(vert + chunkGen.chunkResolution.x + 1);
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = (int)(vert + chunkGen.chunkResolution.x + 1);
                triangles[tris + 5] = (int)(vert + chunkGen.chunkResolution.x + 2);

                //Increment vert by one, add this to each of the points so in each loop it shifts by 1
                //Increment tris by 6, add this to the triangle index so we dont update the first 6 points of the first square infinitely
                //It shifts to the next triangle in the list
                vert++;
                tris += 6; 
            }

            //Increment vert to create another row Repeat the loop until 
            vert++; 
        }

        
        mesh.Clear();

        //Just setting up the values for the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateBounds();
        mesh.triangles = mesh.triangles.Reverse().ToArray();
        meshCollider.sharedMesh = mesh;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

    }


    //NOISE GENERATOR
    float Noise(float x, float z, float biomeNoise)
    {
        //This is basically copy paste the same noise but change it differently to make multiple layers of noise
        //Base plate
        float y = biomeNoise * 150f;

        //Mountains
        float multiplier = 1 + Mathf.Pow(BiomeNoise(x, z), 3) * 10f;
        y = y * multiplier;
        y += Mathf.PerlinNoise(((x * (128 / chunkGen.chunkResolution.x)) + transform.position.x + chunkGen.seed) * 0.0003f, ((z * (128 / chunkGen.chunkResolution.y)) + transform.position.z + chunkGen.seed) * 0.0003f * 150f) * biomeNoise;

        //Hills
        y += (Mathf.PerlinNoise(((x * (128 / chunkGen.chunkResolution.x)) + transform.position.x + chunkGen.seed) * 0.007f, ((z * (128 / chunkGen.chunkResolution.y)) + transform.position.z + chunkGen.seed) * 0.007f) * 70) * biomeNoise;

        return y;
    }

    float BiomeNoise(float x, float z)
    {
        //Adding more Perlin Noise Modifications to the mesh allows for terrain detailing or even creating biomes
        //We take the coordinates of our vertex points (x,z), same thing we did with the previous scripts above, apply the PerlinNoise function on them
        //to add noise to our (x,z) points, then we add that noise to our y-value which has the coordinates for or our x,z
        //in order to add the height on them, we also clamp those values from -1 to 1 to prevent values over those
        float y = Mathf.PerlinNoise(((x * (128 / chunkGen.chunkResolution.x)) + transform.position.x + chunkGen.seed) * 0.0002f, ((z * (128 / chunkGen.chunkResolution.y)) + transform.position.z + chunkGen.seed) * 0.0002f);
        y += Mathf.PerlinNoise(((x * (128 / chunkGen.chunkResolution.x)) + transform.position.x + chunkGen.seed) * 0.0007f, ((z * (128 / chunkGen.chunkResolution.y)) + transform.position.z + chunkGen.seed) * 0.0007f);
        y -= Mathf.PerlinNoise(((x * (128 / chunkGen.chunkResolution.x)) + transform.position.x + chunkGen.seed) * 0.00005f, ((z * (128 / chunkGen.chunkResolution.y)) + transform.position.z + chunkGen.seed) * 0.00005f) * 2;
        y = Mathf.Clamp(y, -1, 1);
        return y;  
    }

}