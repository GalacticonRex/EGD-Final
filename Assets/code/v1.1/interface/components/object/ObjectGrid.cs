using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastStar {
    public class ObjectGrid : MonoBehaviour {

        public int Dimension;
        public GameObject Cell;

        private int _row_count;
        private int _active_elements;
        private List<GameObject> _elements = new List<GameObject>();
        private int _hover_index;

        public void SetHoverIndex(int index)
        {
            _hover_index = index;
        }
        public int GetHoverIndex()
        {
            return (_hover_index>=_elements.Count||!_elements[_hover_index].GetComponent<UnityEngine.UI.Button>().interactable)?-1:_hover_index;
        }
        public int GetIndexOfElement(GameObject go)
        {
            return _elements.IndexOf(go);
        }
        public int GetIndexOfMounted(object obj)
        {
            for (int i=0;i<_elements.Count;i++)
            {
                ObjectGridItem item = _elements[i].GetComponent<ObjectGridItem>();
                if (item.Mounted == obj)
                    return i;
            }
            return -1;
        }

        public void PushElement(object item)
        {
            PushElement(_active_elements, item);
        }
        public void PushElement(int index, object item_mount)
        {
            if (index > _active_elements)
                index = _active_elements;

            print("Insert to " + index);

            object data = item_mount;

            int i = index;
            for (; i < _active_elements; i++)
            {
                ObjectGridItem next_item = _elements[i].GetComponent<ObjectGridItem>();
                object temp = next_item.Mounted;
                next_item.Mounted = data;
                next_item.OnMountChange.Invoke();
                data = temp;
            }
            _active_elements++;
            while ( _active_elements >= _elements.Count )
                AddRow();
            
            ObjectGridItem last_item = _elements[i].GetComponent<ObjectGridItem>();
            UnityEngine.UI.Button but = _elements[i].GetComponent<UnityEngine.UI.Button>();
            last_item.Mounted = data;
            last_item.OnMountChange.Invoke();
            but.interactable = true;
        }
        public object PopElement(int index)
        {
            if (index >= _active_elements)
                return null;

            ObjectGridItem item = _elements[index].GetComponent<ObjectGridItem>();
            object obj = item.Mounted;

            int i = index + 1;
            for ( ;i<_active_elements; i++ )
            {
                ObjectGridItem next_item = _elements[i].GetComponent<ObjectGridItem>();

                item.Mounted = next_item.Mounted;
                item.OnMountChange.Invoke();

                print(item.Mounted);
                
                item = next_item;
            }

            item.Mounted = null;
            item.OnMountChange.Invoke();

            UnityEngine.UI.Button but = _elements[i-1].GetComponent<UnityEngine.UI.Button>();
            but.interactable = false;

            _active_elements--;

            return obj;
        }

        private void AddRow()
        {
            RectTransform self_transform = GetComponent<RectTransform>();
            RectTransform src_transform = Cell.GetComponent<RectTransform>();

            Vector2 celldim = new Vector2(src_transform.rect.width, src_transform.rect.height);
            Vector2 location = new Vector2(celldim.x / 2, celldim.y * _row_count + celldim.y / 2);

            for (int x = 0; x < Dimension; x++)
            {
                GameObject cell = Instantiate(Cell);
                cell.transform.SetParent(self_transform);
                cell.transform.localPosition = new Vector2(location.x, -location.y);
                cell.transform.localScale = new Vector3(1, 1, 1);

                UnityEngine.UI.Button but = cell.GetComponent<UnityEngine.UI.Button>();
                but.interactable = false;

                ObjectGridItem item = cell.GetComponent<ObjectGridItem>();
                item.Index = _elements.Count;
    
                _elements.Add(cell);
                location.x += celldim.x;
            }

            self_transform.sizeDelta += new Vector2(0, celldim.y);
            _row_count++;
        }
        private void RemoveRow()
        {

        }

        public void Initialize() {
            _row_count = 0;
            _active_elements = 0;

            RectTransform self_transform = GetComponent<RectTransform>();
            RectTransform src_transform = Cell.GetComponent<RectTransform>();

            self_transform.sizeDelta = new Vector2(0, 0);
            for (int y = 0; y < Dimension; y++)
                AddRow();
        }
    }
}