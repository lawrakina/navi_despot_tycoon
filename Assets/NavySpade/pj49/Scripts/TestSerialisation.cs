using System;
using System.Collections.Generic;
using NavySpade.Modules.Utils.Serialization.Interfaces.Runtime;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEngine;

namespace NavySpade.pj49.Scripts
{
    public class TestSerialisation : MonoBehaviour
    {
        [SerializeField] private ResourceAsset _resources;
        
        private void Start()
        {
            // Test item = new Test();
            var list = new List<Item>()
            {
                new Item(_resources, 10),
                new Item(_resources, 10)
            };
            
            SaveableInventory sv = new SaveableInventory();
            sv.Items = list; 
            
            
            // List<Test> _items = new List<Test>();
            // _items.Add(item);
            // _items.Add(item);
            // _items.Add(item);
            // _items.Add(item);
            string result = JsonUtility.ToJson(sv);
            Debug.Log(result);
        }
    }
}