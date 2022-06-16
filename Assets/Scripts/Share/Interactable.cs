using System;
using UnityEngine;

public interface Interactable
{
	void Interact();

	void LocalExecute();

	void AllExecute();

	void RemoveObject();

	string GetName();

	bool IsStarted();
}