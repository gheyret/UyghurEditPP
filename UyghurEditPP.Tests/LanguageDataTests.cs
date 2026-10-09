using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UyghurEditPP.Tests
{
	[TestClass]
	public class LanguageDataTests
	{
		// GetText("...") anywhere, and Text("...") in Program.cs (its wrapper around GetText).
		static readonly Regex gGetText = new Regex(@"\bGetText\(\s*""((?:[^""\\]|\\.)*)""\s*\)");
		static readonly Regex gText = new Regex(@"\bText\(\s*""((?:[^""\\]|\\.)*)""\s*\)");

		static string RepoRoot()
		{
			DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
			while(dir != null && !File.Exists(Path.Combine(dir.FullName, "UyghurEditPP.sln"))){
				dir = dir.Parent;
			}
			Assert.IsNotNull(dir, "UyghurEditPP.sln was not found above the test folder");
			return dir.FullName;
		}

		// The keys passed to GetText in the program's source (commented-out lines are skipped).
		static Dictionary<string, string> CodeKeys()
		{
			string root = RepoRoot();
			string[] skip = { "bin", "obj", "packages", "UyghurEditPP.Tests", "TestResults" };
			Dictionary<string, string> keys = new Dictionary<string, string>();
			foreach(string file in Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories)){
				string rel = file.Substring(root.Length).TrimStart('\\');
				if(skip.Contains(rel.Split('\\')[0])){
					continue;
				}
				string[] lines = File.ReadAllLines(file, Encoding.UTF8);
				for(int i = 0; i < lines.Length; i++){
					if(lines[i].TrimStart().StartsWith("//", StringComparison.Ordinal)){
						continue;
					}
					List<Match> found = gGetText.Matches(lines[i]).Cast<Match>().ToList();
					if(Path.GetFileName(file) == "Program.cs"){
						found.AddRange(gText.Matches(lines[i]).Cast<Match>());
					}
					foreach(Match m in found){
						string key = Regex.Unescape(m.Groups[1].Value);
						if(!keys.ContainsKey(key)){
							keys[key] = rel + ":" + (i + 1);
						}
					}
				}
			}
			return keys;
		}

		// langdata.json as built into the program.
		static dynamic LangData()
		{
			using(Stream stm = typeof(Language).Assembly.GetManifestResourceStream("UyghurEditPP.langdata.json")){
				Assert.IsNotNull(stm, "langdata.json is not embedded");
				using(StreamReader rd = new StreamReader(stm, Encoding.UTF8)){
					return DynaJson.Parse(rd.ReadToEnd());
				}
			}
		}

		[TestMethod]
		public void EveryKeyInTheCode_HasEnglishAndJapanese()
		{
			Dictionary<string, string> keys = CodeKeys();
			Assert.IsTrue(keys.Count > 100, "only " + keys.Count + " keys found");
			dynamic data = LangData();
			List<string> problems = new List<string>();
			foreach(KeyValuePair<string, string> kv in keys){
				if(!data.IsDefined(kv.Key)){
					problems.Add("missing: " + kv.Key + " (" + kv.Value + ")");
					continue;
				}
				foreach(string til in new[] { "eng", "jpn" }){
					string text = data[kv.Key].IsDefined(til) ? (string)data[kv.Key][til] : null;
					if(string.IsNullOrEmpty(text)){
						problems.Add("no " + til + ": " + kv.Key + " (" + kv.Value + ")");
					}
				}
			}
			Assert.AreEqual(0, problems.Count, string.Join(Environment.NewLine, problems));
		}

		[TestMethod]
		public void EveryKeyInLangData_IsUsedInTheCode()
		{
			Dictionary<string, string> keys = CodeKeys();
			List<string> unused = new List<string>();
			// DynaJson turns an object into a Dictionary<string, object> when cast to IEnumerable.
			System.Collections.IEnumerable data = LangData();
			foreach(string key in ((Dictionary<string, object>)data).Keys){
				if(!keys.ContainsKey(key)){
					unused.Add(key);
				}
			}
			Assert.AreEqual(0, unused.Count, string.Join(Environment.NewLine, unused));
		}

		const string Json = @"{
  ""A"": { ""jpn"": ""エー"", ""eng"": ""Ay"" },
  ""B"": { ""jpn"": """", ""eng"": ""Bee"" },
  ""C"": { ""jpn"": """", ""eng"": """" },
  ""D"": { ""eng"": ""Dee"" },
  ""E"": { ""uly"": ""Éy"", ""jpn"": """", ""eng"": ""E"" }
}";

		[TestMethod]
		public void Japanese_FallsBackToEnglishThenToTheKey()
		{
			Language lang = new Language(Json);
			lang.LanguaID = "jpn";

			Assert.AreEqual("エー", lang.GetText("A"));
			Assert.AreEqual("Bee", lang.GetText("B"));
			Assert.AreEqual("C", lang.GetText("C"));
			Assert.AreEqual("Dee", lang.GetText("D"));
			Assert.AreEqual("E", lang.GetText("E"));
			Assert.AreEqual("not in the data", lang.GetText("not in the data"));
		}

		[TestMethod]
		public void English_FallsBackToTheKey()
		{
			Language lang = new Language(Json);
			lang.LanguaID = "eng";

			Assert.AreEqual("Ay", lang.GetText("A"));
			Assert.AreEqual("Bee", lang.GetText("B"));
			Assert.AreEqual("C", lang.GetText("C"));
			Assert.AreEqual("not in the data", lang.GetText("not in the data"));
		}

		[TestMethod]
		public void Uyghur_IsNotAffectedByTheTranslations()
		{
			Language lang = new Language(Json);
			lang.LanguaID = "uly";

			Assert.AreEqual("A", lang.GetText("A"));
			Assert.AreEqual("Éy", lang.GetText("E"));
		}

		[TestMethod]
		public void RealData_EnglishAndJapaneseAreShown()
		{
			Language lang = new Language();
			lang.LanguaID = "eng";
			Assert.AreEqual("File", lang.GetText("Höjjet"));
			lang.LanguaID = "jpn";
			Assert.AreEqual("ファイル", lang.GetText("Höjjet"));
			lang.LanguaID = "uly";
			Assert.AreEqual("Höjjet", lang.GetText("Höjjet"));
		}
	}
}
