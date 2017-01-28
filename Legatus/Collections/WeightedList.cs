using System;
using System.Collections;
using System.Collections.Generic;

namespace Legatus.Collections
{
    public sealed class WeightedList<T> : IEnumerable<T>
    {
        private static Random RNG = new Random();
        
        private List<WeightedListNode<T>> _Items = new List<WeightedListNode<T>>();
        private double _TotalWeight;
        
        public T this[int i] { get { return _Items[i].Item; } }
        public double TotalWeight { get { return _TotalWeight; } }
        public int Count { get { return _Items.Count; } }

        public WeightedList()
        {
        }

        public void Add(T item, double weight)
        {
            _Items.Add(new WeightedListNode<T>(item, weight));
            _TotalWeight += weight;
        }

        public void AddRange(IEnumerable<WeightedListNode<T>> items)
        {
            foreach (WeightedListNode<T> node in items)
            {
                Add(node.Item, node.Weight);
            }
        }

        public void Remove(T item)
        {
            int i = _Items.FindIndex(e => e.Item.Equals(item));
            _TotalWeight -= _Items[i].Weight;
            _Items.RemoveAt(i);
        }

        public void RemoveAt(int index)
        {
            _TotalWeight -= _Items[index].Weight;
            _Items.RemoveAt(index);
        }
        
        public void Clear()
        {
            _Items.Clear();
            _TotalWeight = 0;
        }
        
        public void ChangeWeight(T item, double newWeight)
        {
            int i = _Items.FindIndex(e => e.Item.Equals(item));
            _TotalWeight -= _Items[i].Weight;
            _TotalWeight += newWeight;
            _Items[i].Weight = newWeight;
        }

        public double GetWeightAt(int i)
        {
            return _Items[i].Weight;
        }
        
        /// <summary>
        /// Returns default(T) if the list is empty.
        /// </summary>
        /// <returns></returns>
        public T SelectRandom()
        {
            double rn = RNG.NextDouble() * _TotalWeight;
            for (int i = 0; i < _Items.Count; i++)
            {
                if (rn <= _Items[i].Weight)
                {
                    return _Items[i].Item;
                }
                rn -= _Items[i].Weight;
            }
            return default(T);
        }

        /// <summary>
        /// Returns -1 if the list is empty.
        /// </summary>
        /// <returns></returns>
        public int SelectRandomIndex()
        {
            int i = -1;
            double rn = RNG.NextDouble() * _TotalWeight;
            while (i < _Items.Count)
            {
                i++;
                if (rn <= _Items[i].Weight)
                {
                    break;
                }
                rn -= _Items[i].Weight;
            }
            return i;
        }

        /// <summary>
        /// Returns default(T) if the list is empty.
        /// </summary>
        /// <returns></returns>
        public T PopRandom()
        {
            int i = SelectRandomIndex();
            if (i >= 0)
            {
                T item = this[i];
                RemoveAt(i);
                return item;
            }
            return default(T);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //return _Items.GetEnumerator();
            foreach (WeightedListNode<T> item in _Items)
            {
                yield return item.Item;
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            //return _Items.GetEnumerator();
            foreach (WeightedListNode<T> item in _Items)
            {
                yield return item.Item;
            }
        }
    }

    public class WeightedListNode<T>
    {
        public T Item;
        public double Weight;

        public WeightedListNode(T item, double weight)
        {
            Item = item;
            Weight = weight;
        }
    }
}
