
using System.Collections;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var array = new SmartStack<int>(5);
        array.Push(1);
        array.Push(2);
        array.Push(3);
        array.Push(4);
        array.Push(5);
        array.Push(6);
        foreach (var item in array)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine(array.Count());
        Console.WriteLine(array.Capacity());
    }
    private class SmartStack<T> : IEnumerable<T>
    {
        private T[] _array;
        private int _size = 0;
        private T? _defaultItem;
        

        public int Length => _array.Length;
        public int elementsCount => _size;

        public T this[int index]
        {
            get => _array[_size - 1 - index];
            set => _array[_size - 1 - index] = value;
        }

        public SmartStack()
        {
            _array = new T[4];
        }

        public SmartStack(int length)
        {
            _array = new T[length];
        }

        public SmartStack(IEnumerable<T> collection)
        {
            T[] tempArrayFromCollection = collection.ToArray();
            _array = new T[tempArrayFromCollection.Length];
            for (int i = 0;  i < tempArrayFromCollection.Length; i++)
            {
                _array[i] = tempArrayFromCollection[i];
            }
            _size = tempArrayFromCollection.Length;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _size - 1; i >= 0; i--)
            {
                yield return _array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Resize()
        {
            T[] tempArray = _array;
             
            _array = new T[tempArray.Length * 2];
            for (int i = 0; i < _array.Length; i++)
            {
                if (i < tempArray.Length)
                    _array[i] = tempArray[i];
                else
                    _array[i] = _defaultItem!;
            }
        }

        public void Push(T item)
        {
            if (_size == _array.Length)
                Resize();
            _array[_size] = item;
            _size++;
        }

        public void PushRange (IEnumerable<T> collection)
        {
            T[] arrayNewItems = collection.ToArray();
            if (arrayNewItems.Length == 0)
            {
                Console.WriteLine("Массив пуст");
                return;
            }
            while (_size + arrayNewItems.Length > _array.Length)
            {
                Resize();
            }

            for (int i = 0; i < arrayNewItems.Length; i++)
            {
                _array[_size] = arrayNewItems[i];
                _size++; 
            }

        }

        public T Pop()
        {
            if (_array.Length == 0 || _size == 0)
                throw new InvalidOperationException("Стек пуст");
            int topIndex = _size - 1;
            T poppedItem = _array[topIndex];
            _array[topIndex] = _defaultItem!;
            _size--;
            return poppedItem;
        }

        public T Peek()
        {
            if (_array.Length == 0 || _size == 0)
                throw new InvalidOperationException("Стек пуст");
            int topIndex = _size - 1;
            T peekedItem = _array[topIndex];
            return peekedItem;
        }

        public bool Contains (T item)
        {
            for (int i = 0; i < _size; i++)
            {
                if (_array[i].Equals(item))
                    return true;
            }
            return false;
        }

        public int Count()
        {
            return _size;
        }

        public int Capacity()
        {
            return _array.Length;
        }

    }
}
