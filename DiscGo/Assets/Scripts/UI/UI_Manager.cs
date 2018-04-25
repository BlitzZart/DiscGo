using RSTD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_Manager : Singleton<UI_Manager> {

    public Image buildPanel, actionPanel;

	void Start () {
        PlayerTrigger.PlayerEnteredTile += OnPlayerEnteredTile;
        PlayerTrigger.PlayerLeftTile += OnPlayerLeftTile;
    }
    private void OnDestroy() {
        PlayerTrigger.PlayerEnteredTile -= OnPlayerEnteredTile;
        PlayerTrigger.PlayerLeftTile -= OnPlayerLeftTile;
    }

    private void OnPlayerEnteredTile(Sector tile) {
        if (tile.GetType() == typeof(BlankTile)) {
            buildPanel.gameObject.SetActive(true);
        }
    }

    private void OnPlayerLeftTile(Sector tile) {
        buildPanel.gameObject.SetActive(false);
        //actionPanel.gameObject.SetActive(false);
    }

    public void BuildGun() {
        // TODO: Here the proper player (local player) must be assigned for networking

        Player player = FindObjectOfType<Player>();
        player.ToolBox.BuildGun();
    }
}