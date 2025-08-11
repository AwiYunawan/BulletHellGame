using System;

namespace BulletHellGame.Managers
{
    public class ShopManager
    {
        public int Money { get; private set; } = 1000;
        
        public ShopManager()
        {
            // Initialize shop with default money
        }
        
        public bool BuyWeaponUpgrade()
        {
            if (Money >= 1000)
            {
                Money -= 1000;
                // Apply weapon upgrade logic here
                return true;
            }
            return false;
        }
        
        public bool BuyShieldUpgrade()
        {
            if (Money >= 800)
            {
                Money -= 800;
                // Apply shield upgrade logic here
                return true;
            }
            return false;
        }
        
        public bool BuySpeedUpgrade()
        {
            if (Money >= 600)
            {
                Money -= 600;
                // Apply speed upgrade logic here
                return true;
            }
            return false;
        }
        
        public void AddMoney(int amount)
        {
            Money += amount;
        }
        
        public void SpendMoney(int amount)
        {
            if (Money >= amount)
            {
                Money -= amount;
            }
        }
    }
}
