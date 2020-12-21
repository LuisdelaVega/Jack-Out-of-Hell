using UnityEngine;

public class CombatMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject buttons = null;

    private void OnEnable()
    {
        EnemySpawner.OnPlayerMoved += EnableButtons;
        Enemy.OnHasDied += DisableButtons;
        PlayerCombat.OnHasDied += DisableButtons;
    }

    private void OnDisable()
    {
        EnemySpawner.OnPlayerMoved -= EnableButtons;
        Enemy.OnHasDied -= DisableButtons;
        PlayerCombat.OnHasDied -= DisableButtons;
    }

    private void EnableButtons() => buttons.SetActive(true);
    private void DisableButtons() => buttons.SetActive(false);
}
