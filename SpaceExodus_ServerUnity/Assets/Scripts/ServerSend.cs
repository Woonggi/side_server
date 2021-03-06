﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int toClient, CustomPacket packet)
    {
        packet.WriteLength();
        Server.clients[toClient].tcp.SendData(packet);
    }

    private static void SendUDPData(int toClient, CustomPacket packet)
    {
        packet.WriteLength();
        Server.clients[toClient].udp.SendData(packet);
    }

    private static void SendTCPDataToAll(CustomPacket packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.maxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(packet);
        }
    }

    private static void SendTCPDataToAll(CustomPacket packet, int except)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.maxPlayers; i++)
        {
            if (i != except)
            {
                Server.clients[i].tcp.SendData(packet);
            }
        }
    }

    private static void SendUDPDataToAll(CustomPacket packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.maxPlayers; i++)
        {
            Server.clients[i].udp.SendData(packet);
        }
    }

    private static void SendUDPDataToAll(CustomPacket packet, int except)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.maxPlayers; i++)
        {
            if (i != except)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }
    }

    public static void Welcome(int toClient, string msg)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_WELCOME))
        {
            packet.Write(msg);
            packet.Write(toClient);
            packet.Write(Server.goalKills);
            SendTCPData(toClient, packet);
        }
    }


    public static void SpawnPlayer(int toClient, Player player)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_SPAWN_PLAYER))
        {
            packet.Write(player.id);
            packet.Write(player.username);
            packet.Write(player.spawnPosition);
            packet.Write(player.transform.rotation);
            packet.Write(GameSettings.PLAYER_MAX_HEALTH);

            SendTCPData(toClient, packet);
        }
    }

    public static void PlayerPosition(Player player)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_POSITION))
        {
            packet.Write(player.id);
            packet.Write(player.transform.position);
            packet.Write(Server.frame);
            SendUDPDataToAll(packet);
        }
    }
    public static void PlayerRotation(Player player)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_ROTATION))
        {
            // TEST!
            packet.Write(player.id);
            packet.Write(player.transform.rotation);
            // TODO: no need to send frame.
            packet.Write(Server.frame);
            SendUDPDataToAll(packet);
        }
    }
    public static void PlayerShooting(Player player, Bullet bullet)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_SHOOTING))
        {
            packet.Write(player.id);
            packet.Write(bullet.index);
            packet.Write(player.heading);
            SendTCPDataToAll(packet);
        }
    }

    public static void BulletPosition(Bullet bullet)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_BULLET_POSITION))
        {
            packet.Write(bullet.bulletId);
            packet.Write(bullet.index);
            packet.Write(bullet.transform.position);
            SendUDPDataToAll(packet);
        }
    }
    public static void BulletDestroy(Bullet bullet)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_BULLET_DESTROY))
        {
            packet.Write(bullet.bulletId);
            packet.Write(bullet.index);
            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerDisconnected(int playerId)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_DISCONNECTED))
        {
            packet.Write(playerId);
            SendTCPDataToAll(packet);
        }    
    }

    public static void PlayerHit(Player player)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_HIT))
        {
            packet.Write(player.id);
            packet.Write(player.health);
            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerDestroy(Player player, int killerId)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_DESTROY))
        {
            packet.Write(player.id);
            packet.Write(killerId);
            SendTCPDataToAll(packet);
        }
    }
    public static void PlayerRespawn(Player player, Vector3 respawnPosition)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_RESPAWN))
        {
            packet.Write(player.id);
            packet.Write(GameSettings.PLAYER_MAX_HEALTH);
            packet.Write(respawnPosition);
            SendTCPDataToAll(packet);
        }
    }
    
    public static void GameOver(int winner)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_GAME_OVER))
        {
            packet.Write(winner);
            SendTCPDataToAll(packet);
        }
    }
    public static void SpawnPowerUp(Player player)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_SPAWN_POWERUP))
        {
            packet.Write(player.transform.position);
            SendTCPDataToAll(packet);
        }
    }
    public static void PowerUp(Player player)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_PLAYER_POWERUP))
        {
            packet.Write(player.id); 
            packet.Write(player.weaponLevel);
            SendTCPDataToAll(packet);
        }
    }

    public static void SpawnAsteroid(Asteroid asteroid)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_ASTEROID_SPAWN))
        {
            packet.Write(asteroid.id);
            packet.Write(asteroid.type);
            packet.Write(asteroid.transform.position);
            packet.Write(asteroid.transform.rotation);
            packet.Write(asteroid.transform.localScale);
            SendTCPDataToAll(packet);
        }
    }

    public static void AsteroidPosition(Asteroid asteroid)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_ASTEROID_MOVEMENT))
        {
            packet.Write(asteroid.id);
            packet.Write(asteroid.type);
            packet.Write(asteroid.transform.position);
            SendUDPDataToAll(packet);
        }
    }
    public static void DestroyAsteroid(Asteroid asteroid)
    {
        using (CustomPacket packet = new CustomPacket((int)ServerPackets.SP_ASTEROID_DESTROY))
        {
            packet.Write(asteroid.id);
            SendTCPDataToAll(packet);
        }
    }
}
