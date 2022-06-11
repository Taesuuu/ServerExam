


//목요드라마 석 & 훈 Mutex

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
		
		cout << "테스트";
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

int main() //Thread0 PD : 이은석 (main)
{
	Hoon* h = new Hoon();

	 //Thread2
	//thread TSeok(GetLove, "정석아.."); //Thread1 
	//thread TSoo(GetLove, "정석아.."); //Thread1 

	//na_seok_hoon.join();
	//oh_jeong_seok.join();
	this_thread::sleep_for(std::chrono::milliseconds(1000));
	delete(h);
	
	return 0;
}



string taesu = "솔아..";
mutex taesu_maum;

void Propose(string my_name, string his_name)
{
	cout << "\n\n";
	cout << my_name << ": " << his_name << ".. 당신을 진심으로 사랑합니다.." << endl;
	cout << my_name << ": " << his_name << "..날 가져줘.." << endl;
	this_thread::sleep_for(std::chrono::milliseconds(1000));
}

void GetLove(string name)
{
	taesu_maum.lock();
	taesu = name;
	Propose("수", taesu);
	this_thread::sleep_for(std::chrono::milliseconds(5000));//5년
	cout << "수 :" << name << " 우리 헤어져";
	taesu_maum.unlock();
}

