using CalculatorService.Console.DTOs;
using CalculatorServices.Console.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalculatorService.Console
{
    class Program
    {
        private static string Host => Environment.GetEnvironmentVariable("API_HOST") ?? "localhost:5001";
        private static string UrlBase => $"http://{Host}/api/v1/calculator/";

        private static Regex validInputRegEx = new Regex("[^a-zA-Z0-9,]", RegexOptions.Compiled);

        static async Task Main(string[] args)
        {
            var continueOp = true;
            var justFirstTime = true;
            int? trackId = null;
            var request = default(object);

            while (continueOp)
            {
                if (justFirstTime)
                {
                    PrintInfo();
                    justFirstTime = false;
                }

                var trackIdText = trackId.HasValue ? $"(trackId = {trackId})" : "";
                Write($"operation {trackIdText} -> ");
                var op = ReadLine();

                switch (op.ToUpper())
                {
                    case "R":
                        WriteLine("Reset traking Id", ConsoleColor.Cyan);
                        trackId = null;

                        break;

                    case "U":
                        WriteLine("Using a traking Id", ConsoleColor.Cyan);
                        if (!TryInputInt("trackId", out int trackIdPased))
                            break;

                        if (trackIdPased <= 0)
                        {
                            WriteLine("The trackId should be positive");
                            break;
                        }
                        trackId = trackIdPased;

                        break;

                    case "D":
                        WriteLine("Divide", ConsoleColor.Cyan);
                        if (!TryInputInt("dividend", out int dividend))
                            break;

                        if (!TryInputInt("divisor", out int divisor))
                            break;

                        request = new DivRequest() { Dividend = dividend, Divisor = divisor };
                        await PostAndPrint("div", request, trackId);

                        break;

                    case "M":
                        WriteLine("Multiplication", ConsoleColor.Cyan);
                        if (!TryInputListInt(out List<int> toSend))
                            break;

                        request = new MultRequest() { Factors = toSend };
                        await PostAndPrint("mult", request, trackId);

                        break;

                    case "Q":
                        WriteLine("Square", ConsoleColor.Cyan);
                        if (!TryInputInt("number", out int number))
                            break;
                        request = new SqrtRequest() { Number = number };
                        await PostAndPrint("sqrt", request, trackId);

                        break;

                    case "S":
                        WriteLine("Substract", ConsoleColor.Cyan);
                        if (!TryInputInt("minuend", out int minuend))
                            break;

                        if (!TryInputInt("subtrahend", out int subtrahend))
                            break;

                        request = new SubRequest() { Minuend = minuend, Subtrahend = subtrahend };
                        await PostAndPrint("sub", request, trackId);

                        break;

                    case "A":
                        WriteLine("Sum", ConsoleColor.Cyan);
                        if (!TryInputListInt(out toSend))
                            break;

                        request = new SumRequest() { Addends = toSend };
                        await PostAndPrint("add", request, trackId);

                        break;

                    case "G":
                        WriteLine("Query", ConsoleColor.Cyan);
                        if (!TryInputInt("trackId", out int trackIdToSend))
                            break;

                        request = new QueryRequest() { Id = trackIdToSend };
                        await PostAndPrint("query", request, trackId);

                        break;

                    case "O":
                        WriteLine("Operations", ConsoleColor.Cyan);
                        await GetAndPrint("operations");

                        break;

                    case "E":
                        continueOp = false;

                        break;

                    case "I":
                        PrintInfo();

                        break;

                    default:
                        WriteLine("Invalid operation!", ConsoleColor.Red);

                        break;
                }
            }

            WriteLine("Bye :)");
        }

        private static void PrintInfo()
        {
            var padRight = 6;
            WriteLine(@$"Type a letter to perform a operation:");
            WriteLine(@$" {"D".PadRight(padRight, '.')} Divide");
            WriteLine(@$" {"M".PadRight(padRight, '.')} Multiplication");
            WriteLine(@$" {"Q".PadRight(padRight, '.')} Square");
            WriteLine(@$" {"S".PadRight(padRight, '.')} Substract");
            WriteLine(@$" {"A".PadRight(padRight, '.')} Sum");
            WriteLine(@$" {"G".PadRight(padRight, '.')} Query");
            WriteLine(@$" {"O".PadRight(padRight, '.')} Get all operations");
            WriteLine(@$" {"U".PadRight(padRight, '.')} Using a traking Id");
            WriteLine(@$" {"R".PadRight(padRight, '.')} Reset traking Id");
            WriteLine(@$" {"I".PadRight(padRight, '.')} This operation list");
            WriteLine(@$" {"E".PadRight(padRight, '.')} Exit");
        }

        private static bool TryInputListInt(out List<int> list)
        {
            list = default;
            try
            {
                Write($"Input a list of integer (ie 1,2,3,..): ");
                var data = ReadLine();
                list = data.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                 .Select(int.Parse)
                                 .Where(d => d > 0)
                                 .ToList();

                if (list.Count() == 0)
                {
                    WriteLine($"No data to send");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                WriteLine($"Invalid data!", ConsoleColor.Red);
            }
            return false;
        }

        private static bool TryInputInt(string name, out int data)
        {
            Write($"Input {name}: ");
            var strData = ReadLine();
            if (!int.TryParse(strData, out data))
            {
                WriteLine($"Incorrect data! need input a valid integer!", ConsoleColor.Red);
                return false;
            }
            else
            {
                return true;
            }
        }

        private static async Task PostAndPrint(string method, object request, int? trackId)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request, Formatting.Indented);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                using var client = new HttpClient();

                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{UrlBase}{method}"),
                    Content = data
                };
                httpRequestMessage.Headers.Add(HttpRequestHeader.Accept.ToString(), "application/json");
                if (trackId.HasValue && trackId.Value > 0)
                {
                    httpRequestMessage.Headers.TryAddWithoutValidation("X-Evi-Tracking-Id", trackId.Value.ToString());
                }
                WriteLine($"Operation: {method}", ConsoleColor.Yellow);
                WriteLine("Request:", ConsoleColor.Yellow);
                if (trackId.HasValue && trackId.Value > 0)
                {
                    WriteLine($@"Header X-Evi-Tracking-Id = {trackId.Value}", ConsoleColor.Yellow);
                }
                WriteLine(json, ConsoleColor.Yellow);

                WriteLine("Sending...", ConsoleColor.Yellow);

                var response = await client.SendAsync(httpRequestMessage);
                var result = response.Content.ReadAsStringAsync().Result;
                var responseFormatted = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(result), Formatting.Indented);

                WriteLine("Response:", ConsoleColor.Green);
                WriteLine(responseFormatted, ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                WriteLine("Exception!", ConsoleColor.Red);
                WriteLine(ex.Message, ConsoleColor.Red);
            }
        }

        private static async Task GetAndPrint(string method)
        {
            try
            {
                using var client = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{UrlBase}{method}")
                };
                httpRequestMessage.Headers.Add(HttpRequestHeader.Accept.ToString(), "application/json");

                WriteLine($"Method: {method}", ConsoleColor.Yellow);
                WriteLine("Sending...", ConsoleColor.Yellow);

                var response = await client.SendAsync(httpRequestMessage);
                var result = response.Content.ReadAsStringAsync().Result;
                var responseFormatted = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(result), Formatting.Indented);

                WriteLine("Response:", ConsoleColor.Green);
                WriteLine(responseFormatted, ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                WriteLine("Exception!", ConsoleColor.Red);
                WriteLine(ex.Message, ConsoleColor.Red);
            }
        }

        static void Write(string text, ConsoleColor consoleColor = ConsoleColor.White)
        {
            System.Console.ForegroundColor = consoleColor;
            System.Console.Write(text);
        }

        static void WriteLine(string text, ConsoleColor consoleColor = ConsoleColor.White)
        {
            System.Console.ForegroundColor = consoleColor;
            System.Console.WriteLine(text);
        }

        static string ReadLine()
        {
            var line = System.Console.ReadLine();
            return validInputRegEx.Replace(line, "");
        }
    }
}
