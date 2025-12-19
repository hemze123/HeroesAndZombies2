public interface IWeaponAttackStrategy
{
    void Attack(Weapon weapon);
    bool CanAttack(Weapon weapon);
}
