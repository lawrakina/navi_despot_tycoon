using System;
using System.Collections;
using System.Collections.Generic;
using NavySpade.pj49.Scripts.UnitsQueues.Positions;
using NavySpade.pj49.Scripts.Zone;
using UnityEngine;

[ExecuteInEditMode]
public class Zone : MonoBehaviour
{
    [SerializeField] private ZoneVisual _visual;
    [SerializeField] private BoxCollider _boundsCollider;
    [SerializeField] private PointsHolder _pointsHolder;
    [SerializeField] private Vector3 _colliderPadding;

    private void Update()
    {
        UpdateAreaSize();
    }

    private void UpdateAreaSize()
    {
        Vector2 size = _pointsHolder.RectSize;
        _boundsCollider.size = new Vector3(
            size.x + _colliderPadding.x,
            _colliderPadding.y,
            size.y + _colliderPadding.z);
    }
}
