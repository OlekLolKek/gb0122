public interface IDamageable
{
    void Damage(float damage);
    void SendIdToDamage(int idToDamage, float damage);
    int ID { get; }
    bool CheckIfMine();
}