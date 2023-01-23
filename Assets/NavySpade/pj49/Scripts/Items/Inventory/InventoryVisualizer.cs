using System;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Items.Inventory
{
    public abstract class InventoryVisualizer : MonoBehaviour
    {
        [SerializeField] protected ResourcesInventory _inventory;
    }
}