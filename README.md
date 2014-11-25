StringCalc　(C# Expression Evaluator)
======================================
文字列の数式を計算するクラス

xamarin iOS用に思ったように使えるコードを見つけられなかったので自作。
※ Visual Studio 2010のC#でも大丈夫。

(1) 一番簡単な使い方
    Example 1

      FoxStringCalc foxCalc = new FoxStringCalc();
      Decimal result = foxCalc.Calculate(TextFormula.Text);
			LabelResult.Text = result.ToString();
				
(2) 関数を加える
    Example 2

			FoxStringCalc foxCalc = new FoxStringCalc();
			Decimal result;

			// 未定義関数のコールバック
			foxCalc.OnCallbackFunction += delegate(String functionName, Decimal value)
			{
				if (functionName == "asin")
					return (Decimal)Math.Asin((Double)value);
				return 0;
			};

      :
      :
      
      result = foxCalc.Calculate("2 * asin(0.12345)");

(3) パラメータを指定して実行
    Example 3

			// ハイポサイクロイドを計算させてみる
			Decimal A = 3;
			Decimal B = 2;
			Decimal theta = (Decimal)Math.PI / 2 + 0.12345M;

			// パラメータの設定をして演算
			foxCalc.AddParameter("A", A);
			foxCalc.AddParameter("B", B);
			foxCalc.AddParameter("theta", theta);
			Decimal dx = foxCalc.Calculate("(A - B) * cos(theta) + B * cos(((A - B) / B) * theta))");
			Decimal dy = foxCalc.Calculate("(A - B) * sin(theta) - B * sin(((A - B) / B) * theta))");


This software is released under the MIT License, see LICENSE.txt.
