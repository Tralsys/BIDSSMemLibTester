using System;

using TR.BIDSSMemLib;

namespace BIDSSMemLibWriteTester
{
	class Program
	{
		const char PairConnectChar = ':';
		const char IndexSeparatorChar = ',';
		const char IndexRangeSettingChar = '_';

		static void Main()
		{
			//Console.WriteLine("get panel nnn : get value in panel[nnn].  eg:\"get panel 10\"");
			Console.WriteLine("set panel nnn:xxx : set value\"xxx\" to panel[nnn].  eg:\"set panel 10:20\" => value of panel[10] will be 20");
			Console.WriteLine("set panel nnn,aaa:xxx : set value\"xxx\" to panel[nnn] and panel[aaa].  eg:\"set panel 10,50,123:20\" => value of panel[10], panel[50], and panel[123] will be 20");
			Console.WriteLine("set panel nnn_aaa:xxx : set value\"xxx\" to panel[nnn] ~ panel[aaa - 1].  eg:\"set panel 10_13:20\" => value of panel[10], panel[11], and panel[12] will be 20");
			Console.WriteLine("set panel abc,nnn_aaa,nxn:xxx abc:yyy : set value\"xxx\" to panel[nnn] ~ panel[aaa - 1] and panel[nxn].  And, set value \"yyy\" to panel[abc]  eg:\"set panel 99,10_13:20 99_101:5\" => value of panel[10], panel[11], and panel[12] will be 20, and panel[99] and panel[100] will be 5");
			Console.WriteLine("");
			Console.WriteLine("To end this program, enter \"exit\"");

			SMemLib.Begin();
			int[] arr = SMemLib.ReadPanel();
			if(arr.Length<256)
			{
				int[] narr = new int[256];
				Array.Copy(arr, narr, arr.Length);
				SMemLib.WritePanel(narr);
			}

			string? s;
			while ((s = Console.ReadLine()) != "exit")
			{
				try
				{
					if (string.IsNullOrWhiteSpace(s))
						continue;

					string[] sa = s.Split(' ');
					ValueTypes vt = GetValueType(sa[1]);
					switch (sa[0])
					{
						case "g":
						case "ge":
						case "get":
							break;

						case "s":
						case "se":
						case "set":
							switch (vt)
							{
								case ValueTypes.Panel:
									SetPanel(sa.AsSpan(2));
									break;
							}
							break;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
		}

		static void SetPanel(in Span<string> PosValPairs) => SMemLib.WritePanel(SetArrayData(SMemLib.ReadPanel(), PosValPairs));
		
		static int[] SetArrayData(int[] arr, in Span<string> PosValPairs)
		{
			foreach (var PosValPair in PosValPairs)
			{
				string[] PosAndValue = PosValPair.Split(PairConnectChar);
				foreach (var pos in PosAndValue[0].Split(IndexSeparatorChar))
				{
					int val = int.Parse(PosAndValue[1]);
					if (pos.Contains(IndexRangeSettingChar))
					{
						string[] indexRange = pos.Split(IndexRangeSettingChar);
						int from = int.Parse(indexRange[0]);
						int to = int.Parse(indexRange[1]);
						for (int i = from; i < to; i++)
							arr[i] = val;
					}
					else
						arr[int.Parse(pos)] = val;
				}
			}

			return arr;
		}

		enum ValueTypes
		{
			None,
			Panel,
			Sound
		}
		static ValueTypes GetValueType(in string s)
			=> s switch
			{
				"p" or "pa" or "pan" or "pane" or "panel" or "pnl" => ValueTypes.Panel,
				"s" or "so" or "sou" or "soun" or "sound" or "snd" => ValueTypes.Sound,
				_ => ValueTypes.None
			};
	}
}
