using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {
    static private List<Item> _items;

    static private bool _isDatabaseLoaded = false;

    static private void ValidateDatabase()
    {
        if (_items == null) _items = new List<Item>();
        if (!_isDatabaseLoaded) LoadDatabase();
    }

    static public void LoadDatabase()
    {
        if (_isDatabaseLoaded) return;
        _isDatabaseLoaded = true;
        LoadDatabaseForce();
    }

    static public void LoadDatabaseForce()
    {
        ValidateDatabase();
        Item[] resources = Resources.LoadAll<Item>(@"Items");
        foreach (Item item in resources)
        {
            if(!_items.Contains(item))
            {
                _items.Add(item);
            }
        }
    }

    static public void ClearDatabase()
    {
        _isDatabaseLoaded = false;
        _items.Clear();
    }

    static public Item GetItem(int id)
    {
        ValidateDatabase();
        foreach (Item item in _items)
        {
            if (item.itemID == id)
            { return ScriptableObject.Instantiate(item) as Item; } }
        return null;
        }
}
