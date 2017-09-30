using System;
using System.IO;
using ICSharpCode.NRefactory.CSharp;

namespace CodeFormatter
{
	/// <summary>
	/// FormattingOptionの管理を行います。
	/// </summary>
	public static class FormattingOptions
	{
		/// <summary>
		/// FormattingOptionの設定を保持します
		/// </summary>
		public static CSharpFormattingOptions options = FormattingOptionsFactory.CreateAllman();

		/// <summary>
		/// FromattingOption設定を読み込みます
		/// 読み込みに失敗した場合はデフォルトの値が使用されます
		/// </summary>
		/// <param name="filePath"></param>
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
					options = (CSharpFormattingOptions)serializer.Deserialize(reader);
				}
				throw new ArgumentNullException();
			}
			catch (FileNotFoundException)
			{
				// ファイルが存在しない場合は新規作成する
				Save(filePath);
			}
			catch (Exception)
			{

			}
		}

		/// <summary>
		/// FormattingOption設定を書き込みます
		/// </summary>
		/// <param name="filePath"></param>
		public static void Save(string filePath = "settings.xml")
		{
			// 書き込むファイルを開く（UTF-8 BOM無し）
			using (var writer = new StreamWriter(filePath, false, new System.Text.UTF8Encoding(false)))
			{
				//オブジェクトの型を指定する
				System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CSharpFormattingOptions));
				//シリアル化し、XMLファイルに保存する
				serializer.Serialize(writer, options);
			}
		}
	}
}
