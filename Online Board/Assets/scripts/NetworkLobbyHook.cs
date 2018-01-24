using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook 
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        scr_cursor cursor = gamePlayer.GetComponent<scr_cursor>();

        cursor.pname = lobby.playerName;
        cursor.pcolor = lobby.playerColor;
    }
}
