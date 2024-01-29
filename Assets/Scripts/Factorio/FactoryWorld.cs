using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public struct Tile
{
    public int Id;
    private int2 Coordinate; 
}

public class FactoryWorld : MonoBehaviour
{
    private const float TILE_HEIGHT = 4;

    [SerializeField]
    private float2  TileSize = new int2(4, 4);
    [SerializeField]
    private int2 GridSize = new int2(5, 5);

    private Tile[] worldMap;

    private void Awake()
    { 
        worldMap = new Tile[GridSize.x * GridSize.y];

        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0 ; y < GridSize.y; y++)
            {
                int idx = GetIndex(new int2(x, y));
                worldMap[idx] = new Tile();
                worldMap[idx].Id = (UnityEngine.Random.Range(0, 2) == 0)  ? 0 : 1;
            }
        }
    }

    // Draw Gizmos
    private void OnDrawGizmos()
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y  < GridSize.y; y++)
            {
                int idx = GetIndex(new int2(x, y));
                Tile tile = worldMap[idx];

                Gizmos.color = (tile.Id == 0) ? new Color(0, 1, 0, 0.25f) : new Color(0, 0, 1, 0.25f) ;
                Gizmos.DrawCube(GetCubeCenter(idx), new float3(TileSize.x, 5, TileSize.y));
            }
        }
    }

    private int GetIndex(int2 idx)
    {
        return idx.y + GridSize.y * idx.x;
    }

    private float3 GetCubeCenter(int2 idx)
    {
        return new float3(TileSize.x, 5, TileSize.y) * new int3(idx.x, 0, idx.y);
    }
}
