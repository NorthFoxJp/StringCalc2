using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FoxCommon;

namespace FoxStringCalcTest
{
	class Program
	{
		static void Main(string[] args)
		{

			FoxStringCalc formula = new FoxStringCalc();
			Decimal value = formula.Calculation("sin(0.5)");

			Console.ReadKey();
		}
	}
}
