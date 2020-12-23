using TMPro;
using UnityEngine;

public class ModifierUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject attackModifierCanvas;
    [SerializeField] private TextMeshProUGUI attackModifierText;

    private Combatant combatant;

    private void Awake()
    {
        if (combatant == null)
            combatant = GetComponentInParent<Combatant>();
    }

    private void OnEnable()
    {
        EnemySpawner.OnEnemySpawned += HandleCombatStart;
        Enemy.OnHasDied += HandleCombatEnd;
        PlayerCombat.OnHasDied += HandleCombatEnd;
    }
    private void OnDisable()
    {
        EnemySpawner.OnEnemySpawned -= HandleCombatStart;
        Enemy.OnHasDied -= HandleCombatEnd;
        PlayerCombat.OnHasDied -= HandleCombatEnd;
    }

    private void HandleCombatStart<T>(T _)
    {
        attackModifierCanvas.SetActive(true);

        // Set the text equal to the modifier amount
        attackModifierText.text = $"{combatant.AttackModifier}";
    }

    private void HandleCombatEnd() => attackModifierCanvas.SetActive(false);
}
