﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Sever
{
    class Player
    {
        public int id;
        public string username;
        public Vector3 position;
        public Vector3 velocity;
        public float rotation;
        public float heading;

        private float moveSpeed = 5f / Constants.TICKS_PER_SEC;
        private bool[] inputs;
        public Player(int _id, string _username, Vector3 spawnPosition)
        {
            id = _id;
            username = _username;
            position = spawnPosition;
            rotation = 0.0f;
            inputs = new bool[4];
        }

        public void Update()
        {
            Vector2 inputDirection = Vector2.Zero;
            if (inputs[0] == true)
            {
                //velocity.X += MathF.Cos(Constants.Deg2Rad * rotation + 90.0f);
                //velocity.Y += MathF.Sin(Constants.Deg2Rad * rotation + 90.0f);
                inputDirection.Y += 1f;
            }
            if (inputs[1] == true)
            {
                inputDirection.Y -= 1f; 
            }
            if (inputs[2] == true)
            {
                inputDirection.X += 1f;
                //rotation += 3f;
                //rotation %= 360;
            }
            if (inputs[3] == true)
            {
                inputDirection.X -= 1f;
                //rotation -= 3f;
                //rotation %= 360;
            }

            Move(inputDirection);
        }

        private void Move (Vector2 inputDirection)
        {
            position.X += inputDirection.X * moveSpeed;
            position.Y += inputDirection.Y * moveSpeed;
            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }
        
        public void SetInputs (bool[] _inputs, float _rotation)
        {
            inputs = _inputs;
            rotation = _rotation;
        }
    }
}
