using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class TowerPedestal : MonoBehaviour, IPointerClickHandler
{
    [Header("Which Towers Can Be Placed Here")]
    [Tooltip("Drag in all tower prefabs you want to be able to select")]
    [SerializeField] private List<GameObject> towerPrefabs;

    [Header("UI Prefab for Selection Menu")]
    [Tooltip("A prefab with TowerSelectionUI on it")]
    [SerializeField] private TowerSelectionUI towerSelectionUIPrefab;

    private GameObject currentTower;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (towerSelectionUIPrefab == null || towerPrefabs == null || towerPrefabs.Count == 0)
        {
            UnityEngine.Debug.LogWarning($"[{name}] No tower types configured!");
            return;
        }

        // spawn the selection UI and hook up our callback
        var ui = Instantiate(towerSelectionUIPrefab);
        ui.Show(towerPrefabs, OnTowerChosen);
    }

    private void OnTowerChosen(GameObject towerPrefab)
    {
        if (towerPrefab == null) return;

        if (currentTower != null)
            Destroy(currentTower);

        // instantiate the chosen tower at our position
        currentTower = Instantiate(
            towerPrefab,
            transform.position + new Vector3(0, 0.035f, 0),
            Quaternion.identity,
            transform   // parent to pedestal if you like
        );
    }

    public bool HasTower => currentTower != null;
}
