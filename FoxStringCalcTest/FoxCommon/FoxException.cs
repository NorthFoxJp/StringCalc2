using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FoxCommon
{
	public class FoxException : Exception
	{
		static public Int32 LanguageNo = 0;

		private Dictionary<ExceptionType, String> MessageDictionary = new Dictionary<ExceptionType, String>
		{
			{ExceptionType.Non, "例外は発生していません。"},
			{ExceptionType.CalcUndefinedFunctionCallback, "関数の演算用コールバックが未定義です。"},
			{ExceptionType.CalcDuplicateIndex, "既に登録されている添え字が指定されました。"},
			{ExceptionType.CalcDuplicateIndexPosition, "既に登録されている添え字の位置が指定されました。"},
			{ExceptionType.CalcUnknownIndex, "削除する添え字が見つかりません。"},
		};

		private String FoxMessage = "";

		public override string Message
		{
			get
			{
				//return base.Message;
				return FoxMessage;
			}
		}

		public FoxException(ExceptionType type)
		{
			FoxMessage = MessageDictionary[type];
		}


		#region 例外発生時のログおよびトレース出力
		/// <summary>
		/// 例外発生時のログおよびトレース出力
		/// </summary>
		/// <param name="ex"></param>
		public static void TraceException(Exception ex)
		{
			// 発生元関数名の取得
			StackFrame stackFrame = new StackTrace().GetFrame(1);
			string methodName = stackFrame.GetMethod().ToString();

			// 現在の例外情報をメッセージに追加
			StringBuilder message = new StringBuilder();
			message.AppendFormat("[Exception:{0}]\n", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff"));
			message.AppendFormat("{0}: {1}\n\n", methodName, ex.Message);

			// 現在の例外を発生させた元をメッセージに追加
			Exception innerException = ex.InnerException;
			while (innerException != null)
			{
				message.AppendFormat("{0}:{1}\n", innerException.Source, innerException.Message);
				innerException = innerException.InnerException;
			}

			// スタックトレースをメッセージに追加
			message.Append(ex.StackTrace);

			// トレースおよびログに出力
			Console.WriteLine(message.ToString());
		}
		#endregion
	}

	public enum ExceptionType
	{
		/// <summary>
		/// 例外発生なし
		/// </summary>
		Non,

		/// <summary>
		/// 演算:関数コールバックの未定義
		/// </summary>
		CalcUndefinedFunctionCallback,

		/// <summary>
		/// 演算:添え字の重複
		/// </summary>
		CalcDuplicateIndex,

		/// <summary>
		/// 演算:添え字の位置重複
		/// </summary>
		CalcDuplicateIndexPosition,

		/// <summary>
		/// 演算:削除対象の添え字がない
		/// </summary>
		CalcUnknownIndex,
	}
}
