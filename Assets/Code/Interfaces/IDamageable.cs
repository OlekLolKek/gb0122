using UnityEngine;


public interface IDamageable
{
    void Damage(float damage, IDamageable sender);
    void SendIdToDamage(int idToDamage, float damage);
    bool CheckIfMine();
    int ID { get; }
    string Nickname { get; }
    bool IsDead { get; }
    GameObject Instance { get; }
    void SetScore(int kills, int deaths, int score);
}