public interface Interact
{
	void Interact();

	void RemoveObject();

	void DropObject();

	string GetName();

	ItemStats GetItem();

	bool IsStarted();
}