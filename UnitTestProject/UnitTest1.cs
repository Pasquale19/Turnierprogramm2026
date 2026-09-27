using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using Turnierprogramm2;
using Turnierprogramm2.Utilities;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1 : Auslosung
    {
        [TestMethod]
        public void TestMethod1()
        {
            int[,] arr = new int[,] { { 0,1},
            { 1,2}, { 2,3},{ 3,0}};

            int[,] sol = new int[,] { { 0,1},
            { 1,2}, { 2,0},{ 3,3}};

            int[,] res = extractFreilos(arr, arr.GetLength(0) - 1);

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                Trace.WriteLine($"{res[i, 0]} : {res[i, 1]}");
            }
            Assert.AreEqual(sol, res);
        }
    }

    public class UnitTest2 
    {
        [TestMethod]
        public void TestMethod1()
        {
            string folderPath= @"C:\Users\Pascal\Desktop\test\test.pdf";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
           // Assert.AreEqual(sol, res);
        }
    }
}
