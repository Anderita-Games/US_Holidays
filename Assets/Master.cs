using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Master : MonoBehaviour {
	public TextAsset US_Holidays;
	public string[] Holidays;
	public string[] Dates;
	public UnityEngine.UI.Text[] Upcoming_Holidays;
	public UnityEngine.UI.Text Date_Text;
	public UnityEngine.UI.Text Holiday_Text;
	public int Text_Length;
	public DateTime ClosestDate;

	void Start () {
		Text_Length = US_Holidays.text.Split('\n').Length;
		Holidays = new string[Text_Length];
		Dates = new string[Text_Length];
		StreamReader SR = new StreamReader(new MemoryStream(US_Holidays.bytes));
		for (int i = 0; i < Text_Length; i++) {
			string Current_Line = SR.ReadLine();
			bool Second_Part = false;
			foreach (char Char in Current_Line) {
				if (Char != ':') {
					if (Second_Part == false) {
						Holidays[i] = Holidays[i] + Char;
					}else {
						Dates[i] = Dates[i] + Char;
					}
				}else {
					Second_Part = true;
				}
			}
		}
		SR.Close(); 
		Organize_Dates();
	}

	void Organize_Dates () { //MM/DD/YYYY
		long Min = long.MaxValue;
		for (int i = 0; i < Dates.Length; i++) {
			string Temp_Date = Dates[i];
			int Month = int.Parse(Dates[i].Substring(0, 2));
			int Day = int.Parse(Dates[i].Substring(3, 2));
			int Year = int.Parse(Dates[i].Substring(Dates[i].Length - 4));
			string Current_Year = (DateTime.Now.Year + 1).ToString();
			if (Month < DateTime.Now.Month && Year <= DateTime.Now.Year) {
				Dates[i] = Dates[i].Substring(0, 6) + Current_Year;
			}else if (Month == DateTime.Now.Month && Day < DateTime.Now.Day && Year <= DateTime.Now.Year) {
				Dates[i] = Dates[i].Substring(0, 6) + Current_Year;
			}
			long Difference = Math.Abs(DateTime.Now.Ticks - DateTime.Parse(Dates[i]).Ticks);
			Debug.Log(Difference);
			if (Difference < Min)  {
				Min = Difference;
				ClosestDate = DateTime.Parse(Dates[i]);
				Date_Text.text = ClosestDate.ToString("D");
				Holiday_Text.text = Holidays[i];
				for (int a = 1; a <= 5; a++) {
					Upcoming_Holidays[a - 1].text = Holidays[a - 1] + ": " + Dates[a - 1];
				}
				for (int a = 1; a <= 5; a++) {
					if (a != 5) {
						Upcoming_Holidays[a].text = Upcoming_Holidays[a - 1].text;
					}
					Upcoming_Holidays[a - 1].text = Holidays[i + a] + ": " + Dates[i + a];
				}
			}
		}
	}
}