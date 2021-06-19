using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake_sandbox01
{
    public class InterfaceTest
    {
        public void Start()
        {
            MyClass myClass = new MyClass();
            TestInterface(myClass);
            MySecondClass secClass = new MySecondClass();
            TestInterface(secClass);
        }

        private void TestInterface(IMyInterface myInterface)
        {
            myInterface.TestFunction();
            
        }
    }



    public interface IMyInterface
    {
        void TestFunction();

    }

    public interface IMySecondInterface
    {
        void TestMethod();
    }



    public class MyClass : IMyInterface, IMySecondInterface
    {
        public void TestMethod()
        {

        }

        public void TestFunction()
        {
            MessageBox.Show("MyClass.TestFunction()");
        }
    }

    public class MySecondClass : IMyInterface
    {
        public void TestFunction()
        {
            MessageBox.Show("MySecondlass.TestFunction()");
        }
    }



}