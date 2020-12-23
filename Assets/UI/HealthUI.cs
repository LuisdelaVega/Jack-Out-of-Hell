using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [Header("Refereces")]
    public GameObject heart;
    public GameObject emptyHeart;

    private Transform m_transform;
    private HealthBehaviour characterHealth;

    private List<GameObject> instantiatedHearts = new List<GameObject>();

    private void Awake()
    {
        m_transform = transform;
        characterHealth = GetComponentInParent<HealthBehaviour>();
    }

    private void OnEnable() => HealthBehaviour.OnHealthUpdate += SetHealthUI;
    private void OnDisable() => HealthBehaviour.OnHealthUpdate -= SetHealthUI;
    void Start() => SetHealthUI();

    private void SetHealthUI(HealthBehaviour healthBehaviour)
    {
        if (healthBehaviour.Equals(characterHealth))
            SetHealthUI();
    }
    private void SetHealthUI()
    {
        DestroyInstantiatedHearts();

        for (int i = 0; i < characterHealth.MaxHealth; i++)
        {
            GameObject heartObject;
            if (i + 1 > characterHealth.CurrentHealth)
                heartObject = Instantiate(emptyHeart, m_transform);
            else
                heartObject = Instantiate(heart, m_transform);

            instantiatedHearts.Add(heartObject);
        }
    }

    private void DestroyInstantiatedHearts()
    {
        foreach (var heart in instantiatedHearts)
            Destroy(heart);

        instantiatedHearts.Clear();
    }
}
