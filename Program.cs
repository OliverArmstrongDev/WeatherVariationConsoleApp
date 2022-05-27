using System;

namespace WeatherVariance
{
    public class Program
    {

        private class WeatherVars
        {
            public int MinVariationDayNumber;
            public List<Decimal> minTempsList = new List<Decimal>();
            public List<Decimal> maxTempsList = new List<Decimal>();
            public string path = "./weather.txt";
            public int minIndx = -1;
            public int maxIndx = -1;
            public string CurrentMonth = "For the month of ";
            public string MinVariationHeading = "----------- Minimum Temperarature Variation ------------";
            public string MinVariationString = "The day which had the least difference between high and low temperatures was: ";
            public string MinVariationMinTempString = "Min temp that day was: ";
            public string MinVariationMaxTempString = "Max temp that day was: ";
            public string divider = "----------------------------------------------------------";


        }
        private class Months
        {
            public string[] monthList = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        }
        static void Main(string[] args)
        {
            string fileErrorMessage = "To get the temperature variation, weather.txt must be in the root directory. This application will now Exit!";
            Program prog = new Program();
            WeatherVars weatherVars = new WeatherVars();
            if (File.Exists(weatherVars.path))
            {
                prog.MainMenu();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (fileErrorMessage.Length / 2)) + "}", fileErrorMessage));
                Console.ResetColor();
                Console.ReadKey();
                Environment.Exit(1);
            }

        }

        public void MainMenu()
        {
            string titleText = "Welcome to the \"Weather Minimum Variation App.\"";
            string divider = "-----------------------------------------------------------------------------";
            string optionHeader = "What would you like to do?:";
            string option1 = "1) Get Minimum Temperature variation ";
            string optionSelectMessage = "Type the number of the option you want:";
            string exitApp = "2) Exit                               ";

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine();
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (titleText.Length / 2)) + "}", titleText));
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (divider.Length / 2)) + "}", divider));
            Console.WriteLine();
            Console.ResetColor();
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (optionHeader.Length / 2)) + "}", optionHeader));
            Console.WriteLine();
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (option1.Length / 2)) + "}", option1));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (exitApp.Length / 2)) + "}", exitApp));
            Console.WriteLine();
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (optionSelectMessage.Length / 2)) + "}", optionSelectMessage));

            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        DoWork();
                        break;
                    case "2":
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("That is not a valid option. Please choose a number shown...");
                        break;
                }
            }
        }

        public void DoWork()
        {
            WeatherVars weatherVars = new WeatherVars();
            Months months = new Months();
            try
            {
                string[] contents = File.ReadAllLines(weatherVars.path);

                for (int i = 0; i < contents.Length; i++)
                {
                    string[] columns = contents[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    for (int c = 0; c < columns.Length; c++)
                    {
                        if (months.monthList.Any(columns[c].Contains))
                        {
                            string dateString = weatherVars.CurrentMonth + columns[c] + " " + columns[c + 1];
                            weatherVars.CurrentMonth = dateString;
                            continue;
                        }
                        //find max temp column
                        if (columns[c].Contains("MxT"))
                        {
                            weatherVars.maxIndx = c;
                            continue;
                        }
                        //find min temp column
                        if (columns[c].Contains("MnT"))
                        {
                            weatherVars.minIndx = c;
                            break;
                        }
                        if ((weatherVars.minIndx > -1) && (weatherVars.maxIndx > -1))
                        {
                            //if NOT the </pre> tag, continue...
                            if (!columns.Contains("</pre>"))
                            {
                                var minTempVal = Convert.ToDecimal(columns[weatherVars.minIndx].Replace("*", ""));
                                var maxTempVal = Convert.ToDecimal(columns[weatherVars.maxIndx].Replace("*", ""));
                                weatherVars.minTempsList.Add(minTempVal);
                                weatherVars.maxTempsList.Add(maxTempVal);
                                break;
                            }
                        }
                    }
                }

                List<Decimal> Tmp_TemperaturesList = new List<Decimal>();
                decimal minVal = 0;

                for (int i = 0; i < weatherVars.minTempsList.Count - 1; i++)
                {
                    var result = Math.Abs(weatherVars.minTempsList[i] - weatherVars.maxTempsList[i]);
                    Tmp_TemperaturesList.Add(result);
                    minVal = Tmp_TemperaturesList.Min(t => t);
                    weatherVars.MinVariationDayNumber = (Tmp_TemperaturesList.IndexOf(minVal) + 1);
                }


                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (weatherVars.MinVariationHeading.Length / 2)) + "}", weatherVars.MinVariationHeading));
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (weatherVars.CurrentMonth.Length / 2)) + "}", weatherVars.CurrentMonth));
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (weatherVars.divider.Length / 2)) + "}", weatherVars.divider));
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (weatherVars.MinVariationString.Length / 2)) + "}", weatherVars.MinVariationString));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Day " + (weatherVars.MinVariationDayNumber));
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (weatherVars.MinVariationMaxTempString.Length / 2)) + "}", weatherVars.MinVariationMaxTempString));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write((weatherVars.maxTempsList[Tmp_TemperaturesList.IndexOf(minVal)]));
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (weatherVars.MinVariationMinTempString.Length / 2)) + "}", weatherVars.MinVariationMinTempString));
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write((weatherVars.minTempsList[Tmp_TemperaturesList.IndexOf(minVal)]));
                Console.WriteLine();
                Console.WriteLine();
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (weatherVars.divider.Length / 2)) + "}", weatherVars.divider));



            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("weather.txt not found! It must be in the same directory as this app.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();

            }

            MenuOptions();

        }

        public void MenuOptions()
        {
            string MenuReturnMessage = "<- PRESS ANY KEY TO RETURN TO THE MENU ";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (MenuReturnMessage.Length / 2)) + "}", MenuReturnMessage));
            Console.ReadKey();
            MainMenu();
        }
    }
}