using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FinalTestSample;

//�� ������Ʈ�� ������ ������� �ϴ°� �ƴ϶� �׽�Ʈ���� �׽�Ʈ�� �����ؾ���
namespace TestProject15
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            if(FinalTestSample.Program.Main() == 0)
            {
                
                Assert.IsTrue(false);
                
            }
        }
    }
}