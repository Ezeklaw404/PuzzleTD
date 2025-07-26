using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class TowerSelectionUI : MonoBehaviour
{
    [SerializeField] private RectTransform buttonContainer;
    [SerializeField] private GameObject buttonPrefab;  // root has Button + TextMeshProUGUI child

    private Action<GameObject> onSelected;

    public void Show(List<GameObject> towerOptions, Action<GameObject> onSelectedCallback)
    {
        onSelected = onSelectedCallback;

        // clear old buttons
        foreach (Transform t in buttonContainer)
            Destroy(t.gameObject);

        // helper to size a button to its TMP child
        void SizeButton(GameObject go, float padX, float padY)
        {
            var tmp = go.GetComponentInChildren<TextMeshProUGUI>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(tmp.rectTransform);

            float w = LayoutUtility.GetPreferredSize(tmp.rectTransform, 0);
            float h = LayoutUtility.GetPreferredSize(tmp.rectTransform, 1);

            var rt = go.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w + padX);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h + padY);
        }

        // spawn a button per tower type
        foreach (var prefab in towerOptions)
        {
            var btnGO = Instantiate(buttonPrefab, buttonContainer);
            var btn = btnGO.GetComponent<Button>();
            var tmp = btnGO.GetComponentInChildren<TextMeshProUGUI>();

            tmp.text = prefab.name;
            SizeButton(btnGO, padX: 20f, padY: 10f);

            btn.onClick.AddListener(() => {
                onSelected?.Invoke(prefab);
                Destroy(gameObject);
            });
        }

        // spawn the Cancel button
        var cancelGO = Instantiate(buttonPrefab, buttonContainer);
        var cancelBtn = cancelGO.GetComponent<Button>();
        var cancelTmp = cancelGO.GetComponentInChildren<TextMeshProUGUI>();

        cancelTmp.text = "Cancel";
        SizeButton(cancelGO, padX: 20f, padY: 10f);

        cancelBtn.onClick.AddListener(() => {
            onSelected?.Invoke(null);
            Destroy(gameObject);
        });
    }
}
