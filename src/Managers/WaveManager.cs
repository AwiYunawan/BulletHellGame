using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using BulletHellGame.GameObjects;
using System.Collections.Generic;

namespace BulletHellGame.Managers
{
    public class WaveManager
    {
        private List<Wave> _waves;
        private int _currentWaveIndex;
        private float _waveTimer;
        private bool _isWaveActive;
        
        public WaveManager()
        {
            _waves = new List<Wave>();
            _currentWaveIndex = 0;
            _waveTimer = 0f;
            _isWaveActive = false;
        }
        
        public void LoadContent(ContentManager content)
        {
            // Load wave configurations
            InitializeWaves();
        }
        
        private void InitializeWaves()
        {
            // Wave 1: Basic enemies
            var wave1 = new Wave
            {
                Enemies = new List<EnemySpawnInfo>
                {
                    new EnemySpawnInfo { EnemyType = typeof(Enemy), SpawnTime = 0f, SpawnPosition = new Vector2(100, -50) },
                    new EnemySpawnInfo { EnemyType = typeof(Enemy), SpawnTime = 1f, SpawnPosition = new Vector2(300, -50) },
                    new EnemySpawnInfo { EnemyType = typeof(Enemy), SpawnTime = 2f, SpawnPosition = new Vector2(500, -50) }
                },
                Duration = 10f
            };
            _waves.Add(wave1);
            
            // Wave 2: More enemies with different timing
            var wave2 = new Wave
            {
                Enemies = new List<EnemySpawnInfo>
                {
                    new EnemySpawnInfo { EnemyType = typeof(Enemy), SpawnTime = 0f, SpawnPosition = new Vector2(200, -50) },
                    new EnemySpawnInfo { EnemyType = typeof(Enemy), SpawnTime = 0.5f, SpawnPosition = new Vector2(400, -50) },
                    new EnemySpawnInfo { EnemyType = typeof(Enemy), SpawnTime = 1f, SpawnPosition = new Vector2(600, -50) },
                    new EnemySpawnInfo { EnemyType = typeof(Enemy), SpawnTime = 1.5f, SpawnPosition = new Vector2(100, -50) },
                    new EnemySpawnInfo { EnemyType = typeof(Enemy), SpawnTime = 2f, SpawnPosition = new Vector2(700, -50) }
                },
                Duration = 15f
            };
            _waves.Add(wave2);
        }
        
        public void Update(GameTime gameTime)
        {
            if (_currentWaveIndex >= _waves.Count)
            {
                // All waves completed
                return;
            }
            
            var currentWave = _waves[_currentWaveIndex];
            _waveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (!_isWaveActive)
            {
                StartWave(currentWave);
            }
            
            UpdateWave(currentWave);
            
            if (_waveTimer >= currentWave.Duration)
            {
                EndWave();
            }
        }
        
        private void StartWave(Wave wave)
        {
            _isWaveActive = true;
            _waveTimer = 0f;
        }
        
        private void UpdateWave(Wave wave)
        {
            foreach (var enemySpawn in wave.Enemies)
            {
                if (_waveTimer >= enemySpawn.SpawnTime && !enemySpawn.HasSpawned)
                {
                    SpawnEnemy(enemySpawn);
                    enemySpawn.HasSpawned = true;
                }
            }
        }
        
        private void SpawnEnemy(EnemySpawnInfo spawnInfo)
        {
            // Create and spawn enemy at specified position
            var enemy = new Enemy();
            enemy.Position = spawnInfo.SpawnPosition;
            // Add enemy to game world
        }
        
        private void EndWave()
        {
            _isWaveActive = false;
            _currentWaveIndex++;
            _waveTimer = 0f;
        }
        
        public bool IsWaveActive => _isWaveActive;
        public int CurrentWave => _currentWaveIndex + 1;
        public int TotalWaves => _waves.Count;
    }
    
    public class Wave
    {
        public List<EnemySpawnInfo> Enemies { get; set; }
        public float Duration { get; set; }
    }
    
    public class EnemySpawnInfo
    {
        public System.Type EnemyType { get; set; }
        public float SpawnTime { get; set; }
        public Vector2 SpawnPosition { get; set; }
        public bool HasSpawned { get; set; } = false;
    }
} 