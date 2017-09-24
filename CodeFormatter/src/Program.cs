using System;
using System.IO;
using ICSharpCode.NRefactory.CSharp;

namespace CodeFormatter
{
	/// <summary>
	/// 引数に渡された*.csファイルをフォーマットし、上書き保存するプログラムです
	/// </summary>
	class Program
	{
		static int Main(string[] args)
		{
			Console.WriteLine("");
			Console.WriteLine("==============================");

			// パラメータチェック
			if (args.Length <= 0)
			{
				Console.WriteLine("[FAILURE] : no parameter...");
				return 1;
			}

			// コードフォーマットを行うファイル名を引数から取得
			var targetFileName = args[0];
			// ファイルが存在しないなら何もしない
			if (!File.Exists(targetFileName))
			{
				Console.WriteLine("[FAILURE] : file not found...");
				return 1;
			}

			// オプションの設定を読み込む
			FormattingOptions.Load();

			// フォーマットを行うソースコードの中身が全て入る
			string targetSourceCode;
			using (var reader = new StreamReader(targetFileName))
			{
				targetSourceCode = reader.ReadToEnd();
			}

			// ソースコードを読み込んだ結果、空っぽの場合は何もせず終了する
			if (string.IsNullOrEmpty(targetSourceCode))
			{
				Console.WriteLine("[FAILURE] : not .cs file...");
				return 1;
			}

			// ソースコードをフォーマットする
			CSharpFormatter formatter = new CSharpFormatter(FormattingOptions.options);
			var formatSourceCode = formatter.Format(targetSourceCode);

			// 同じファイル名で出力する
			using (var writer = new StreamWriter(targetFileName))
			{
				writer.Write(formatSourceCode);
			}
			Console.WriteLine("format... : " + targetFileName);

			return 0;
		}
	}
}
