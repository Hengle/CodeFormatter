using System;
using System.IO;
using ICSharpCode.NRefactory.CSharp;


namespace CodeFormatter
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length <= 0)
			{
				return;
			}
			// コードフォーマットを行うファイル名を引数から取得
			var targetFileName = args[0];
			// ファイルが存在しないなら何もしない
			if(!File.Exists(targetFileName))
			{
				return;
			}

			// オプションの設定を読み込む
			FormattingOptions.Load();

			// フォーマットを行うソースコードの中身が全て入る
			var targetSourceCode = "";
			using (var reader = new StreamReader(targetFileName))
			{
				targetSourceCode = reader.ReadToEnd();
			}

			// ソースコードを読み込んだ結果、空っぽの場合は何もせず終了する
			if (string.IsNullOrEmpty(targetSourceCode))
			{
				return;
			}
			/*
			CompilerSettings a = new CompilerSettings();
			var parser = new CSharpParser();


			var root = parser.Parse(targetSourceCode);

			//			var program = root.Descendants.OfType<ICSharpCode.NRefactory.CSharp.TypeDeclaration>().First();
			//			program.Name = "Hogegram";
			var newLineNode = new NewLineNode();
			newLineNode.NewLineType = ICSharpCode.NRefactory.UnicodeNewline.CRLF;
			root.Parent.InsertChildBefore(root, newLineNode, Roles.NewLine);
			root.SetFormattingOptions(FormattingOptions.DefaultOptions);

			Console.WriteLine(root.ToSourceString());
			using (var writer = new StreamWriter(targetFileName + "new.cs"))
			{
				writer.Write(root.ToSourceString());
			}
			*/

			CSharpFormatter f = new CSharpFormatter(FormattingOptions.DefaultOptions);
			var dest = f.Format(targetSourceCode);
			using (var writer = new StreamWriter(targetFileName + "new.cs"))
			{
				writer.Write(dest);
			}
		}
	}
}
