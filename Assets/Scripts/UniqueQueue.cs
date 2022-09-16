using System;
using System.Collections.Generic;

public class UniqueQueue<T>
{
	public int Count
	{
		get
		{
			return this.queue.Count;
		}
	}
	public UniqueQueue()
	{
		this.queue = new Queue<T>();
		this.hashSet = new HashSet<T>();
	}

	public UniqueQueue(int capacity)
	{
		this.queue = new Queue<T>(capacity);
		this.hashSet = new HashSet<T>();
	}

	public void Enqueue(T item)
	{
		if (this.hashSet.Contains(item))
		{
			return;
		}
		this.queue.Enqueue(item);
		this.hashSet.Add(item);
	}

	public T Dequeue()
	{
		if (this.queue.Count == 0)
		{
			return default(T);
		}
		T t = this.queue.Dequeue();
		this.hashSet.Remove(t);
		return t;
	}

	public T Peek()
	{
		return this.queue.Peek();
	}

	public void Clear()
	{
		this.queue.Clear();
		this.hashSet.Clear();
	}

	private Queue<T> queue;

	private HashSet<T> hashSet;
}
