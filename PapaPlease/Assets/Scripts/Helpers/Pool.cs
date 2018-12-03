using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T>
{
    public delegate T CreatePoolItemCallback(T item, Transform itemsParent = null);

    Transform _itemsParent;

    public static U DelGameObjectItemCreate<U>(U actorRef, Transform itemsParent = null) where U : Component
    {
        U newItem = Object.Instantiate(actorRef) as U;
        if (itemsParent != null)
            newItem.transform.parent = itemsParent;
        newItem.gameObject.SetActive(false);
        return newItem;
    }

    [SerializeField]
    private List<T> poolItems;
    [SerializeField]
    private List<T> borrowedItems;
    public List<T> GetBorrowedItems { get { return new List<T>(borrowedItems); } }

    CreatePoolItemCallback CreatePoolItem;

    T importedItemRef;


    private bool _isPoolReady = false;
    public bool IsPoolReady { get { return _isPoolReady; } }

    public Pool() { }

    public Pool(T itemRef, CreatePoolItemCallback createPoolItemCallback, int poolStartSize, Transform itemsParent = null)
    {
        if (itemsParent != null)
            _itemsParent = itemsParent;
        InitPool(itemRef, createPoolItemCallback, poolStartSize);
    }

    public List<T> InitPool(T itemRef, CreatePoolItemCallback delPoolItemCreate, int poolStartSize)
    {
        if (_isPoolReady == false)
        {
            _isPoolReady = true;

            importedItemRef = itemRef;

            poolItems = new List<T>();
            borrowedItems = new List<T>();

            CreatePoolItem = delPoolItemCreate;


            for (int i = 0; i < poolStartSize; i++)
            {
                T newItem = CreatePoolItem(itemRef, _itemsParent);
                poolItems.Add(newItem);
            }
            return poolItems;
        }
        return null;
    }

    public T BorrowItem()
    {
        T toReturn = default(T);
        if (poolItems.Count > 0)
            toReturn = poolItems[0];

        if (EqualityComparer<T>.Default.Equals(toReturn, default(T))) // check if toReturn has not been set
        {
            toReturn = CreatePoolItem(importedItemRef, _itemsParent);
            borrowedItems.Add(toReturn);
        }
        if (toReturn != null)
        {
            poolItems.Remove(toReturn);
            borrowedItems.Add(toReturn);
        }
        else
            Debug.LogError("Could'nt return item");
        return toReturn;
    }

    public List<T> BorrowListOfItems(int number)
    {
        List<T> toReturn = new List<T>();

        for (int i = 0; i < poolItems.Count; i++)
        {
            toReturn.Add(poolItems[i]);
            if (toReturn.Count >= number)
                break;
        }
        foreach (T curT in toReturn)
        {
            poolItems.Remove(curT);
            borrowedItems.Add(curT);
        }
        if (toReturn.Count < number)
        {
            T createdItem = default(T);
            for (int i = 0; i < number - toReturn.Count; i++)
            {
                createdItem = CreatePoolItem(importedItemRef, _itemsParent);
                toReturn.Add(createdItem);
                borrowedItems.Add(createdItem);
            }
        }
        return toReturn;
    }

    public void BringBackItem(T broughtItem)
    {
        poolItems.Add(broughtItem);
        if (borrowedItems.Contains(broughtItem))
            borrowedItems.Remove(broughtItem);
    }

    public void BringBackAllItems()
    {
        foreach (var item in borrowedItems)
            poolItems.Add(item);
        borrowedItems.Clear();
    }

    public void MakeReachSize(int desiredSize)
    {
        if (_isPoolReady == false)
        {
            Debug.LogError("Pool not ready for MakeReachSize");
            return;
        }
        if (desiredSize > GetBorrowedItems.Count)
        {
            for (int i = 0; i < desiredSize - GetBorrowedItems.Count; i++)
            {
                T newItem = CreatePoolItem(importedItemRef, _itemsParent);
                poolItems.Add(newItem);
            }
        }
    }
}
