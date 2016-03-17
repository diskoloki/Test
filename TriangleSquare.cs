
using System;
using System.Collections.Generic;
using NUnit.Framework;


namespace Console1
{
	class A
	{
		public double Calc(double a, double b, double c)
		{
			double epsilon = Math.Pow(10, -10);	//	Точность согласовывается с заказчиком

			if (a <= 0 || b <= 0 || c <= 0)
				throw new ArgumentException("TextOutOfRange");
			
			var list = new List<double>(){a,b,c};
			list.Sort();
			
			if (Math.Abs (list[0] * list[0] + list[1] * list[1] - list[2] * list[2]) > epsilon)
				throw new ArgumentException("TextNotRectangular");
			
			return list[0]*list[1]*0.5;

		}
	}
	
	class Tests
	{
		[Test]
		public void TestCalc()
		{
			A a = new A();
			try{
				var res1 = a.Calc(0, 1, 1);
				Assert.Fail("ArgRange");
			}
			catch (ArgumentException e){};

			try{
				var res2 = a.Calc(3, 4, 5.00000001);
				Assert.Fail("Rectangle");
			}
			catch (ArgumentException e){};

			Assert.AreEqual(a.Calc(5, 5, Math.Sqrt(50)), 12.5);
			
		}
	}
	
	
	class Program
	{
		
		public static void Main(string[] args)
		{
			A a = new A();

			Tests t = new Tests();
			t.TestCalc();

			a.Calc(3,4,5);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		

		
		
	}
}