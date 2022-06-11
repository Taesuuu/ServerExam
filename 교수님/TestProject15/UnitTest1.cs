using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FinalTestSample;

//이 프로젝트는 빌드후 디버깅을 하는게 아니라 테스트탭의 테스트를 실행해야함
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