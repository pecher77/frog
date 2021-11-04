using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public Transform Player;
    public Chunk[] Chunks;
    public Chunk FirstChunk;


    private List<Chunk> spawnedChunks = new List<Chunk>();

    void Start()
    {
        spawnedChunks.Add(FirstChunk);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.position.x > spawnedChunks[spawnedChunks.Count - 1].End.position.x - 15)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        Chunk chunk = Instantiate(Chunks[Random.RandomRange(0, Chunks.Length)]);
        chunk.transform.position = spawnedChunks[spawnedChunks.Count - 1].End.position - chunk.Begin.localPosition;
        spawnedChunks.Add(chunk);
    }
}
