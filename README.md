StringCalc rev.2　(C# Expression Evaluator)
============================================
文字列の数式を計算するクラス

xamarin iOS用に思ったように使えるコードを見つけられなかったので自作。
※ Visual Studio 2010のC#でも大丈夫。

(1) 一番簡単な使い方
    Example 1

      FoxStringCalc formula = new FoxStringCalc();
      Decimal value = formula.Calculation("sin(0.5)");


(2) 関数を加える
    Example 2

      public class FoxStringCalc のコンストラクタ内、以下のコメント行以降に、
      追加シンボル(関数名)と、演算処理を追加する。

      //*** 関数を追加する場合には、これ以降に追加する ***

      // xy
      SymbolDefineList.Add(new SymbolDefine("pow", delegate(Decimal valueX, Decimal valueY)
      {
          return (Decimal)Math.Pow((Double)valueX, (Double)valueY);
      }));


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
    Decimal dx = foxCalc.Calculation("(A - B) * cos(theta) + B * cos(((A - B) / B) * theta))");
    Decimal dy = foxCalc.Calculation("(A - B) * sin(theta) - B * sin(((A - B) / B) * theta))");


This software is released under the MIT License, see LICENSE.txt.
