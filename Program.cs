using System;

namespace hw23
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Введите строковый разделитель:");
            var stringSeparator = Console.ReadLine();
            //var stringSeparator = "+-55";   // для теста

            Console.WriteLine("Введите Строку целиком:");
            var stringInput = Console.ReadLine();
            //var stringInput = "-55+-5547";  // для теста

            try
            {
                var myDoubleResult = GetDouble(stringInput, stringSeparator);

                Console.WriteLine($"Общий результат программы: {myDoubleResult} as double");
            }
            catch (Exception e)
            {
                Console.Write("Вычисление по параметрам невозможно! \r\n" + e.Message);
            }
            Console.ReadKey();
        }

        /// <summary>
        /// вычисляет double от введенного текста и разделителя
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static double GetDouble(string str, string sep)
        {
            bool valSepResult = ValidateSep(str, sep);
            bool valDigResult = ValidateDigits(str, sep);

            if (valSepResult && valDigResult)
            {

                bool isNegative = false;

                // определим отрицательное число
                if (str.Length > 0 && str[0].Equals('-') && !sep.StartsWith("-"))
                {
                    isNegative = true;
                    str = str.Substring(1, str.Length - 1);
                    
                }

                // обработаем плюс при записи положительного числа
                if (str.Length > 0 && str[0].Equals('+') && !sep.StartsWith("+"))
                {
                    str = str.Substring(1, str.Length - 1);
                }


                string mainStr = str;
                string subStr = "";
                // если разделитель присутствует, разделим основную и дробные части
                if (sep.Length > 0 &&  str.IndexOf(sep) != -1)
                {
                    mainStr = str.Substring(0, (str.IndexOf(sep)));
                    subStr = str.Substring(str.IndexOf(sep) + sep.Length, str.Length - str.IndexOf(sep) - sep.Length);
                }

                // найдем дабл от целой части
                double mainP = GetMainPart(mainStr);
                // найдем дабл от дробной части
                double subP = GetSubPart(subStr);
                // сложим части
                double interResult = mainP + subP;
                // обработаем отрицательное число
                if (isNegative)
                    interResult = 0 - interResult;

                return interResult;
            }
            else // выбросим исключение при невозможности вычисления результата
            {
                string msg = "";
                if (!valSepResult)
                    msg += $"\r\nРазделитель должен встречаться 1 раз или не встречаться вовсе. Разделитель не может начинаться со знака минус `-` или плюс `+`, " +
                        " т.к. в таком случае невозможно определить отрицательное число";

                if (!valDigResult)
                    msg += "\r\n В самой строке вне входящего разделителя допустимы лишь символы цифр или лидирующий знак плюса и минуса в единственном числе" + 
                        " (символы + и - должны встречаться вне разделителя не более одного раза)";
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// проверяет валидность строки  разделителя
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static bool ValidateSep(string str, string sep)
        {

            // больше 2 разделителей в тексте
            if (str.Split(sep).Length > 2)
            {
                return false;
            }
            // не может начинаться с минуса или плюса
            if (sep.StartsWith("-") || sep.StartsWith("+"))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// проверяет валидность строки без разделителей
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sep"></param>
        /// <returns>true если прошли проверку</returns>
        public static bool ValidateDigits(string str, string sep)
        {
            var body = (sep.Length == 0) ? str : str.Replace(sep, "");

            // символы + и - должны встречаться вне разделителя не более одного раза
            if (body.Split(new char[2] { '+', '-' }).Length > 2) 
            {                 
                return false;
            }

            body = body.Replace("+", "").Replace("-", "");

            // текст числа должен содержать только цифры
            foreach (var c in body)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// получаем число из символа цифры
        /// </summary>
        /// <param name="c">символ</param>
        /// <returns> дабл от символа или -1 если символ не цифра</returns>
        public static int GetDigitFromChar(char c)
        {            

            if (c.Equals('0'))
            {
                return 0;
            }
            if (c.Equals('1'))
            {
                return 1;
            }
            if (c.Equals('2'))
            {
                return 2;
            }
            if (c.Equals('3'))
            {
                return 3;
            }
            if (c.Equals('4'))
            {
                return 4;
            }
            if (c.Equals('5'))
            {
                return 5;
            }
            if (c.Equals('6'))
            {
                return 6;
            }
            if (c.Equals('7'))
            {
                return 7;
            }
            if (c.Equals('8'))
            {
                return 8;
            }
            if (c.Equals('9'))
            {
                return 9;
            }
            else
                return -1;
        }

        /// <summary>
        /// возвращает целое до разделителя из цифр
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double GetMainPart(string str)
        {
            // разряд 
            int depth = str.Length;
            double result = 0;
            foreach (var c in str)
            {
                if (depth > 0)
                    result += GetDigitFromChar(c) * Math.Pow(10, --depth);
                else result += GetDigitFromChar(c);
            }
            return result;
        }

        /// <summary>
        /// возвращает дробное после разделителя
        /// </summary>
        /// <param name="str">строковое представление из цифр</param>
        /// <returns></returns>
        public static double GetSubPart(string str)
        {
            // разряд
            int depth = 0;
            double result = 0;
            foreach (var c in str)
            {
                if (depth == 0)
                {
                    depth++;
                    result += GetDigitFromChar(c) * 0.1;
                }
                else
                {
                    depth++;
                    var x = GetDigitFromChar(c) * Math.Pow(0.1, depth);
                    // исключим артефакты с плавающей точкой
                    result = Math.Round(result += x, depth);
                    
                }
            }
            return result;
        }
    }
}