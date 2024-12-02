using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public GameObject[] tilePrefabs;
    public float zSpawn = 0;
    public float tileLenght = 30;
    //public Vector3 offset = new Vector3(-1.3f, 0, 0);
    public int numberOfTiles = 5;
    public Transform playerTransform;
    public int anzTiles = 6;
    private List<GameObject> activeTiles = new List<GameObject>();

    void Start()
    {
        for(int i = 0; i < numberOfTiles; i++)
        {
            if(i==0)
            {
                SpawnTile(0);
            } else
            {
                //5 sind die Anzahl der Tiles die in GameObject drinne sind
                SpawnTile(Random.Range(0, anzTiles));
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        //35 safezone weil sonst die Plattform unter dem Spieler deletet werden würde
        if(playerTransform.position.z - 35 > zSpawn - (numberOfTiles * tileLenght))
        {
            SpawnTile(Random.Range(0, anzTiles));
            DeleteTile();
        }
    }

    public void SpawnTile(int tileIndex)
    {
        // Calculate the spawn position with an X offset of -2.6f
        Vector3 spawnPosition = transform.position + transform.forward * zSpawn;

        // Instantiate the tile at the calculated position
        GameObject go = Instantiate(tilePrefabs[tileIndex], spawnPosition, transform.rotation);

        // Add the tile to the activeTiles list
        activeTiles.Add(go);

        // Update zSpawn for the next tile
        zSpawn += tileLenght;
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
