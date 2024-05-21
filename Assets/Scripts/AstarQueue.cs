using System;
using System.Collections.Generic;

//For priority queue / heap storage simulation for AStar needs
public class AstarQueue<T> where T : IComparable<T>
{
    //Stores nodes
    private List<T> _queue = new();
    
    public int Count => _queue.Count;

    public bool Contains(T item)
    {
        return _queue.Contains(item);
    }
    
    public void Enqueue(T item)
    {
        _queue.Add(item);
        var childId = _queue.Count - 1;

        //Minimal heap assurance
        while (childId > 0)
        {
            //Binary heap parenting rule
            var parentId = (childId - 1) / 2;

            //Checking if heap property is preserved
            if (_queue[childId].CompareTo(_queue[parentId]) >= 0)
                break;

            //If not preserved, replace child and parent
            (_queue[childId], _queue[parentId]) = (_queue[parentId], _queue[childId]);
            childId = parentId;
        }
    }

    public T Dequeue()
    {
        var lastId = _queue.Count - 1;
        
        //Store first element
        var first = _queue[0];

        //Replacing first element with last element
        _queue[0] = _queue[lastId];
        _queue.RemoveAt(lastId--);
        
        var parentId = 0;

        //Fixing the heap after deletion
        while (true)
        {
            //Left and right child in binary heap
            var left = parentId * 2 + 1;
            var right = left + 1;

            //Stop if parent has no children
            if (left > lastId)
                break;

            var childToCompare = left;
            if (right <= lastId) //If right child exist
                if (_queue[right].CompareTo(_queue[left]) < 0) //Get smaller child candidate to compare with parent 
                    childToCompare = right;

            //Compare parent with smaller child, if parent is not greater than smaller child, heap property is preserved
            if (_queue[parentId].CompareTo(_queue[childToCompare]) <= 0)
                break;

            //Replace and fix heap down
            (_queue[parentId], _queue[childToCompare]) = (_queue[childToCompare], _queue[parentId]);
            parentId = childToCompare;
        }

        return first;
    }
}