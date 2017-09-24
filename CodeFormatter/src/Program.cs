using System;
using System.IO;
using ICSharpCode.NRefactory.CSharp;
using System.Linq;

namespace CodeFormatter
{
	/// <summary>
	/// 引数に渡された*.csファイルをフォーマットし、上書き保存するプログラムです
	/// </summary>
	class Program
	{
		static int Main(string[] args)
		{
			// パラメータチェック
			if (args.Length <= 0)
			{
				Console.WriteLine("[CodeFormatter FAILURE] : no parameter...");
				return 1;
			}

			#region TODO コマンドライン引数のパーサーなりを使って解析するようにするべき
			//参考: http://qiita.com/Marimoiro/items/a090344432a5f69e1fac
			// オプションのみと　そもそも入っていないの識別
			args = args.Concat(new string[] { "" }).ToArray();
			var options = new string[] { "-s", "-o" };

			var result = options.ToDictionary(p => p.Substring(1), p => args.SkipWhile(a => a != p).Skip(1).FirstOrDefault());
			#endregion

			// コードフォーマットを行うファイル名を引数から取得
			var targetFileName = result["o"];
			// ファイルが存在しないなら何もしない
			if (!File.Exists(targetFileName))
			{
				Console.WriteLine("[CodeFormatter FAILURE] file not found... : " + targetFileName);
				return 1;
			}

			// オプションの設定を読み込む + targetFileName
			FormattingOptions.Load(result["s"]);

			// フォーマットを行うソースコードの中身が全て入る
			string targetSourceCode;
			using (var reader = new StreamReader(targetFileName))
			{
				targetSourceCode = reader.ReadToEnd();
			}

			// ソースコードを読み込んだ結果、空っぽの場合は何もせず終了する
			if (string.IsNullOrEmpty(targetSourceCode))
			{
				Console.WriteLine("[CodeFormatter FAILURE] not .cs file... : " + targetFileName);
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
			Console.WriteLine("[CodeFormatter SUCCESS] : " + targetFileName);

			return 0;
		}
	}
}
