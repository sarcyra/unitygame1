using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private string _mainPointTag;
    [SerializeField] private Vector3 _size;
    [SerializeField] private Transform _pivot;

    private ItemFactory _factory;

    private Item _itemPrefab;
    
    private Stack<GameObject> _itemStack;

    private int _maxCount;

    private void Start()
    {
        GameObject mainPoint = GameObject.FindWithTag(_mainPointTag);
        _factory = mainPoint.GetComponent<ItemFactory>();
        _itemStack = new Stack<GameObject>();

        setPlaceEmpty();
    }

    private void setPlaceEmpty()
    {
        _itemPrefab = _factory.Get(ItemsConsts.ItemIndificator.Empty);
        _maxCount = 0;
    }


    public bool IsItemPlaceable(ItemsConsts.ItemIndificator toPlace)
    {
        
        bool e =  (_itemPrefab._itemIndificator == toPlace || _itemPrefab._itemIndificator == ItemsConsts.ItemIndificator.Empty) && (_maxCount == 0 || _maxCount > _itemStack.Count);
        return e;
    }

    public bool IsItemDecplaceable()
    {
        return _itemStack.Count > 0;
    }

    public void AddNewItem(ItemsConsts.ItemIndificator toPlace)
    {
        if(_itemPrefab._itemIndificator == ItemsConsts.ItemIndificator.Empty)
        {
            _itemPrefab = _factory.Get(toPlace);
            _maxCount = (int)(_size.x / _itemPrefab._size.x) * (int)(_size.z / _itemPrefab._size.z);
        } 

        placeItem();
    }

    public void DestroyLastItem() {
        GameObject item = _itemStack.Pop();

        Destroy(item);

        if(_itemStack.Count == 0)
        {
            setPlaceEmpty();
        }
    }

    private void placeItem()
    { 
        float deltaZ = (_itemStack.Count / (int)(_size.x / _itemPrefab._size.x)) * _itemPrefab._size.x;
        float deltaX = (_itemStack.Count % (int)(_size.x / _itemPrefab._size.x)) * _itemPrefab._size.z;

        Vector3 nv = (_itemPrefab._size.x / 2 + deltaX) * transform.right + (_itemPrefab._size.z / 2 + deltaZ) * transform.forward;

        float x = nv.x + _pivot.position.x;
        float z = nv.z + _pivot.position.z;

        _itemStack.Push(Instantiate(_itemPrefab._gameBody, new Vector3(x, transform.position.y, z), transform.rotation));
    }

}
