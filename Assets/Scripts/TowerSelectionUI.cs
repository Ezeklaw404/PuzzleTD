using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class TowerSelectionUI : MonoBehaviour
{
    [Tooltip("Parent RectTransform where buttons will be spawned")]
    [SerializeField] private RectTransform buttonContainer;

    [Tooltip("A simple Button prefab with a Text child")]
    [SerializeField] private Button buttonPrefab;

    private Action<GameObject> onSelected;

    /// <summary>
    /// Call this to initialize the menu.
    /// </summary>
    public void Show(List<GameObject> towerOptions, Action<GameObject> onSelectedCallback)
    {
        onSelected = onSelectedCallback;

        // create one button per tower prefab
        foreach (var prefab in towerOptions)
        {
            var btn = Instantiate(buttonPrefab, buttonContainer);
            btn.GetComponentInChildren<UnityEngine.UI.Text>().text = prefab.name;
            btn.onClick.AddListener(() => Select(prefab));
        }
    }

    private void Select(GameObject prefab)
    {
        onSelected?.Invoke(prefab);
        Destroy(gameObject);
    }
}
