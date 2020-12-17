using UnityEngine;

public class CombatMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject buttons = null;

    private void OnEnable()
    {
        EnemySpawner.OnEnemySpawned += EnableButtons;
        GameManager.OnCombatEnded += DisableButtons; // TODO: This will probably be broadcasted by the combatants instead of the GameManager
    }

    private void OnDisable()
    {
        EnemySpawner.OnEnemySpawned -= EnableButtons;
        GameManager.OnCombatEnded -= DisableButtons;
    }
    private void EnableButtons() => buttons.SetActive(true);
    private void DisableButtons() => buttons.SetActive(false);
}
