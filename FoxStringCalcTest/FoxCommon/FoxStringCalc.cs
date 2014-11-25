// FoxStringCalc.cs
// 
// ver 1.00 07/12/2014
// ver 1.01 07/13/2014 Chage Calculate() argument.
//                     Add ChangeParameter().
// ver 1.02 07/14/2014 Add Define function.
//                     Change ReplaceParameter(). check define function Name.
// ver 2.00 09/13/2014 Replace All!
//
// Copyright (c) 2014 Tsutomu Mamiya
// 
// This software is released under the MIT License.
// 
// http://opensource.org/licenses/mit-license.php
//
//#define RECALCULATION												// 検算する場合に有効にする

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FoxCommon
{
	public class FoxStringCalc
	{
		#region フィールド

		/// <summary>
		/// シンボルの定義リスト
		/// </summary>
		List<SymbolDefine> SymbolDefineList = new List<SymbolDefine>();

		/// <summary>
		/// 固定数:円周率 π
		/// </summary>
		private String ReplacePI = ((Decimal)Math.PI).ToString();		// 3.14159265358979323846

		/// <summary>
		/// 固定数:自然対数の底 ε
		/// </summary>
		private String ReplaceE = ((Decimal)Math.E).ToString();			// 2.7182818284590452354

		/// <summary>
		/// 演算の変数リスト
		/// </summary>
		private Dictionary<String, Decimal> ArgumentList = new Dictionary<String, Decimal>();

		/// <summary>
		/// 定義済み関数
		/// </summary>
		private List<String> DefineFunctionList = new List<string> { "sin", "cos", "tan", "abs", "log10", "loge", "pow", "exp", "sqrt" };

		#endregion

		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FoxStringCalc()
		{
			//*** 定義済み関数をシンボルとして登録する ***

			// sin
			SymbolDefineList.Add(new SymbolDefine("sin", delegate(Decimal valueX, Decimal valueY)
			{
				return (Decimal)Math.Sin((Double)valueX);
			}));
			// cos
			SymbolDefineList.Add(new SymbolDefine("cos", delegate(Decimal valueX, Decimal valueY)
			{
				return (Decimal)Math.Cos((Double)valueX);
			}));
			// tan
			SymbolDefineList.Add(new SymbolDefine("tan", delegate(Decimal valueX, Decimal valueY)
			{
				return (Decimal)Math.Tan((Double)valueX);
			}));
			// abs
			SymbolDefineList.Add(new SymbolDefine("abs", delegate(Decimal valueX, Decimal valueY)
			{
				return (Decimal)Math.Abs((Double)valueX);
			}));
			// log
			SymbolDefineList.Add(new SymbolDefine("log10", delegate(Decimal valueX, Decimal valueY)
			{
				return (Decimal)Math.Log10((Double)valueX);
			}));
			// ln
			SymbolDefineList.Add(new SymbolDefine("loge", delegate(Decimal valueX, Decimal valueY)
			{
				return (Decimal)Math.Log((Double)valueX);
			}));
			// xy
			SymbolDefineList.Add(new SymbolDefine("pow", delegate(Decimal valueX, Decimal valueY)
				{
					return (Decimal)Math.Pow((Double)valueX, (Double)valueY);
				}));
			// ex
			SymbolDefineList.Add(new SymbolDefine("exp", delegate(Decimal valueX, Decimal valueY)
			{
				return (Decimal)Math.Exp((Double)valueX);
			}));
			// √
			SymbolDefineList.Add(new SymbolDefine("sqrt", delegate(Decimal valueX, Decimal valueY)
			{
				if (valueX < 0)
				{// || valueY < 0){
					return 0;
				}

				return (Decimal)Math.Sqrt((Double)valueX);
			}));

			//*** 関数を追加する場合には、これ以降に追加する ***

			#region 検算
			// 検算:四則演算
#if RECALCULATION
			Decimal val = 0;
			Decimal checkValue = 0;

			if (Calculation("1+2") != (1 + 2))
				Console.WriteLine("False. 1+2");
			if (Calculation("1+2+4") != (1 + 2 + 4))
				Console.WriteLine("False. 1+2+4");
			if (Calculation("5-2-1") != (5 - 2 - 1))
				Console.WriteLine("False. 5-2-1");
			if (Calculation("5-2+1") != (5 - 2 + 1))
				Console.WriteLine("False. 5-2+1");
			if (Calculation("2*3") != (2 * 3))
				Console.WriteLine("False. 2*3");
			if (Calculation("2*3*5") != (2 * 3 * 5))
				Console.WriteLine("False. 2*3*5");
			if (Calculation("2*3/5") != (2M * 3M / 5M))
				Console.WriteLine("False. 2*3/5");

			if (Calculation("5-(1+2)") != (5 - (1 + 2)))
				Console.WriteLine("False. 5-(1+2)");
			if (Calculation("(5-3)+2") != ((5 - 3) + 2))
				Console.WriteLine("False. (5-3)+2");
			if (Calculation("2*(5-3)") != (2 * (5 - 3)))
				Console.WriteLine("False. 2*(5-3)");
			if (Calculation("(5-3)-2") != ((5 - 3) - 2))
				Console.WriteLine("False. (5-3)-2");
			if (Calculation("(5-3)*2") != ((5 - 3) * 2))
				Console.WriteLine("False. (5-3)*2");
			if (Calculation("1+2*3-4") != ((1 + 2 * 3 - 4)))
				Console.WriteLine("False. 1+2*3-4");

			if (Calculation("(5-3)*(3+4)") != ((5 - 3) * (3 + 4)))
				Console.WriteLine("False. (5-3)*(3+4)");
			if (Calculation("(5-3)/(3+4)") != ((5M - 3M) / (3M + 4M)))
				Console.WriteLine("False. (5-3)/(3+4)");
			if (Calculation("((3+4)/5-2)/3") != (((3M + 4M) / 5M - 2M) / 3M))
				Console.WriteLine("False. ((3+4)/5-2)/3");


			val = Calculation("5+-3");
			checkValue = (Decimal)(5 + -3);
			if (val != checkValue)
				Console.WriteLine("False. 5+-3");

			val = Calculation("2*-3");
			checkValue = (Decimal)(2 * -3);
			if (val != checkValue)
				Console.WriteLine("False. 2*-3");

			val = Calculation("-π");
			checkValue = (Decimal)(-Math.PI);
			if (val != checkValue)
				Console.WriteLine("False. -π");

			val = Calculation("-sin(π/2.3)");
			checkValue = (Decimal)(-Math.Sin(Math.PI / 2.3));
			if (val != checkValue)
				Console.WriteLine("False. -sin(π/2.3)");

			// 検算:関数
			if (Calculation("sin(0.5)") != (Decimal)(Math.Sin(0.5D)))
				Console.WriteLine("False. sin(0.5)");

			val = Calculation("2*sin((0.4-0.1)/5)");
			checkValue = 2M * (Decimal)(Math.Sin((0.4D - 0.1D) / 5D));
			if (val != checkValue)
				Console.WriteLine("False. 2*sin((0.4-0.1)/5)");

			val = Calculation("2*cos((0.4-0.1)/5)");
			checkValue = 2M * (Decimal)(Math.Cos((0.4D - 0.1D) / 5D));
			if (val != checkValue)
				Console.WriteLine("False. 2*cos((0.4-0.1)/5)");

			val = Calculation("2*tan((0.4-0.1)/5)");
			checkValue = 2M * (Decimal)(Math.Tan((0.4D - 0.1D) / 5D));
			if (val != checkValue)
				Console.WriteLine("False. 2*tan((0.4-0.1)/5)");

			val = Calculation("2*abs((0.4-1.8)/5)");
			checkValue = 2M * (Decimal)(Math.Abs((0.4D - 1.8D) / 5D));
			if (val != checkValue)
				Console.WriteLine("False. 2*abs((0.4-1.8)/5)");

			val = Calculation("2*loge(abs(0.4-1.8)/5)");
			checkValue = 2M * (Decimal)(Math.Log(Math.Abs(0.4D - 1.8D) / 5D));
			if (val != checkValue)
				Console.WriteLine("False. 2*log((0.4-1.8)/5)");

			val = Calculation("2*log10(abs(0.4-1.8)/5)");
			checkValue = 2M * (Decimal)(Math.Log10(Math.Abs(0.4D - 1.8D) / 5D));
			if (val != checkValue)
				Console.WriteLine("False. 2*log10((0.4-1.8)/5)");

			val = Calculation("2*pow(10, (0.4-1.8)/5)");
			checkValue = 2M * (Decimal)(Math.Pow(10D, (0.4D - 1.8D) / 5D));
			if (val != checkValue)
				Console.WriteLine("False. 2*pow(10, (0.4-1.8)/5)");

			val = Calculation("2*exp((0.4-1.8)/5)");
			checkValue = 2M * (Decimal)(Math.Exp((0.4D - 1.8D) / 5D));
			if (val != checkValue)
				Console.WriteLine("False. 2*exp((0.4-1.8)/5)");

			val = Calculation("2*sqrt(abs(0.4-1.8)/5)");
			checkValue = 2M * (Decimal)(Math.Sqrt(Math.Abs(0.4D - 1.8D) / 5D));
			if (val != checkValue)
				Console.WriteLine("False. 2*sqrt(abs(0.4-1.8)/5)");

			val = Calculation("π");
			checkValue = (Decimal)Math.PI;
			if (val != checkValue)
				Console.WriteLine("False. π");

			val = Calculation("sin(π)");
			checkValue = (Decimal)(Math.Sin(Double.Parse(((Decimal)Math.PI).ToString())));
			if (val != checkValue)
				Console.WriteLine("False. sin(π)");

			val = Calculation("sin(π*0.01)+1.2");
			checkValue = (Decimal)(Math.Sin(Math.PI * 0.01D)) + 1.2M;
			if (val != checkValue)
				Console.WriteLine("False. sin(π*0.01)+1.");

			val = Calculation("sin(ε*0.01)+1.2");
			checkValue = (Decimal)(Math.Sin(Math.E * 0.01D)) + 1.2M;
			if (val != checkValue)
				Console.WriteLine("False. sin(ε*0.01)+1.2");

			val = Calculation("powxy(2,8)");
			checkValue = (Decimal)(Math.Pow(2, 8));
			if (val != checkValue)
				Console.WriteLine("False. pow(2, 8)");

			val = Calculation("powxy(2+7,5+2*4)");
			checkValue = (Decimal)(Math.Pow(2 + 7, 5 + 2 * 4));
			if (val != checkValue)
				Console.WriteLine("False. pow(2+7, 5+2*4)");

			//Double a = 3;
			//Double b = 1;
			//Double c = 1;
			//Double theta = -2.397;
			//checkValue = (Decimal)(a / (b + c * Math.Cos(theta)) * Math.Cos(theta));
			//val = Calculation("3 ÷ (1 + 1 × cos(-2.397)) × cos(-2.397)");
			//checkValue = (Decimal)(a / (b + c * Math.Cos(theta)) * Math.Sin(theta));
			//val = Calculation("3 ÷ (1 + 1 × cos(-2.397)) × sin(-2.397)");
			//if (val != checkValue)
			//    Console.WriteLine("False.");

			//Double a = 1;
			//Double theta = 0.5;
			//checkValue = (Decimal)(Math.Sqrt(2 * Math.Pow(a, 2) * Math.Cos(2 * theta)));
			//val = Calculation("sqrt(2 × pow(1, 2) × cos(2 × 0.5))");
			//if (val != checkValue)
			//    Console.WriteLine("False.");

			Console.WriteLine("Recalculation Complete.");
#endif
			#endregion
		}
		#endregion

		#region 演算の優先順位
		/// <summary>
		/// 演算の優先順位
		/// </summary>
		/// <returns>0〜20。0=最優先, 20=標準</returns>
		/// <param name="ch">キャラクタ</param>
		private OperatorLevel GetOperatorLevel(Char ch)
		{
			switch (ch) {
			case ',':
				return OperatorLevel.Comma;
			case '+':
			case '-':
				return OperatorLevel.AddSub;
			case '*':
			case '/':
			case '×':
			case '÷':
				return OperatorLevel.MultiDiv;
			case '^':							// べき乗
				return OperatorLevel.PowerMinus;
			}

			return OperatorLevel.Default;
		}
		#endregion

		#region 固定値の置き換え
		/// <summary>
		/// 固定値の置き換え
		/// </summary>
		/// <param name="formula"></param>
		/// <returns></returns>
		private String ReplaceStaticValue(String formula)
		{
			String result = formula.Replace("π", ReplacePI);
			result = result.Replace("ε", ReplaceE);
			result = result.Replace("√", "sqrt");
			result = result.Replace(" ", "");

			return result;
		}
		#endregion

		#region パラメータの追加
		/// <summary>
		/// パラメータの追加
		/// </summary>
		/// <param name="name">パラメータ名</param>
		/// <param name="value">値</param>
		public void AddParameter(String name, Decimal value)
		{
			ArgumentList.Add(name, value);
		}
		#endregion

		#region パラメータ値の変更
		/// <summary>
		/// パラメータ値の変更
		/// </summary>
		/// <param name="name">パラメータ名</param>
		/// <param name="value">値</param>
		public void ChangeParameter(String name, Decimal value)
		{
			ArgumentList[name] = value;
		}
		#endregion

		#region パラメータのクリア
		/// <summary>
		/// パラメータのクリア
		/// </summary>
		public void ClearParameter()
		{
			ArgumentList.Clear();
		}
		#endregion

		#region 式文字列に含まれるパラメータ(変数)を実値で置換
		/// <summary>
		/// 式文字列に含まれるパラメータ(変数)を実値で置換
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		private String ReplaceParameter(String expression)
		{
			// 関数名とパラメータが重ならないように、関数名を一時的に別名にする
			Char functionMark = '_';
			for (Int32 funcIndex = 0; funcIndex < DefineFunctionList.Count; ++funcIndex)
			{
				String replaceName = functionMark + funcIndex.ToString();
				expression = expression.Replace(DefineFunctionList[funcIndex], replaceName);
			}

			// パラメータを実値に置換する
			foreach (KeyValuePair<String, Decimal> arg in ArgumentList)
			{
				Int32 index = expression.IndexOf(arg.Key);
				if (index == -1)
					continue;
				expression = expression.Replace(arg.Key, arg.Value.ToString());
			}

			// 一時的に別名とした関数名を元に戻す
			for (Int32 funcIndex = 0; funcIndex < DefineFunctionList.Count; ++funcIndex)
			{
				String replaceName = functionMark + funcIndex.ToString();
				expression = expression.Replace(replaceName, DefineFunctionList[funcIndex]);
			}

			return expression;
		}
		#endregion

		#region 演算
		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="formula"></param>
		/// <returns></returns>
		public Decimal Calculation(String formula)
		{
			Int32 index = 0;
			Decimal resultRightValue = 0;

			// 無駄なスペースを削除
			formula = formula.Replace(" ", "");

			// パラメータ(変数)を実際の値に置換する
			formula = ReplaceParameter(formula);

			// ここからの演算は再起で行う
			return Calculation (ReplaceStaticValue(formula), ref index, ref resultRightValue, OperatorLevel.Default);
		}

		/// <summary>
		/// 演算(再起)
		/// </summary>
		/// <param name="formula"></param>
		/// <param name="index"></param>
		/// <param name="operatorLevel"></param>
		/// <returns></returns>
		public Decimal Calculation(String formula, ref Int32 index, ref Decimal resultRightValue, OperatorLevel operatorLevel = OperatorLevel.Default)
		{
			Decimal resultValue = 0;
			StringBuilder tokenBuilder = new StringBuilder ();

			for (; index < formula.Length; ++index) {
				Char ch = formula [index];

				// 括弧内を優先して計算する
				switch (ch) {
				case '(':							// ネストレベルUP
					// 括弧の中を先に計算する
					Int32 subindex = 0;
					resultValue = Calculation (formula.Substring (index + 1), ref subindex, ref resultRightValue, OperatorLevel.Default);

					// 移動量と'('と')'の2文字分を加算する
					index += subindex + 2;

					// 呼び出し元が関数もしくはべき乗の優先順位の場合、カッコ内での演算を終わる
					if (operatorLevel <= OperatorLevel.Function)
						return resultValue;

					// indexが最後まで進んでいたら演算を終わる
					if (index >= formula.Length)
						return resultValue;

					// 演算結果をTokenとして設定して演算を続ける
					--index;
					tokenBuilder.Clear ();
					tokenBuilder.Append(resultValue.ToString ());
					continue;
				
				case ')':							// ネストレベルDOWN
					return Decimal.Parse (tokenBuilder.ToString());
				}

				// 四則演算
				if (ch == '+' || ch == '-' || ch == '*' || ch == '/'|| ch == '^'|| ch == ',' ||
					ch == '×' || ch == '÷') {
					// 左辺の値
					String token = tokenBuilder.ToString ();

					// 演算の優先順位を取得する
					OperatorLevel currentOperatorLevel = GetOperatorLevel (ch);

					// '−'符号への対応
					if (token == "" && ch == '-') {
						token = "0";
						currentOperatorLevel = OperatorLevel.PowerMinus;
					}

					// 呼び出し元の優先順位が低い場合は、先に右辺を計算する
					if (operatorLevel > currentOperatorLevel) {
						// 右辺の計算結果を取得する
						++index;
						Decimal rightValue = Calculation (formula, ref index, ref resultRightValue, currentOperatorLevel);
						switch (ch) {
						case '+':
							resultValue = Decimal.Parse (token) + rightValue;
							break;
						case '-':
							resultValue = Decimal.Parse (token) - rightValue;
							break;
						case '*':
						case '×':
							resultValue = Decimal.Parse (token) * rightValue;
							break;
						case '/':
						case '÷':
							if (rightValue != 0)
								resultValue = Decimal.Parse (token) / rightValue;
							else
								resultValue = Decimal.MaxValue;
							break;
						case ',':
							resultRightValue = rightValue;
							resultValue = Decimal.Parse (token);
							break;
						}

						// indexが最後まで進んでいたら演算を終わる
						if (index >= formula.Length)
							return resultValue;

						// 演算結果をTokenとして設定して演算を続ける
						--index;
						tokenBuilder.Clear ();
						tokenBuilder.Append(resultValue.ToString ());
						continue;
					}
					// 呼び出し元よりも優先順位が低い場合は、取得した値を返す
					else {
						return Decimal.Parse (token);
					}
				}

				tokenBuilder.Append(ch);
				String tokenName = tokenBuilder.ToString();

				// 数値の場合は、次の文字を確認する
				if (Regex.IsMatch(tokenName, @"^[+-]?[0-9]*[\.]?[0-9]+$") == true)
					continue;

				// 関数の呼び出し
				foreach (SymbolDefine symbol in SymbolDefineList) {
					if (tokenName == symbol.Name) {
						Int32 subindex = 0;
						resultRightValue = 0;
						resultValue = Calculation(formula.Substring(index + 1), ref subindex, ref resultRightValue, OperatorLevel.Function);

						// 移動量を加算する
						index += subindex;

						// 関数の演算
						resultValue = symbol.CallCalculation(resultValue, resultRightValue);
						tokenBuilder.Clear ();
						tokenBuilder.Append(resultValue.ToString ());
						break;
					}
				}
			}

			return Decimal.Parse(tokenBuilder.ToString ());
		}
		#endregion

	}

	#region シンボル定義
	/// <summary>
	/// シンボル定義
	/// </summary>
	public class SymbolDefine //: UIView
	{
		///
		/// 演算のデリゲート
		///
		public delegate Decimal DelegateCalculation(Decimal valueX, Decimal valueY);
		public event DelegateCalculation OnCalculation;

		/// <summary>
		/// シンボル名
		/// </summary>
		public String Name { get; private set; }

		/// <summary>
		/// 添え字のリスト
		/// </summary>
		public List<IndexScript> IndexScriptList { get; private set; }

		/// <summary>
		/// フォント情報
		/// </summary>
		public SymbolIndexScriptFont FontInformation { get; set; }

		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="シンボル名">
		public SymbolDefine(String name, DelegateCalculation onCalculation)
		{
			// シンボル名
			Name = name;

			// 添字リスト/フォント情報を初期化する
			IndexScriptList = new List<IndexScript>();
			FontInformation = new SymbolIndexScriptFont();

			OnCalculation = onCalculation;
		}
		#endregion

		#region 添え字の追加
		/// <summary>
		/// 添え字の追加
		/// </summary>
		/// <param name="newIndexScript">追加する添え字</param>
		public void AddIndexScript(IndexScript newIndexScript)
		{
			foreach (IndexScript index in IndexScriptList)
			{
				// 名前が同一の場合はNG
				if (index.Name == newIndexScript.Name)
					throw new FoxException(ExceptionType.CalcDuplicateIndex);

				// 配置の位置が同じ場合はNG
				if (index.IndexPositionType == newIndexScript.IndexPositionType)
					throw new FoxException(ExceptionType.CalcDuplicateIndexPosition);
			}

			IndexScriptList.Add(newIndexScript);
		}
		#endregion

		#region 添え字の削除
		/// <summary>
		/// 添え字の削除
		/// </summary>
		/// <param name="indexScriptName">添え字の名前</param>
		public void DelteScriptIndex(String indexScriptName)
		{
			foreach (IndexScript index in IndexScriptList)
			{
				if (index.Name == indexScriptName)
				{
					IndexScriptList.Remove(index);
					return;
				}
			}

			// 該当する添え字がなかった場合
			throw new FoxException(ExceptionType.CalcUnknownIndex);
		}
		#endregion

		#region 添え字の変更
		/// <summary>
		/// 添え字の変更
		/// </summary>
		/// <param name="changeIndexScript">変更する添え字の情報</param>
		public void ChangeIndexScript(IndexScript changeIndexScript)
		{
			// 対象となる添え字の名前を取得する
			String indexName = "";
			foreach (IndexScript index in IndexScriptList)
			{
				if (index.Name == changeIndexScript.Name)
				{
					indexName = index.Name;
					break;
				}
			}

			// 該当する添え字がなかった場合
			if (indexName == "")
				throw new FoxException(ExceptionType.CalcUnknownIndex);

			// 該当する添え字を削除して追加する
			DelteScriptIndex(indexName);
			AddIndexScript(changeIndexScript);
		}
		#endregion

		#region 関数シンボルの演算用コールバック
		/// <summary>
		/// 関数シンボルの演算用コールバック
		/// </summary>
		/// <param name="valueX"></param>
		/// <param name="valueY"></param>
		/// <returns></returns>
		public Decimal CallCalculation(Decimal valueX, Decimal valueY)
		{
			// シンボルに対する演算を呼び出す
			return this.OnCalculation(valueX, valueY);
		}
		#endregion
	}
	#endregion

	#region 添え字
	/// <summary>
	/// 添え字
	/// </summary>
	public class IndexScript
	{
		/// <summary>
		/// 名前
		/// </summary>
		public String Name { get; private set; }

		/// <summary>
		/// 値
		/// </summary>
		public String Value { get; private set; }

		/// <summary>
		/// 添え字の位置
		/// </summary>
		public IndexScriptPosition IndexPositionType { get; private set; }

		/// <summary>
		/// フォント情報
		/// </summary>
		public SymbolIndexScriptFont FontInformation { get; set; }

		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">名前</param>
		/// <param name="value">値</param>
		/// <param name="indexPosition">添え字の位置</param>
		public IndexScript(String name = "", String value ="", IndexScriptPosition indexPosition = IndexScriptPosition.RightScript)
		{
			Name = name;
			Value = value;
			IndexPositionType = indexPosition;
			FontInformation = new SymbolIndexScriptFont();
		}
		#endregion
	}
	#endregion

	#region 添え字の位置
	/// <summary>
	/// 添え字の位置
	/// </summary>
	public enum IndexScriptPosition
	{
		/// <summary>
		/// 左:上付き文字
		/// </summary>
		LeftSuperScript,

		/// <summary>
		/// 左:中
		/// </summary>
		LeftScript,

		/// <summary>
		/// 左:下付き文字
		/// </summary>
		LeftSubScript,

		/// <summary>
		/// 右:上付き文字
		/// </summary>
		RightSuperScript,

		/// <summary>
		/// 右:中
		/// </summary>
		RightScript,

		/// <summary>
		/// 右:下付き文字
		/// </summary>
		RightSubScript,

		/// <summary>
		/// 中央:上付き文字
		/// </summary>
		CenterSuperScript,

		/// <summary>
		/// 中央:中
		/// </summary>
		CenterScript,

		/// <summary>
		/// 中央:下付き文字
		/// </summary>
		CenterSubScript,
	}
	#endregion

	#region フォント情報
	/// <summary>
	/// フォント情報
	/// </summary>
	public class SymbolIndexScriptFont
	{
		/// <summary>
		/// 名前
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// サイズ
		/// </summary>
		public float Size { get; set; }

		/// <summary>
		/// スタイル
		/// </summary>
		public SymbolIndexScriptFontStyle Style { get; set; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SymbolIndexScriptFont()
		{
			Name = "";
			Size = 10.0f;
			Style = SymbolIndexScriptFontStyle.Normal;
		}
	}
	#endregion

	#region フォントスタイル
	/// <summary>
	/// フォントスタイル
	/// </summary>
	public enum SymbolIndexScriptFontStyle
	{
		/// <summary>
		/// 標準
		/// </summary>
		Normal,

		/// <summary>
		/// 斜体
		/// </summary>
		Italic,

		/// <summary>
		/// 太字
		/// </summary>
		Bold,

		/// <summary>
		/// 太字/斜体
		/// </summary>
		BoldItalic
	}
	#endregion

	#region 演算の優先順位
	/// <summary>
	/// 演算の優先順位
	/// </summary>
	public enum OperatorLevel
	{
		/// <summary>
		/// べき乗/マイナス
		/// </summary>
		PowerMinus = 6,

		/// <summary>
		/// 関数
		/// </summary>
		Function = 7,

		/// <summary>
		/// 掛け算/割り算 (×/÷)
		/// </summary>
		MultiDiv = 8,

		/// <summary>
		/// 足し算/引き算 (+/-)
		/// </summary>
		AddSub = 9,

		/// <summary>
		/// カンマ
		/// </summary>
		Comma = 10,

		/// <summary>
		/// 標準
		/// </summary>
		Default = 20,
	}
	#endregion
}
