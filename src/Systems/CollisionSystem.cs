using Microsoft.Xna.Framework;
using BulletHellGame.GameObjects;
using System.Collections.Generic;

namespace BulletHellGame.Systems
{
    public class CollisionSystem
    {
        public void CheckCollisions(Player player, List<Enemy> enemies, List<Bullet> playerBullets, 
                                  List<EnemyBullet> enemyBullets, List<PowerUp> powerUps)
        {
            // Check player vs enemies
            foreach (var enemy in enemies)
            {
                if (CheckCollision(player.Bounds, enemy.Bounds))
                {
                    player.TakeDamage();
                    enemy.TakeDamage();
                }
            }
            
            // Check player vs enemy bullets
            for (int i = enemyBullets.Count - 1; i >= 0; i--)
            {
                if (CheckCollision(player.Bounds, enemyBullets[i].Bounds))
                {
                    player.TakeDamage();
                    enemyBullets[i].Destroy();
                }
            }
            
            // Check player vs power-ups
            for (int i = powerUps.Count - 1; i >= 0; i--)
            {
                if (CheckCollision(player.Bounds, powerUps[i].Bounds))
                {
                    player.CollectPowerUp(powerUps[i]);
                    powerUps[i].Destroy();
                }
            }
            
            // Check player bullets vs enemies
            for (int i = playerBullets.Count - 1; i >= 0; i--)
            {
                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    if (CheckCollision(playerBullets[i].Bounds, enemies[j].Bounds))
                    {
                        enemies[j].TakeDamage();
                        playerBullets[i].Destroy();
                        break;
                    }
                }
            }
        }
        
        private bool CheckCollision(Rectangle bounds1, Rectangle bounds2)
        {
            return bounds1.Intersects(bounds2);
        }
    }
}
