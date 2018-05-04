using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBox : MonoBehaviour {
    public Cannon cannonPrefab;
    public Generator generatorPrefab;
    private Player player;
    void Start() {
        player = GetComponent<Player>();
    }

    public void DismountFacility() {
        BlankTile tile = GetValidTile();
        if (tile == null)
            return;
        if (tile.Slot == null)
            return;
        tile.Slot.Dismount(player.team);
    }

    public void InstallCannon() {
        BlankTile tile = GetValidAndEmptyTile();
        if (tile == null)
            return;
        // build cannon if BlankTile has empty slot
        tile.Slot = Instantiate(cannonPrefab, tile.transform);
        tile.Slot.Install(player.team);
    }
    public void InstallGenerator() {
        BlankTile tile = GetValidAndEmptyTile();
        if (tile == null)
            return;
        // build generator if BlankTile has empty slot
        tile.Slot = Instantiate(generatorPrefab, tile.transform);
        tile.Slot.Install(player.team);
    }

    /// <summary>
    /// only return a tile if it's slot is free
    /// </summary>
    /// <returns></returns>
    private BlankTile GetValidAndEmptyTile() {
        BlankTile tile = GetValidTile();
        if (tile == null || tile.Slot != null)
            return null;

        return tile;
    }
    private BlankTile GetValidAndMountedTile() {
        BlankTile tile = GetValidTile();
        if (tile == null || tile.Slot != null)
            return null;

        return tile;
    }
    private BlankTile GetValidTile() {
        BlankTile tile = null;
        // check if player has valid tile
        if (player.currentSector != null) {
            if (player.currentSector.GetType() == typeof(BlankTile)) {
                tile = player.currentSector as BlankTile;
            }
        }
        return tile;
    }
}