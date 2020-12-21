using System;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int maxHealth = 100;
    public int MaxHealth => maxHealth;
    [SerializeField] private int currentHealth;
    public int CurrentHealth
    {
        get => currentHealth;
        private set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthUpdate?.Invoke(currentHealth, gameObject);
        }
    }

    private Combatant combatant;

    public static event Action<int, GameObject> OnHealthUpdate;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        combatant = GetComponent<Combatant>();
    }

    private void Add(int value) => CurrentHealth += Mathf.Max(value, 0);

    private int Remove(int value)
    {
        CurrentHealth -= Mathf.Max(value, 0);

        if (CurrentHealth == 0)
            combatant.Die();
        else
            combatant.Damaged();

        return CurrentHealth;
    }

    public void Heal(int value) => Add(value);
    public int DealDamage(int value) => Remove(value);
}
