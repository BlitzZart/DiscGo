using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBox : MonoBehaviour {
    public Gun gunPrefab;
    private Player player;
    void Start() {
        player = GetComponent<Player>();
    }

    public void UnmountUnit() {

    }

    public void BuildGun() {
        BlankTile tile = GetCurrentTileIfValid();
        if (tile == null)
            return;
        // build gun if BlankTile has empty slot
        tile.Slot = Instantiate(gunPrefab, tile.transform);
    }

    public void BuildGenerator() {
        BlankTile tile = GetCurrentTileIfValid();
        if (tile == null)
            return;
        // build generator if BlankTile has empty slot

    }

    /// <summary>
    /// only return a tile if it's slot is free
    /// </summary>
    /// <returns></returns>
    private BlankTile GetCurrentTileIfValid() {
        BlankTile tile = null;
        // check if player has valid tile
        if (player.currentSector != null) {
            if (player.currentSector.GetType() == typeof(BlankTile)) {
                tile = player.currentSector as BlankTile;
            }
        }
        if (tile == null)
            return null;
        if (tile.Slot != null)
            return null;

        return tile;
    }
}