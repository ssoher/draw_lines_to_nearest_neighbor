/*
Ironbelly Programming Test
Sarper Soher - https://www.accidentalee.com
Mar 04 2021 - 09:25
*/

using UnityEngine;
using UnityEngine.UI;

public sealed class ApplicationControls : MonoBehaviour {
    public Pool CubePool;
    public GameObject CubePrefab;
    public Text ActiveObjectCountLabel;

    private int _activateCount;

    private void Awake() {
        _activateCount = 1;
    }

    private void OnEnable() {
        Pool.ItemActivated += OnItemActivated;
        Pool.ItemExpired += OnItemExpired;
    }

    private void OnDisable() {
        Pool.ItemActivated -= OnItemActivated;
        Pool.ItemExpired -= OnItemExpired;
    }

    private void OnItemActivated() {
        RefreshActiveObjectCountLabel();
    }

    private void OnItemExpired() {
        RefreshActiveObjectCountLabel();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.A)) {
            ActivateCubes();
        } else if(Input.GetKeyDown(KeyCode.R)) {
            CubePool.ExpireItem();
        }
    }

    // NOTE(sarper Mar 04 21): Called by `InputFieldCubeCount`s OnEndEdit event
    public void SetActivateCount(string value) {
        _activateCount = int.Parse(value);
    }

    // NOTE(sarper Mar 04 21): Also called by `ButtonAdd` OnClick event
    public void ActivateCubes() {
        for(int i = 0; i < _activateCount; i++) {
            CubePool.ActivateItem(CubePrefab, MovementSceneBounds.Instance.RandomPosition());
        }
    }

    private void RefreshActiveObjectCountLabel() {
        ActiveObjectCountLabel.text = $"Active Objects:{CubePool.ActiveList.Count}";
    }
}