using RSTD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_Manager : Singleton<UI_Manager> {
    public Image buildPanel, actionPanel;
    public Button BtnBuildCannon, BtnBuildGen, BtnDismount;

	void Start () {
        PlayerTrigger.PlayerEnteredTile += OnPlayerEnteredTile;
        PlayerTrigger.PlayerLeftTile += OnPlayerLeftTile;
        PlayerTrigger.PlayerStaysInTile += OnPlayerStaysInTile;
    }
    //private void OnDestroy() {
    //    PlayerTrigger.PlayerEnteredTile -= OnPlayerEnteredTile;
    //    PlayerTrigger.PlayerLeftTile -= OnPlayerLeftTile;
    //    PlayerTrigger.PlayerStaysInTile -= OnPlayerStaysInTile;
    //}

    private void OnPlayerEnteredTile(Sector tile) {
        if (tile.GetType() == typeof(BlankTile)) {
            buildPanel.gameObject.SetActive(true);
        }
    }
    private void OnPlayerStaysInTile(Sector tile) {
        if (tile.GetType() == typeof(BlankTile)) {
            BlankTile bt = tile as BlankTile;
            if (bt.Slot == null) {
                BtnBuildCannon.gameObject.SetActive(true);
                BtnBuildGen.gameObject.SetActive(true);
                BtnDismount.gameObject.SetActive(false);
            } else {
                BtnBuildCannon.gameObject.SetActive(false);
                BtnBuildGen.gameObject.SetActive(false);
                BtnDismount.gameObject.SetActive(true);
            }             
        }
    }
    private void OnPlayerLeftTile(Sector tile) {
        BtnBuildCannon.gameObject.SetActive(false);
        BtnBuildGen.gameObject.SetActive(false);
        BtnDismount.gameObject.SetActive(false);
        buildPanel.gameObject.SetActive(false);
        //actionPanel.gameObject.SetActive(false);
    }

    public void BuildCannon() {
        // TODO: Here the proper player (local player) must be assigned for networking
        Player player = FindObjectOfType<Player>();
        player.ToolBox.InstallCannon();
    }
    public void BuildGenerator() {
        // TODO: Here the proper player (local player) must be assigned for networking
        Player player = FindObjectOfType<Player>();
        player.ToolBox.InstallGenerator();
    }
    public void DismountFacility() {
        // TODO: Here the proper player (local player) must be assigned for networking
        Player player = FindObjectOfType<Player>();
        player.ToolBox.DismountFacility();
    }
}