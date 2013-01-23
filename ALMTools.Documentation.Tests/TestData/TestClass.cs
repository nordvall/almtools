using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALMTools.Documentation.Tests.TestData
{
    /// <summary>
    /// Class used for documentation testing
    /// </summary>
    public class TestClass
    {
        /// <summary>
        /// Stores a private string
        /// </summary>
        private string _private;

        /// <summary>
        /// Constructor with no arguments
        /// </summary>
        public TestClass()
        {

        }

        /// <summary>
        /// Constructor with string arument
        /// </summary>
        /// <param name="argument">The text to provide</param>
        public TestClass(string argument)
        {
            _private = argument;
        }

        /// <summary>
        /// Public method with no return value
        /// </summary>
        public void PublicVoidMethodWithoutArguments()
        { }

        /// <summary>
        /// Public method with two arguments
        /// </summary>
        /// <param name="first">Number of days</param>
        /// <param name="second">Name of city</param>
        public void PublicVoidMethodWithTwoArguments(int first, string second)
        { }

        /// <summary>
        /// Private method with no return value
        /// </summary>
        private void PrivateVoidMethodWithoutArguments()
        { }

        /// <summary>
        /// Private method with two arguments
        /// </summary>
        /// <param name="first">Age of person</param>
        /// <param name="second">Favorite soccer team</param>
        private void PrivateVoidMethodWithTwoArguments(int first, string second)
        { }

        /// <summary>
        /// Name of current president
        /// </summary>
        public string PublicStringProperty { get; set; }

        /// <summary>
        /// Population in Sandviken
        /// </summary>
        public int PublicIntProperty { get; set; }

        /// <summary>
        /// Public method with return value of string
        /// </summary>
        /// <returns>String containing day of week</returns>
        public string PublicStringMethodWithNoArguments()
        {
            return string.Empty;
        }

        /// <summary>
        /// Public method with return value and two arguments
        /// </summary>
        /// <param name="time">Time to retire</param>
        /// <param name="days">Days since the beginning</param>
        /// <returns>Word of wisdom</returns>
        public string PublicStringMethodWithTwoArguments(DateTime time, double days)
        {
            return string.Empty;
        }

        /// <summary>
        /// Public method with return value of string
        /// </summary>
        /// <returns>name of city</returns>
        private string PrivateStringMethodWithNoArguments()
        {
            return string.Empty;
        }

        /// <summary>
        /// Private method with return value and two arguments
        /// </summary>
        /// <param name="time">Time of day</param>
        /// <param name="days">Days left</param>
        /// <returns>Favorite color</returns>
        private string PrivateStringMethodWithTwoArguments(DateTime time, double days)
        {
            return string.Empty;
        }
    }
}
