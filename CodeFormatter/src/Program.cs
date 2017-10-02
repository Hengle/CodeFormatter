using System;
using System.IO;
using ICSharpCode.NRefactory.CSharp;
using System.Linq;
using Mono.Options;

namespace CodeFormatter
{
	/// <summary>
	/// 引数に渡された*.csファイルをフォーマットし、上書き保存するプログラムです
	/// </summary>
	class Program
	{
		static int Main(string[] args)
		{
			string settingsFilePath = null;
			bool showHelp = false;

			var options = new OptionSet()
			{
				{ "s|settings=", "settings file path", v => settingsFilePath = v },
				{ "h|help", "show help", v => showHelp = (v != null) }
			};
			var sourceFileList = options.Parse(args);

			if (showHelp)
			{
				ShowUsage(options);
				return 1;
			}

			// オプションの設定を読み込む
			if (string.IsNullOrEmpty(settingsFilePath))
			{
				FormattingOptions.Load();
			}
			else
			{
				FormattingOptions.Load(settingsFilePath);
			}

			// フォーマットに失敗したファイルが一つでもあればtrueとなる
			bool formatFailure = false;
			foreach (var source in sourceFileList)
			{
				if (!File.Exists(source))
				{
					Console.WriteLine("[CodeFormatter FAILURE] file not found... : " + source);
					formatFailure = true;
					continue;
				}
				if (System.IO.Path.GetExtension(source) != ".cs")
				{
					Console.WriteLine("[CodeFormatter FAILURE] unsupported file... : " + source);
					formatFailure = true;
					continue;
				}

				try
				{
					Format(source);
					Console.WriteLine("[CodeFormatter SUCCESS] : " + source);
				}
				catch (Exception e)
				{
					Console.Error.WriteLine("[CodeFormatter FAILURE]" + e.Message + " : " + source);
					Console.Error.WriteLine(e.StackTrace);
				}
			}

			return (formatFailure) ? 1 : 0;
		}

		/// <summary>
		/// 実際にフォーマットを行う
		/// </summary>
		/// <param name="path">フォーマットするファイルへのパス</param>
		private static void Format(string path)
		{
			// フォーマットを行うソースコードを全て読み込む
			string targetSourceCode;
			using (var reader = new StreamReader(path))
			{
				targetSourceCode = reader.ReadToEnd();
			}

			// ソースコードをフォーマットする
			CSharpFormatter formatter = new CSharpFormatter(FormattingOptions.options);
			var formatSourceCode = formatter.Format(targetSourceCode);

			// 同じファイル名で出力する
			using (var writer = new StreamWriter(path))
			{
				writer.Write(formatSourceCode);
			}
		}

		// Uasgeを表示する
		private static void ShowUsage(OptionSet p)
		{
			Console.Error.WriteLine("Usage:CodeFormatter [OPTIONS] SOURCE...");
			Console.Error.WriteLine();
			p.WriteOptionDescriptions(Console.Out);
		}
	}
}
