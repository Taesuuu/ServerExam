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
                
                Assert.IsTrue(FinalTestSample.Program.Main() == 0);
                
            }
        }
        [TestMethod]
        public void TestMethod2()
        {
            if (FinalTestSample.Program.mainString() != null)
            {
                Assert.IsNotNull(FinalTestSample.Program.mainString());
                
            }
        }
    }
}