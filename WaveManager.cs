using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BulletHellGame
{
    public class WaveManager
    {
        private float spawnTimer;
        private float timeBetweenSpawns = 1.5f;

        private int currentWave = 0;
        public int CurrentWave => currentWave;
        private bool waveInProgress = false;

        private Game1 _game;

        public WaveManager(Game1 game)
        {
            _game = game;
        }

        public void Update(GameTime gameTime)
        {
            if (!waveInProgress)
            {
                currentWave++;
                StartWave(currentWave);
                waveInProgress = true;
            }

            spawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (spawnTimer <= 0)
            {
                spawnTimer = timeBetweenSpawns;
                bool allEnemiesSpawned = SpawnEnemiesForWave(currentWave);

                if (allEnemiesSpawned)
                {
                    waveInProgress = false;
                }
            }

            // Jika semua musuh dihabisi → lanjut wave
            if (waveInProgress && _game.Enemies.Count == 0 && _game.Boss == null)
            {
                waveInProgress = false;
            }
        }

        private void StartWave(int waveNumber)
        {
            Console.WriteLine($"Wave {waveNumber} started!");
        }

        private int enemiesToSpawn = 0;
        private int spawnedEnemies = 0;

        private bool SpawnEnemiesForWave(int waveNumber)
        {
            if (spawnedEnemies == 0)
            {
                // Set jumlah musuh berdasarkan wave
                if (waveNumber % 3 == 0)
                {
                    // Wave boss
                    _game.SpawnBoss();
                    spawnedEnemies = 1;
                    enemiesToSpawn = 1;
                    return true;
                }
                else
                {
                    enemiesToSpawn = 5 + waveNumber * 2;
                    spawnedEnemies = 0;
                }
            }

            if (spawnedEnemies < enemiesToSpawn)
            {
                _game.SpawnEnemy(waveNumber);
                spawnedEnemies++;
                return false;
            }

            return true;
        }

        public void Reset()
        {
            currentWave = 0;
            enemiesToSpawn = 0;
            spawnedEnemies = 0;
            waveInProgress = false;
        }
    }
} 