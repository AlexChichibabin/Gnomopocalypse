public static class CompatibilityExtention
{
    public static bool IsMainDamage(UnitType unitType, ProjectileType projectileType)
    {
        return unitType switch
        {
            UnitType.Smelly => projectileType == ProjectileType.AirFreshener,
            UnitType.Dirty => projectileType == ProjectileType.Soap,
            UnitType.Leaking => projectileType == ProjectileType.Rag,
            UnitType.Sticky => projectileType == ProjectileType.BottleOfWater,
            _ => false
        };
    }
}
