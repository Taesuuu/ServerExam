//Thread

#include <iostream>
#include <thread>
#include <chrono>

using namespace std;

int trigger;

void seokHoon()
{
	while(trigger == 0)
	{
		cout << "������ : ������ �����\n";
		this_thread::sleep_for(std::chrono::milliseconds(1000));
	}	
}

void JeongSeok()
{
	while (trigger == 0)
	{
		this_thread::sleep_for(std::chrono::milliseconds(500));
		cout << "������ : ���ƾ� �����\n";
		this_thread::sleep_for(std::chrono::milliseconds(500));
	}
}

int main()
{
	trigger = 0;
	cout << "\n--�׵��� ����� ���۵ȴ�...\n\n";
	thread na(seokHoon);
	thread oh(JeongSeok);
	
	while (trigger==0)
	{
		cin >> trigger;
	}

	na.join();
	oh.join();
	cout << "\n--�׵��� ����� ������...\n\n";
}