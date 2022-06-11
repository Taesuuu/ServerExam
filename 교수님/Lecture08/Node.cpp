#include <iostream>
class Node
{
public:
	Node(int input)
	{
		data = input;
		next = nullptr;
	}
	int data;
	Node* next;
};

int main()
{	
	Node* start = new Node(1);
	Node* current = start;
	current->next = new Node(2);
	current = current->next;
	current->next = new Node(3);
	current = current->next;	

	current = start;

	while (current != nullptr)
	{
		std::cout << current->data << std::endl;
		current = current->next;
	}
}