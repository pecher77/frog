using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChunkGenerator : MonoBehaviour
{
    public Transform Player;
    public Transform Enemy;
    public Transform FirstChunkPosition;
    public Chunk[] Chunks;
    public Chunk FirstChunk;
    [Space(20)]
    public bool only1ChunkRepead = false;
    public Chunk oneChunkRepead;


    private List<Chunk> spawnedChunks = new List<Chunk>();

    void Start()
    {
        //var objects = SceneManager.GetActiveScene().GetRootGameObjects();
        //foreach (var gameObject in objects)
        //{
        //    if (gameObject.name.Contains("Chunk"))
        //    {
        //        Destroy(gameObject);
        //    }
        //}

        spawnedChunks.Add(FirstChunk);
        //if (only1ChunkRepead)
        //{
        //    Chunk chunk = Instantiate(oneChunkRepead);
        //    chunk.transform.position = FirstChunkPosition.position - chunk.Begin.localPosition;
        //    spawnedChunks.Add(chunk);
        //}
        //else
        //{
        //Chunk chunk = Instantiate(FirstChunk);
        //chunk.transform.position = FirstChunkPosition.position - chunk.Begin.localPosition;
        //spawnedChunks.Add(FirstChunk);
        //}

    }

    // Update is called once per frame
    void Update()
    {
        if (Player.position.x > spawnedChunks[spawnedChunks.Count - 1].End.position.x - 15 
            || Enemy.position.x > spawnedChunks[spawnedChunks.Count - 1].End.position.x - 15)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        Chunk chunk;
        if (only1ChunkRepead)
        {
            chunk = Instantiate(oneChunkRepead);
        }
        else
        {
            chunk = Instantiate(Chunks[Random.RandomRange(0, Chunks.Length)]);
        }

        chunk.transform.position = spawnedChunks[spawnedChunks.Count - 1].End.position - chunk.Begin.localPosition;
        spawnedChunks.Add(chunk);
    }
}
