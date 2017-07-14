using UnityEngine;
using System.Collections.Generic;
using System;

public class EventNotificationCenter : PSingleton<EventNotificationCenter>
{
	public delegate void Callback();
	public delegate void Callback<T>(T arg1);
	public delegate void Callback<T, U>(T arg1, U arg2);
	public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);

	protected Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
	// Message handlers that should never be removed, regardless of calling Cleanup
	protected List< string > permanentMessages = new List< string > ();
	
	
    protected override void Init()
    {
        eventTable.Clear();
        permanentMessages.Clear();
    }

		//Marks a certain message as permanent.
	public void MarkAsPermanent(string eventType) 
	{
        permanentMessages.Add(eventType);
	}

	public void Cleanup()
	{
		List< string > messagesToRemove = new List<string>();
		foreach (KeyValuePair<string, Delegate> pair in eventTable) 
		{
			bool wasFound = false;
 
			foreach (string message in permanentMessages) 
			{
				if (pair.Key == message) 
				{
					wasFound = true;
					break;
				}
			}
 
			if (!wasFound)
				messagesToRemove.Add( pair.Key );
		}
 
		foreach (string message in messagesToRemove) 
		{
			eventTable.Remove( message );
		}
	}


	public void PrintEventTable()
	{
		Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");
 
		foreach (KeyValuePair<string, Delegate> pair in eventTable) 
		{
			Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
		}
 
		Debug.Log("\n");
	}

	// -- 1. 事件防御处理
	private void OnListenerAdding(string eventType, Delegate listenerBeingAdded) 
	{	 
        if (!eventTable.ContainsKey(eventType)) 
        {
            eventTable.Add(eventType, null );
        }
 
        Delegate d = eventTable[eventType];
        if (d != null && d.GetType() != listenerBeingAdded.GetType()) 
        {
            throw new ListenerException(string.Format(
				"Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}",
				 eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
        }
	}
	 
	private void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved) 
	{	 
        if (eventTable.ContainsKey(eventType)) 
        {
            Delegate d = eventTable[eventType];
            if (d == null)
            {
                throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
            } 
            else if (d.GetType() != listenerBeingRemoved.GetType()) 
            {
                throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
            }
        } 
        else 
        {
            //throw new ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
        }
	}
	 
	private void OnListenerRemoved(string eventType)
	{
        if (eventTable[eventType] == null) 
        {
            eventTable.Remove(eventType);
        }
	}
	 
	private void OnBroadcasting(string eventType) 
	{
		#if REQUIRE_LISTENER
        if (!eventTable.ContainsKey(eventType)) 
        {
            throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
        }
		#endif
	}
	 
	private BroadcastException CreateBroadcastSignatureException(string eventType) 
	{
        return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
	}
	 
    public class BroadcastException : Exception 
    {
        public BroadcastException(string msg): base(msg) 
        {

        }
    }
 
    public class ListenerException : Exception 
    {
        public ListenerException(string msg): base(msg) 
        {

        }
    }


    // -- 2. 添加事件
    //No parameters
	public void AddListener(string eventType, Callback handler) 
	{
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback)eventTable[eventType] + handler;
    }
 
	//Single parameter
	public void AddListener<T>(string eventType, Callback<T> handler) 
	{
        if (handler == null )
		{
            return;
        }
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
    }

	//Two parameters
	public void AddListener<T, U>(string eventType, Callback<T, U> handler) 
	{
        if (handler == null )
		{
            return;
        }
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback<T, U>)eventTable[eventType] + handler;
    }
 
	//Three parameters
	public void AddListener<T, U, V>(string eventType, Callback<T, U, V> handler) 
	{
        if (handler == null )
		{
            return;
        }
        OnListenerAdding(eventType, handler);
        eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] + handler;
    }

    // -- 3. 删除事件

	//No parameters
    public void RemoveListener(string eventType, Callback handler) 
    {
        if (handler == null)
		
		{
            return;
        }
        OnListenerRemoving(eventType, handler);
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = (Callback)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
    }
 
	//Single parameter
	public void RemoveListener<T>(string eventType, Callback<T> handler) 
	{
        if (handler == null)
		{
            return;
        }
        OnListenerRemoving(eventType, handler);
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
    }
 
	//Two parameters
	public void RemoveListener<T, U>(string eventType, Callback<T, U> handler)
	{
        if (handler == null)
		{
            return;
        }
        OnListenerRemoving(eventType, handler);
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = (Callback<T, U>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
    }
 
	//Three parameters
	public void RemoveListener<T, U, V>(string eventType, Callback<T, U, V> handler) 
	{
        if (handler == null)
		{
            return;
        }
        OnListenerRemoving(eventType, handler);
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
    }

    // -- 4. 广播事件
    //No parameters
     public void Broadcast(string eventType) 
    {
        if (eventType == null )
		{
            return;
        }
        OnBroadcasting(eventType);
 
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d)) 
        {
            Callback callback = d as Callback;
 
            if (callback != null) 
            {
                List<Callback> removedDel = new List<Callback>();
                foreach(Delegate del in callback.GetInvocationList())
				{
                    bool isCalled = false;
                    if (del.Target is MonoBehaviour)
					{
                        MonoBehaviour mono = (MonoBehaviour)del.Target;
                        if (mono != null)
						{
                            Callback cb = (Callback)del;
                            isCalled = true;
                            cb();
                        }
                    }
					else if (del.Target!=null)
					{
                        Callback cb = (Callback)del;
                        isCalled = true;
                        cb();
                    }
                    if (!isCalled)
					{
                        removedDel.Add((Callback)del);
                    }
                }
                if (eventTable.ContainsKey(eventType))
				{
                    foreach (Callback rDel in removedDel) 
                    {
                        callback = callback - rDel;
                    }
                    eventTable[eventType] = callback;
                }
            } 
            else 
            {
               
				//throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
 
	//Single parameter
    public void Broadcast<T>(string eventType, T arg1) 
    {
        if (eventType == null)
		{
            return;
        }
        OnBroadcasting(eventType);
 
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d)) 
		{
            Callback<T> callback = d as Callback<T>;
            if (callback != null) 
            {
                List<Callback<T>> removedDel = new List<Callback<T>>();
                foreach(Delegate del in callback.GetInvocationList() )
				{
                    bool isCalled = false;
                    if (del.Target is MonoBehaviour)
					{
                        MonoBehaviour mono = (MonoBehaviour)del.Target;
                        if (mono != null)
						{
                            Callback<T> cb = (Callback<T>)del;
                            cb(arg1);
                            isCalled = true;
                        }
                    }
					else if (del.Target!=null)
					{
                        Callback<T> cb = (Callback<T>)del;
                        cb(arg1);
                        isCalled = true;
                    }
                    if (!isCalled)
					{
                        removedDel.Add((Callback<T>)del);
                    }
                }
                if (eventTable.ContainsKey(eventType))
				{

                    foreach (Callback<T> rDel in removedDel) 
                    {
                        callback = callback - rDel;
                    }
                    eventTable[eventType] = callback;
                }
            } 
            else 
            {
				//throw CreateBroadcastSignatureException(eventType);
            }
        }
	}
 
	//Two parameters
    public void Broadcast<T, U>(string eventType, T arg1, U arg2) 
    {
        if (eventType == null )
		{
            return;
        }
        OnBroadcasting(eventType);
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d)) 
        {
            Callback<T, U> callback = d as Callback<T, U>;
            if (callback != null) 
            {   
                List<Callback<T, U>> removedDel = new List<Callback<T, U>>();
                foreach(Delegate del in callback.GetInvocationList() )
				{ 
                    bool isCalled = false;
                    if (del.Target is MonoBehaviour)
					{
                        MonoBehaviour mono = (MonoBehaviour)del.Target;
                    	if (mono != null)
						{
                            Callback<T, U> cb = (Callback<T, U>)del;
                            cb(arg1, arg2);
                            isCalled = true;
                        }
                    }
					else if (del.Target!=null)
					{
                        Callback<T, U> cb = (Callback<T, U>)del;
                        cb(arg1, arg2);
                        isCalled = true;
                    }
                    if (!isCalled)
					{
                        removedDel.Add((Callback<T, U>)del);
                    }
                }
                if (eventTable.ContainsKey(eventType))
				{

                    foreach (Callback<T, U> rDel in removedDel) 
                    {
                        callback = callback - rDel;
                    }
                    eventTable[eventType] = callback;
                }
            } 
            else 
            {
                // throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
 
	//Three parameters
    public void Broadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3) 
    {
        if (eventType == null )
		{
            return;
        }
        OnBroadcasting(eventType);
        Delegate d;
        if (eventTable.TryGetValue(eventType, out d)) 
        {
            Callback<T, U, V> callback = d as Callback<T, U, V>;
            if (callback != null) 
            {
                List<Callback<T, U, V>> removedDel = new List<Callback<T, U, V>>();
                foreach(Delegate del in callback.GetInvocationList() )
				{
                    bool isCalled = false;
                    if (del.Target is MonoBehaviour)
					{
                        MonoBehaviour mono = (MonoBehaviour)del.Target;
                        if (mono != null)
						{
                            Callback<T, U, V> cb = (Callback<T, U, V>)del;
                            cb(arg1, arg2, arg3);
                            isCalled = true;
                        }
                    }
					else if (del.Target!=null)
					{
                        Callback<T, U, V> cb = (Callback<T, U, V>)del;
                        cb(arg1, arg2, arg3);
                        isCalled = true;
                    }
                    if (!isCalled)
					{
                        removedDel.Add((Callback<T, U, V>)del);
                    }
                }
                if (eventTable.ContainsKey(eventType))
				{
                    foreach (Callback<T, U, V> rDel in removedDel) 
                    {
                        callback = callback - rDel;
                    }
                    eventTable[eventType] = callback;
                }
            } 
            else 
            {
				// throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
}
