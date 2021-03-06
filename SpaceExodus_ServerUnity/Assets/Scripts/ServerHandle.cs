﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerHandle
{
    public static void WelcomeReceived(int fromClient, CustomPacket packet)
    {
        int clientIdCheck = packet.ReadInt();
        string username = packet.ReadString();
        Debug.Log($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {fromClient}");
        if (fromClient != clientIdCheck)
        {
            Debug.Log($"Player \"{username}\" (ID : {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
        }
        Server.started = true;
        Server.clients[fromClient].SendIntoGame(username);
    }

    public static void PlayerMovement(int fromClient, CustomPacket packet)
    {
        bool[] inputs = new bool[packet.ReadInt()];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = packet.ReadBool();
        }
        Server.clients[fromClient].player.SetInputs(inputs);
    }

    public static void PlayerShooting(int fromClient, CustomPacket packet)
    {
        int weaponLevel = packet.ReadInt();
        float projectileSpeed = packet.ReadFloat();
        Server.clients[fromClient].player.Shooting(weaponLevel, projectileSpeed);
    }

    public static void GameOver(int fromClient, CustomPacket packet)
    {
        int winner = packet.ReadInt();
        ServerSend.GameOver(winner);
        NetworkManager.instance.GameOver();
    }
}
