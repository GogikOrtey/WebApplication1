using System.Diagnostics;
using System.Security.Principal;
using System.Management;
using System.Globalization;            // Если этот using не работает, установите в проект пакет "System.Management", используя NuGet

namespace WebApplication1
{
    public static class ScanDiscC
    {
        //------------------------------------------------------//
        //              Основные публичные методы               //
        //------------------------------------------------------//

        // Возвращает буферные значения структуры диска С, если они достаточно корректны.
        // Если некорректны - то запускает новое сканирование
        // Выполнение занимает <1 секунды        
        //
        // Эта процедура по умолчанию основная 
        public static string GetBufer_StructDiskC()
        {
            // Если буферная структура папок существует и мы считаем её корректной
            if (isBuferDataExists_andCorrect() == true)
            {
                // Возвращаем буферные значения
                return MainGetBuferData();
            }
            else
            {
                // Получаем структуру папок заново
                return GetCurrent_StructDiskC();
            }
        }

        // Получение структуры папок с диска С - запуск сканирования, и вывод результатов из него.
        // Сканирование занимает 2-5 минут времени
        public static string GetCurrent_StructDiskC()
        {
            return MainMetod();
        }

        // Тестовый метод
        public static string TestAssecc_returnString()
        {
            return "NNnnn\nNNnn\nNnnnN";
        }







        //------------------------------------------------------//
        //     Методы, для обработки вывода буферного файла     //
        //------------------------------------------------------//

        //
        // Главная процедура возврата буферных значений
        //

        static string MainGetBuferData()
        {
            string result = "";

            // Загружаем текстовый файл
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(directoryPath, "Out_1.txt");
            string fileContent = File.ReadAllText(filePath);

            // Добавляем немного информации
            fileContent = "📒 Вывод данных из буфера\n" + fileContent;
            fileContent = fileContent.Replace("Текущая дата и время:", "Дата и время последнего сканирования:");

            result = fileContent;
            return result;
        }

        //
        // Главная процедура проверки
        //

        // Проверяет, существует ли буферный файл структуры диска
        // Не старый ли он, а также, сделан ли он на этом-же компьютере
        //
        // Возвращает false, если нужно заново просканировать весь диск С
        static bool isBuferDataExists_andCorrect()
        {
            //
            // Проверяем, существует ли буферный файл
            //

            // Путь к текстовому файлу
            string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(directoryPath, "Out_1.txt");

            if (!(File.Exists(filePath)))
            {
                //Console.WriteLine("Файл не найден");
                return false;
            }

            //
            // Проверяем, что программа запускается на том-же компьютере, на котором запускалась раньше
            //

            string currentCPUid = getCurrentPSid();

            if (currentCPUid == "-") return false;  // Если значение текущего id процессора получено некорректно

            string savedCPUidValueFromTxtFile = getSavedCPUidValue();

            // Если текущее значение id процессора, не совпадает с сохранённым (значит программа запускается на другом компьютере)
            if (currentCPUid != savedCPUidValueFromTxtFile) return false;


            //
            // Проверяем время создание буферного файла
            //
            
            // Если с момента записи файла прошло больше 1 дня, то данные уже считаются устаревшими

            string fileContent = File.ReadAllText(filePath);

            // Поиск строки с датой и временем
            string[] lines = fileContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            string dateTimeString = string.Empty;
            foreach (var line in lines)
            {
                if (line.Contains("Текущая дата и время:"))
                {
                    dateTimeString = line.Split(':')[1].Trim() + ":" + line.Split(':')[2].Trim() + ":" + line.Split(':')[3].Trim();
                    break;
                }
            }

            if (!string.IsNullOrEmpty(dateTimeString))
            {
                DateTime fileDateTime = DateTime.ParseExact(dateTimeString, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                // Текущая дата и время
                DateTime currentDateTime = DateTime.Now;

                // Сравнение разницы во времени
                TimeSpan difference = currentDateTime - fileDateTime;

                //print(difference);

                // Если разница больше 1 дня
                if (difference.TotalDays > 1)
                {
                    //Console.WriteLine("Разница больше 1 дня");

                    return false;
                }
            }
            else
            {
                // При ошибке чтения даты из файла
                return false;
            }

            // Во всех остальных случаях
            return true; // Корректно. Можем возвращать кешированную структуру диска
        }

        // Возвращает id процессора текущего компьютера
        static string getCurrentPSid()
        {
            string cpuId = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select ProcessorId from Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                cpuId = obj["ProcessorId"].ToString();
                break;
            }

            if (cpuId == null) cpuId = "-";

            return cpuId;
        }

        // Возвращает сохранённый id процессора компьютера
        static string getSavedCPUidValue()
        {
            string filePath = "SavedCPUidValue.txt";
            string cpuIdValue = "";

            using (StreamReader reader = new StreamReader(filePath))
            {
                cpuIdValue = reader.ReadToEnd();
            }

            return cpuIdValue;
        }

        // Сохраняет текущее id процессора компьютера в текстовый файл
        // Сохранённое текстовое значение нужно для дальнейшей проверки, и используется в процедуре isBuferDataExists_andCorrect
        static void WriteCurrentCPUidValue_FromTxtFile()
        {
            string filePath = "SavedCPUidValue.txt";
            string cpuId = getCurrentPSid();

            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.Write(cpuId);
            }
        }









        //------------------------------------------------------//
        //     Основной код сканирования структуры диска С      //
        //------------------------------------------------------//



        //
        // Главный метод запуска сканирования
        //

        // Возвращает строку с переносами типа \n, со всей структурой диска С
        // Сверху и снизу также добавляются строки описания и статистики
        static string MainMetod()
        {
            Stopwatch stopwatch = new Stopwatch();  
            stopwatch.Start();                      // Запускаем таймер

            UndoStatsPrint();                       // Что выводит перед структурой папок
            
            RecurseDisplFoldersFromDiskC();         // Основная процедура рекурсивного поиска вложенных папок, и вывода их названий в консоль

            stopwatch.Stop();                       // Останавливаем таймер

            PrintStats();                           // Печатает статистику в конце вывода

            // Выводит время выполнения
            if (stopwatch.ElapsedMilliseconds / 1000 > 120)
            {
                // Выводим время выполнения в минутах
                print("Время выполнения: " + Math.Round((double)(stopwatch.ElapsedMilliseconds / 1000 / 60), 2) + " минут");
            }
            else
            {
                // Выводим время выполнения в секундах
                print("Время выполнения: " + stopwatch.ElapsedMilliseconds / 1000 + " секунд");
            }

            SaveTextToFile();                       // Сохраняет полученную структуру папок - в текстовый файл
            WriteCurrentCPUidValue_FromTxtFile();   // Сохраняет id текущего компьютера, в другой текстовый файл

            return (OutTextToTxtFiles);             // Возвращает строку со всей структурой диска С
        }





        //
        // Настраиваемые параметры:
        //

        // Максимальное количество вложенных папок, которое будет выведено
        // Если = 0, то без ограничения
        static int maxCountRecurse = 0;                 ///// Для тестирования алгоритма сканирования, рекомендую установить уровень вложенности папок на 1 или 2

        static bool printLvlId = false;                 // Печатать ли номер уровня вложенной папки?
        static bool printAccessReadFolderError = true;  // Печатать ли предупреждения, когда папка недоступна для чтения?
        static bool whyPrintSpaseLvl = true;            // Выводить пробелы как уровни для папок? (если = false), то они будут выводится со спецсимволами типо └──


        public static string OutTextToTxtFiles = "";    // Весь текст вывода
        // После выполнения сканирования, он сохраняется в тестовый буферный файл Out_1.txt который лежит в корне проекта,
        // и возвращается как ответ на запрос сканирования структуры диска С


        // Статистика:

        static int AllShowStrings = 0;                  // Всего строк было выведено
        static int MaxLvlFromRecurse = 0;               // Максимальный уровень рекурсии (кол-ва вложенных папок друг в друга)



        // Основная процедура рекурсивного поиска вложенных папок, и вывода их названий в консоль
        static void RecurseDisplFoldersFromDiskC(string rootPath = @"C:\", int currRecCount = 0)
        {
            try
            {
                // Получаем массив имен всех каталогов в корне диска С
                string[] directories = Directory.GetDirectories(rootPath);

                if (currRecCount > MaxLvlFromRecurse) MaxLvlFromRecurse = currRecCount;

                // Выводим названия всех каталогов
                foreach (string directory in directories)
                {
                    string currDirectory = Path.GetFileName(directory);

                    string spaseLvl = "";
                    for (int i = 0; i < currRecCount * 2; i++)
                    {
                        if (whyPrintSpaseLvl == true)
                        {
                            spaseLvl += " ";
                        }
                        else
                        {
                            if (i == 0) spaseLvl = "└";
                            else spaseLvl += "─";
                        }
                    }

                    if (printLvlId)
                        print_adjacent("lvl = " + currRecCount + " : ");

                    print(spaseLvl + currDirectory);


                    if ((maxCountRecurse > 0 && currRecCount < maxCountRecurse)         // Если количество рекурсий в допустимом диапазоне
                        || (maxCountRecurse == 0))                                      // Или если нет ограничение на количество рекурсий
                    {
                        string newRootPath = Path.Combine(rootPath, currDirectory);     // Построение нового пути
                        RecurseDisplFoldersFromDiskC(newRootPath, currRecCount + 1);    // Рекурсивный вызов с новыми аргументами
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Игнорируем папки, к которым нет доступа
                if (printAccessReadFolderError)
                    print($"🛑 Нет доступа к папке: {rootPath}");
            }
        }

        // Это выводится перед основной процедурой
        static void UndoStatsPrint()
        {
            // Выводим текущую время и дату
            print("🕓 Текущая дата и время: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "");

            // Получаем текущее имя пользователя
            string username = Environment.UserName;

            // Получаем права текущего пользователя
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            string permissions = principal.IsInRole(WindowsBuiltInRole.Administrator) ? "Администратор" : "Обычный пользователь";

            print("👤 Выполняем сканирование диска C от имени пользователя: " + username + ", его права: " + permissions + "");

            if (maxCountRecurse != 0)
            {
                print("⚠️ [Этот параметр отличается от значения по умолчанию] Ограничение рекурсии: Максимум " + maxCountRecurse + " уровень" + "");
            }

            print("📘 Отступ в 2 пробела слева - это значит папка вложена в родительскую (которая отображена строкой выше)");

            print("👇 Все доступные папки и подпапки с диска С:");

            print("---");
        }

        // Выводит статистику по количеству строк и максимально глубокому уровню
        static void PrintStats()
        {
            print("---");
            print("Всего выведено строк: " + AllShowStrings);
            print("Максимальный уровень рекурсии: " + MaxLvlFromRecurse);
        }

        // Сохраняет все строки вывода в файл
        // Вызывается после завершения сканирования
        static void SaveTextToFile()
        {
            string text = OutTextToTxtFiles;
            string fileName = "Out_1.txt";
            //string fileName = @"\bin\Debug\net8.0\Out_1.txt";

            try
            {
                // Получаем путь к директории выполнения программы
                string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
                // Объединяем путь директории и имя файла
                string filePath = Path.Combine(directoryPath, fileName);

                //print("directoryPath = " + directoryPath + ", filePath = " + filePath);

                // Записываем текст в файл
                File.WriteAllText(filePath, text);
                print("Вывод успешно сохранен в файл, по пути: " + filePath);
            }
            catch (Exception ex)
            {
                print("Произошла ошибка при сохранении текста в файл: " + ex.Message);
            }
        }

        // Первая, текстовая процедура
        // Выводит все папки диска С, без вложенных
        static void DisplFoldersFromDiskC(string rootPath = @"C:\")
        {
            // Получаем массив имен всех каталогов в корне диска С
            string[] directories = Directory.GetDirectories(rootPath);

            // Выводим названия всех каталогов
            foreach (string directory in directories)
            {
                print(Path.GetFileName(directory));
            }
        }

        // Написал свои процедуры для вывода текста в консоль
        static void print(params object[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
                OutTextToTxtFiles += arg + "\n";    // Дополнительно весь текст, который выводится - буферизуется, и дальше записывается в текстовый файл
                AllShowStrings++;                   // Также считаются значения для статистики
            }
        }

        static void print_adjacent(params object[] args)
        {
            // Печать без переноса строки
            foreach (var arg in args)
            {
                Console.Write(arg);
                OutTextToTxtFiles += arg;
            }
        }
    }
}
