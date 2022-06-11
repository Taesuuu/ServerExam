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
		cout << "³ª¼®ÈÆ : Á¤¼®¾Æ »ç¶ûÇØ\n";
		this_thread::sleep_for(std::chrono::milliseconds(1000));
	}	
}

void JeongSeok()
{
	while (trigger == 0)
	{
		this_thread::sleep_for(std::chrono::milliseconds(500));
		cout << "¿ÀÁ¤¼® : ¼®ÈÆ¾Æ »ç¶ûÇØ\n";
		this_thread::sleep_for(std::chrono::milliseconds(500));
	}
}

int main()
{
	trigger = 0;
	cout << "\n--±×µéÀÇ »ç¶ûÀÌ ½ÃÀÛµÈ´Ù...\n\n";
	thread na(seokHoon);
	thread oh(JeongSeok);
	
	while (trigger==0)
	{
		cin >> trigger;
	}

	na.join();
	oh.join();
	cout << "\n--±×µéÀÇ »ç¶ûÀÌ ³¡³µ´Ù...\n\n";
}