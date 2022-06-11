


//����� �� & �� Mutex

#include <iostream>
#include <thread>
#include <mutex>
#include <string>
#include <chrono>

using namespace std;


class Actor
{
public:
	
	mutex heart;
	thread th;

	Actor() {}
	virtual void Run(){}
};


class Hoon : public Actor
{
public:
	Hoon()
	{
		RunPointer = &Hoon::Run;
		th = thread(RunPointer, this);


		th.join();
		
	}

	void(Hoon::* RunPointer)();


	void Run() {
		
		cout << "�׽�Ʈ";
	}
};

class Seok : public Actor
{
public:
	Seok() {
		
	}
	
	void Run() {}
};

class Soo : public Actor
{
public:
	 void Run() {}
};

int main() //Thread0 PD : ������ (main)
{
	Hoon* h = new Hoon();

	 //Thread2
	//thread TSeok(GetLove, "������.."); //Thread1 
	//thread TSoo(GetLove, "������.."); //Thread1 

	//na_seok_hoon.join();
	//oh_jeong_seok.join();
	this_thread::sleep_for(std::chrono::milliseconds(1000));
	delete(h);
	
	return 0;
}



string taesu = "�־�..";
mutex taesu_maum;

void Propose(string my_name, string his_name)
{
	cout << "\n\n";
	cout << my_name << ": " << his_name << ".. ����� �������� ����մϴ�.." << endl;
	cout << my_name << ": " << his_name << "..�� ������.." << endl;
	this_thread::sleep_for(std::chrono::milliseconds(1000));
}

void GetLove(string name)
{
	taesu_maum.lock();
	taesu = name;
	Propose("��", taesu);
	this_thread::sleep_for(std::chrono::milliseconds(5000));//5��
	cout << "�� :" << name << " �츮 �����";
	taesu_maum.unlock();
}

