//����� �� & �� Thread & Mutex

#include <iostream>
#include <thread>
#include <mutex>
#include <string>
#include <chrono>

using namespace std;


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

void main() //Thread0 PD : ������ (main)
{
	thread na_seok_hoon(GetLove, "���ƾ�.."); //Thread2
	thread oh_jeong_seok(GetLove, "������.."); //Thread1 

	na_seok_hoon.join();
	oh_jeong_seok.join();
}