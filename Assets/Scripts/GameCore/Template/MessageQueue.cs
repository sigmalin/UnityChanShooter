using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RequestQueue<T> where T : struct
{
	protected Queue<T> mMsgQueueOUT = null;

	protected Queue<T> mMsgQueueIN = null;

	public RequestQueue()
	{
		mMsgQueueOUT = new Queue<T> ();

		mMsgQueueIN = new Queue<T> ();
	}

	public void Enqueue(T _msg)
	{
		mMsgQueueIN.Enqueue (_msg);
	}

	public T Dequeue()
	{
		if (mMsgQueueOUT.Count == 0)
			return default(T);

		T msg = mMsgQueueOUT.Dequeue ();

		return msg;
	}

	public T[] ToArray()
	{
		T[] output = mMsgQueueOUT.ToArray ();

		while (mMsgQueueOUT.Count != 0) 
			Dequeue ();

		return output;
	}

	public RequestQueue<T> Package()
	{
		Queue<T> Swap = mMsgQueueOUT;

		mMsgQueueOUT = mMsgQueueIN;

		mMsgQueueIN = Swap;

		return this;
	}

	public void Clear()
	{
		while (mMsgQueueOUT.Count != 0) 
			Dequeue ();

		Package ();

		while (mMsgQueueOUT.Count != 0) 
			Dequeue ();
	}

	public bool IsEmpty(bool _recursive = false)
	{
		int count = mMsgQueueOUT.Count;

		if (_recursive == true)
			count += mMsgQueueIN.Count;
		
		return count == 0;
	}
}
