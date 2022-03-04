using UnityEngine;


public interface IDamageable
{
    void Damage(float damage);
    void SendIdToDamage(int idToDamage, float damage);
    bool CheckIfMine();
    int ID { get; }
    GameObject Instance { get; }
}