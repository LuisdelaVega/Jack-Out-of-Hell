using TMPro;
using UnityEngine;

public class ModifierUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI attackModifier;

    private Combatant combatant;

    private void Awake()
    {
        combatant = GetComponentInParent<Combatant>();

        // Set the text equal to the modifier amount
        attackModifier.text = $"{combatant.AttackModifier}";
    }

    // TODO: Listen for changes in the modifier and update accordingly
}
