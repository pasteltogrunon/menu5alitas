using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelecomBuidling : Building
{
    [SerializeField] int radius = 2;

    public override void onPlace(Tile tile)
    {
        base.onPlace(tile);

        Vector2Int center = tile.uvCoords;

        for (int i = -radius; i < 0; i++)
        {
            for (int j = -radius + Mathf.Abs(i); j <= radius; j++)
            {
                Tile neighbor = TileMap.Instance.GetTile(center.x + i, center.y + j);
                if (neighbor)
                    neighbor.Unlocked = true;
            }
        }

        for (int i = 0; i <= radius; i++)
        {
            for (int j = -radius; j <= radius - Mathf.Abs(i); j++)
            {
                Tile neighbor = TileMap.Instance.GetTile(center.x + i, center.y + j);
                if (neighbor)
                    neighbor.Unlocked = true;
            }
        }
    }
}
