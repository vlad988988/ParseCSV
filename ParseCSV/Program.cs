using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ParseCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] text = File.ReadAllLines(args[0], Encoding.Default);//принемаем адресс файла и записываем текст в массив text

            foreach (string str in text)//прогоняем строки в text
            {

                string modifiedStr = PrepareStr(str);//подготавливаем строку для деления

                List<string> data = new List<string>();//создаем список для хранения подстрок
                string[] parts = modifiedStr.Split('\"');//делим строку по кавычкам
                data.AddRange(parts.Where((x, index) => index % 2 != 0));//добавляем в список нечетные подстроки, так как что они в кавычках 
                data.AddRange(parts.Where((x, index) => index % 2 == 0).SelectMany(x => x.Split(',')));//четные подстроки вне кавычках, их делим по запятой и добавляем список
                string result = string.Join(" | ", data.Where(x => !string.IsNullOrWhiteSpace(x)));//соединяем подстроки по " | "

                Console.WriteLine(result);//выводим готовую строку на консоль
            }
            Console.ReadKey();
        }

        private static string PrepareStr(string line)//создаем метод для обработки строк
        {
            if (!line.Contains(','))//проверяем есть ли запятая в строке
            {
                Console.WriteLine("Невозможно распарсить эту строку");//если запятой нет выводим сообщение в консоль
            }
            string src = line.Substring(1, line.Length - 2);//удаляем крайние кавычки

            //удаление лишних последовательных пробелов

            string pattern = @"\s+";//переменной присваиваем регулярное выражение: одна и более пробелов
            string target = " ";//переменной присваеваем пробел
            Regex regex = new Regex(pattern);//создаем объект типа Regex
            string result = regex.Replace(src, target);//меняем последовательные пробелы на один пробел

            //удаление лишних кавычек

            while (result.Contains("\"\""))//создаем цикл с условием: пока строка содержит два последовательных пробелов
            {
                result = result.Replace("\"\"", "\"");//заменяем пробелы на один пробел

            }

            string modifiedResult = result.Replace(",\"", "\"").Replace(", \"", "\"").Replace("\",", "\"").Replace("\" ,", "\"");//удаляем крайние запятые для успешного деления по кавычкам
            return modifiedResult;//возвращяем результат
        }
    }
}
