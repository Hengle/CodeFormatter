using System;
using System.IO;
using ICSharpCode.NRefactory.CSharp;

namespace CodeFormatter
{
	public static class FormattingOptions
	{
		public static CSharpFormattingOptions DefaultOptions = FormattingOptionsFactory.CreateAllman();

		public static void Load(string filePath = "settings.xml")
		{
			try
			{
				// 設定ファイルを開く（UTF-8 BOM無し）
				using (var reader = new StreamReader(filePath, new System.Text.UTF8Encoding(false)))
				{
					//XmlSerializerオブジェクトを作成
					System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CSharpFormattingOptions));
					//XMLファイルから読み込み、逆シリアル化する
					DefaultOptions = (CSharpFormattingOptions)serializer.Deserialize(reader);
				}
			}
			catch
			{
				Save(filePath);
			}
		}

		public static void Save(string filePath = "settings.xml")
		{
			// 書き込むファイルを開く（UTF-8 BOM無し）
			using (var writer = new StreamWriter(filePath, false, new System.Text.UTF8Encoding(false)))
			{
				//オブジェクトの型を指定する
				System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CSharpFormattingOptions));
				//シリアル化し、XMLファイルに保存する
				serializer.Serialize(writer, DefaultOptions);
			}
		}
	}
}
