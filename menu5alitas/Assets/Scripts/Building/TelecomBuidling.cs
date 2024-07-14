using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelecomBuidling : Building
{
    [SerializeField] int radius = 2;
    [SerializeField] AudioClip unlockTilesSound;

    public override void onPlace(Tile tile)
    {
        base.onPlace(tile);

        StartCoroutine(UnlockTiles(tile.uvCoords));
    }

    IEnumerator UnlockTiles(Vector2Int center)
    {
        for(int r = 1; r <= radius; r++)
        {
            for (int i = -r; i < 0; i++)
            {
                for (int j = -r + Mathf.Abs(i); j <= r; j++)
                {
                    Tile neighbor = TileMap.Instance.GetTile(center.x + i, center.y + j);
                    if (neighbor)
                        neighbor.Unlocked = true;
                }
            }

            for (int i = 0; i <= r; i++)
            {
                for (int j = -r; j <= r - Mathf.Abs(i); j++)
                {
                    Tile neighbor = TileMap.Instance.GetTile(center.x + i, center.y + j);
                    if (neighbor)
                        neighbor.Unlocked = true;
                }
            }

            SFXManager.PlaySound(unlockTilesSound);
            yield return new WaitForSeconds(0.7f);
        }
    }
}
