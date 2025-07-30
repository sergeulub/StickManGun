public static class WeaponFactory
{
    public static Weapon CreateWeapon(Weapons data)
    {
        return data.weaponType switch
        {
            WeaponType.Pistol => new Pistol(data),
            WeaponType.Shotgun => new Shotgun(data),
            WeaponType.Rifle => new Rifle(data),
            WeaponType.Sniper => new Sniper(data),
            WeaponType.Flame => new Flame(data),
            WeaponType.PP => new PP(data),
            WeaponType.Minigun => new Minigun(data),
            WeaponType.P90 => new P90(data),
            _ => null
        };
    }
}