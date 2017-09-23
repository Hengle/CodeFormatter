using System.IO;
using ICSharpCode.NRefactory.CSharp;

namespace CodeFormatter
{
	public static class SyntaxTreeExtensions
	{
		//		static readonly CSharpFormattingOptions DefaultOptions = new CSharpFormattingOptions()
		//		{
		//			// TODO:130のboolを自分の気にいるようなOption（というかVSのデフォ）に近づける
		//		};

		public static CSharpFormattingOptions DefaultOptions = FormattingOptionsFactory.CreateAllman();

		public static void SetFormattingOptions(this SyntaxTree syntaxTree, CSharpFormattingOptions options)
		{
			DefaultOptions = options;
		}

		public static string ToSourceString(this SyntaxTree syntaxTree, int indentation = 0, string indentationString = "	")
		{
			return ToSourceString(syntaxTree, DefaultOptions, indentation, indentationString);
		}

		public static string ToSourceString(this SyntaxTree compilationUnit, CSharpFormattingOptions options, int indentation = 0, string indentationString = "	")
		{
			using (var sw = new StringWriter())
			{
				var formatter = new ICSharpCode.NRefactory.CSharp.TextWriterTokenWriter(sw)
				{
					Indentation = indentation,
					IndentationString = indentationString
				};

				var visitor = new CSharpOutputVisitor(formatter, options);
				compilationUnit.AcceptVisitor(visitor);
				return sw.ToString();
			}
		}
	}
}
