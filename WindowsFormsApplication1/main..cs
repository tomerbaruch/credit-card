using System;
using System.Collections.Generic;

public static void Main(string[] args)
{
		Dictionary<string, string> shop_category_hash = loadHash();
		Dictionary<string, double?> result_map = new Dictionary<string, double?>();
		Dictionary<string, int?> shops = new Dictionary<string, int?>();

		Console.WriteLine("Please enter excel file name");
		Scanner read = new Scanner(System.in);
		string excel_name = read.nextLine();

		try
		{
			System.IO.FileStream fileInputStream = new System.IO.FileStream("C:\\credit\\" + excel_name + ".xlsx", System.IO.FileMode.Open, System.IO.FileAccess.Read);
			XSSFWorkbook workbook = new XSSFWorkbook(fileInputStream);
			XSSFSheet worksheet = workbook.getSheetAt(0);
			//init
			int i = 11;
			XSSFRow row1 = worksheet.getRow(i);
			XSSFCell cellA1 = row1.getCell((short) 2);
			XSSFCell cellA2 = row1.getCell((short) 4);
			string shop_name = cellA1.StringCellValue;
			double money = cellA2.NumericCellValue;

			while (!shop_name.Equals(""))
			{
				double curr_sum;
				if (shop_category_hash.ContainsKey(shop_name))
				{
					if (shops.ContainsKey(shop_name))
					{
						shops[shop_name] = shops[shop_name] + 1;
					}
					else
					{
						shops[shop_name] = 1;
					}
					string cat = shop_category_hash[shop_name];
					add_shop_to_result(result_map, cat, money);
				}
				else
				{
					Console.WriteLine("Please enter category for " + shop_name);
					Scanner reader = new Scanner(System.in); // Reading from System.in
					string cat = reader.nextLine();
					try
					{
						using (PrintWriter output = new PrintWriter(new System.IO.StreamWriter("C:\\credit\\shops.txt",true)))
						{
							output.printf("%s#%s\r\n", shop_name, cat);
							shop_category_hash[shop_name] = cat;
							add_shop_to_result(result_map, cat, money);
						}
					}
					catch (Exception)
					{
					}
				}

				i++;
				row1 = worksheet.getRow(i);
				cellA1 = row1.getCell((short) 2);
				cellA2 = row1.getCell((short) 4);
				shop_name = cellA1.StringCellValue;
				money = cellA2.NumericCellValue;
			}

			print_result(result_map);
			print_attention(shops);


		}
		catch (FileNotFoundException e)
		{
			Console.WriteLine(e.ToString());
			Console.Write(e.StackTrace);
		}
		catch (IOException e)
		{
			Console.WriteLine(e.ToString());
			Console.Write(e.StackTrace);
		}
}

using System;
using System.Collections.Generic;

public static void print_attention(Dictionary<string, int?> shops)
{
		foreach (string name in shops.Keys)
		{
			string key = name;
			int value = shops[name].Value;
			if (value > 1)
			{
			Console.WriteLine("Pay attention you have " + value + " charges from " + key);
			}
		}
}

	public static void print_result(Dictionary<string, double?> result_map)
	{
		foreach (string name in result_map.Keys)
		{
			string key = name;
			double? value = result_map[name];
			Console.WriteLine(key + " " + value);
		}
	}

	public static void add_shop_to_result(Dictionary<string, double?> result_map, string cat, double money)
	{
		if (result_map.ContainsKey(cat))
		{
			double curr_sum = result_map[cat].Value;
			result_map[cat] = curr_sum + money;
		}
		else
		{
			result_map[cat] = money;
		}
	}

	public static Dictionary<string, string> loadHash()
	{
		Dictionary<string, string> shop_category_hash = new Dictionary<string, string>();

		System.IO.StreamReader br = new System.IO.StreamReader("C:\\credit\\shops.txt");
		try
		{
			string line = br.ReadLine();

			while (line != null)
			{
				string[] arr = line.Split("#", true);
				if (shop_category_hash.ContainsKey(arr[0]))
				{
					line = br.ReadLine();
					continue;
				}
				string shop = arr[0];
				string category = arr[1];
				shop_category_hash[shop] = category;
				line = br.ReadLine();
			}
		}
		catch (Exception)
		{

		}
		finally
		{
			br.Close();
		}

		return shop_category_hash;
	}

//-------------------------------------------------------------------------------------------
//	Copyright © 2007 - 2015 Tangible Software Solutions Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class is used to convert some aspects of the Java String class.
//-------------------------------------------------------------------------------------------
internal static class StringHelperClass
{
	//----------------------------------------------------------------------------------
	//	This method replaces the Java String.substring method when 'start' is a
	//	method call or calculated value to ensure that 'start' is obtained just once.
	//----------------------------------------------------------------------------------
	internal static string SubstringSpecial(this string self, int start, int end)
	{
		return self.Substring(start, end - start);
	}

	//------------------------------------------------------------------------------------
	//	This method is used to replace calls to the 2-arg Java String.startsWith method.
	//------------------------------------------------------------------------------------
	internal static bool StartsWith(this string self, string prefix, int toffset)
	{
		return self.IndexOf(prefix, toffset, System.StringComparison.Ordinal) == toffset;
	}

	//------------------------------------------------------------------------------
	//	This method is used to replace most calls to the Java String.split method.
	//------------------------------------------------------------------------------
	internal static string[] Split(this string self, string regexDelimiter, bool trimTrailingEmptyStrings)
	{
		string[] splitArray = System.Text.RegularExpressions.Regex.Split(self, regexDelimiter);

		if (trimTrailingEmptyStrings)
		{
			if (splitArray.Length > 1)
			{
				for (int i = splitArray.Length; i > 0; i--)
				{
					if (splitArray[i - 1].Length > 0)
					{
						if (i < splitArray.Length)
							System.Array.Resize(ref splitArray, i);

						break;
					}
				}
			}
		}

		return splitArray;
	}

	//-----------------------------------------------------------------------------
	//	These methods are used to replace calls to some Java String constructors.
	//-----------------------------------------------------------------------------
	internal static string NewString(sbyte[] bytes)
	{
		return NewString(bytes, 0, bytes.Length);
	}
	internal static string NewString(sbyte[] bytes, int index, int count)
	{
		return System.Text.Encoding.UTF8.GetString((byte[])(object)bytes, index, count);
	}
	internal static string NewString(sbyte[] bytes, string encoding)
	{
		return NewString(bytes, 0, bytes.Length, encoding);
	}
	internal static string NewString(sbyte[] bytes, int index, int count, string encoding)
	{
		return System.Text.Encoding.GetEncoding(encoding).GetString((byte[])(object)bytes, index, count);
	}

	//--------------------------------------------------------------------------------
	//	These methods are used to replace calls to the Java String.getBytes methods.
	//--------------------------------------------------------------------------------
	internal static sbyte[] GetBytes(this string self)
	{
		return GetSBytesForEncoding(System.Text.Encoding.UTF8, self);
	}
	internal static sbyte[] GetBytes(this string self, string encoding)
	{
		return GetSBytesForEncoding(System.Text.Encoding.GetEncoding(encoding), self);
	}
	private static sbyte[] GetSBytesForEncoding(System.Text.Encoding encoding, string s)
	{
		sbyte[] sbytes = new sbyte[encoding.GetByteCount(s)];
		encoding.GetBytes(s, 0, s.Length, (byte[])(object)sbytes, 0);
		return sbytes;
	}
}
