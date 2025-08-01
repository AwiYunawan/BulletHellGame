using Microsoft.Xna.Framework;

namespace BulletHellGame
{
    public class Player
    {
        public int Health { get; private set; }
        public bool HasShield { get; set; } = false;
        public bool DoubleBullet { get; private set; } = false;

        public Player(int initialHealth = 3)
        {
            Health = initialHealth;
        }

        public void AddLife()
        {
            Health++;
        }

        public void ActivateShield()
        {
            HasShield = true;
        }

        public void EnableDoubleBullet()
        {
            DoubleBullet = true;
        }

        public void TakeDamage()
        {
            if (HasShield)
            {
                HasShield = false;
                // Shield breaks, but player doesn't take damage
            }
            else
            {
                Health--;
            }
        }

        public bool IsDead => Health <= 0;
    }
} 