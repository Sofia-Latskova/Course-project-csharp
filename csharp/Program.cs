using System;
using System.Collections.Generic;

using SensorList = System.Collections.Generic.List<Firewatch.BaseSensor>;//псевдоним типа

namespace Firewatch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("========== Lab-4 Наследование (C#) ==========\n");
            Console.WriteLine("--- 0. Создание квадрокоптера-носителя ---");
            var quad = new Quadcopter("DRN-001", 100, 100, "BAT-001");
            Console.WriteLine();

            //наследники массив
            Console.WriteLine("--- 1. Создание массива наследников ---");
            SensorList sensors = new SensorList
            {
                new TempSensor("TEMP-1", quad, 25.5),
                new SmokeSensor("SMOKE-1", quad, 0.1),
                new FlameSensor("FLAME-1", quad, 15)
            };

            //полиморфизм
            Console.WriteLine("\n--- 2. Полиморфизм ---");
            foreach (var sensor in sensors)
            {
                sensor.ReadValue();
                Console.WriteLine(sensor.Describe());
                Console.WriteLine("-------------------------");
            }

            //Копирование
            Console.WriteLine("\n--- 3. Поверхностное и глубокое копирование ---");
            var original = new TempSensor("TEMP-ORIG", quad, 20.0);
            original.Quadcopter.TakeOff();
            original.ReadValue();

            //поверхностная
            Console.WriteLine("\nПоверхнстная:");
            Console.WriteLine("Оригинал до изменений:\n" + original.Describe());
            var shallow = original.ShallowCopy();
            shallow.Quadcopter.Land();           
            Console.WriteLine("\nОригинал после изменения дрона через shallow-копию:\n" + original.Describe());

            //глубокая
            Console.WriteLine("\nГлубокая:");
            original.Quadcopter.TakeOff(); // Снова в полет
            Console.WriteLine("Оригинал до изменений:\n" + original.Describe());
            var deep = original.Clone();  
            deep.Quadcopter.Land();       // Меняем свой дрон
            Console.WriteLine("\nОригинал после изменения дрона через deep-копию:\n" + original.Describe());

            //сравнение
            Console.WriteLine("\n--- 4. Сравнение по содержимому ---");
            var s1 = new TempSensor("TEST-123", quad, 10);
            var s2 = new TempSensor("TEST-123", quad, 99); // Другая температура, но тот же номер
            Console.WriteLine($"s1.Equals(s2): {s1.Equals(s2)} (должно быть True, серийники совпадают)");
            Console.WriteLine($"s1.GetHashCode() == s2.GetHashCode(): {s1.GetHashCode() == s2.GetHashCode()}");
            var hashSet = new HashSet<BaseSensor> { s1, s2 };
            Console.WriteLine($"Количество элементов в HashSet: {hashSet.Count} (должно быть 1)");

            //Стат.счетчик
            Console.WriteLine("\n--- 5. Статический счётчик ---");
            foreach (var s in sensors)
            {
                Console.WriteLine($"ID: {s.Id} | Тип: {s.GetType().Name}");
            }
            Console.WriteLine($"Последний выданный ID: {BaseSensor.NextSensorId() - 1}");

            //this
            Console.WriteLine("\n--- 6. Использование this ---");
            var selfCopy = new TempSensor("TEMP-SELF", quad, 15);
            Console.WriteLine($"Сравнение объекта с самим собой (this == o): {selfCopy.Equals(selfCopy)}");
            Console.WriteLine("\n=== КОНЕЦ ===");

        }
    }
}
















