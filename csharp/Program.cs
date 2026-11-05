using System;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;

namespace Firewatch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("========== Лабораторная №3 (C#) ==========\n");
            Console.WriteLine("--- облёт зоны ---\n");
// Создаем дрон
            Quadcopter drone = new Quadcopter("DRN-001", 22, 100, "BAT-001");
            Console.WriteLine($"Дрон: {drone}");
            Console.WriteLine($"Заряд через дрон: {drone.BatteryCharge}%\n");
// Создаем задание
            FlightTask task = new FlightTask("Зона-А", drone);
            Console.WriteLine($"{task}\n");

// Корректный запуск
            Console.WriteLine("--- Корректный запуск ---");
            if (task.Start())
            {
                Console.WriteLine($"Статус задания: {task.StatusStr}");
                Console.WriteLine($"Состояние дрона: {drone.StateStr}\n");
            }

// Сканирование
            Console.WriteLine("--- Сканирование ---");
            drone.Scan();
            drone.Scan();
            drone.Scan();
            Console.WriteLine($"Заряд: {drone.BatteryCharge}%");
            Console.WriteLine($"Состояние: {drone.StateStr}\n");

// Попытка нарушить правило
            Console.WriteLine("--- Попытка нарушить правило ---");
            if (!task.Start())
            {
                Console.WriteLine("Отказ: задание уже выполняется\n");
            }
            // Завершение
            Console.WriteLine("--- Завершение ---");
            task.Complete();
            Console.WriteLine($"Статус: {task.StatusStr}\n");
// Проверка заряд < емкости
            Console.WriteLine("--- Проверка правила: заряд не может быть больше ёмкости ---");
            Battery testBattery = new Battery(80, 100, "BAT-TEST");
            Console.WriteLine($"Изначальный заряд: {testBattery.ChargePercent}%");
            Console.WriteLine("Пытаемся зарядить на 50% (должно превысить ёмкость)...");
            testBattery.Charge(50);

            Console.WriteLine("---  Ссылки и копии ---\n");

            Battery first = new Battery(80, 100, "BAT-100");
            Battery second = first;
            Console.WriteLine($"first:  {first}");
            Console.WriteLine($"second: {second}");
            Console.WriteLine($"first == second? {ReferenceEquals(first, second)}\n");
            Console.WriteLine("Меняем заряд через 'second'...");
            second.Consume(30);
            Console.WriteLine($"first:  {first}"); 
            Console.WriteLine($"second: {second}\n");

            Console.WriteLine("--- Демонстрация структуры ---");
            Coordinates coordA = new Coordinates(53.35, 83.77); 
            Console.WriteLine($"coordA (до изменений): {coordA}");
            Coordinates coordB = coordA;
            coordB.Latitude = 55.75;
            coordB.Longitude = 37.61; 
            Console.WriteLine($"coordA (после изменений coordB): {coordA}"); 
            Console.WriteLine($"coordB: {coordB}\n");
            Console.WriteLine($"coordA == coordB? {Equals(coordA, coordB)}\n");

            Console.WriteLine("--- Освобождение ресурса ---\n");
            using (Battery sessionBattery = new Battery(60, 100, "BAT-SESSION"))
            {
                sessionBattery.Consume(10);
                Console.WriteLine("Работа внутри блока using...");
            }
            Console.WriteLine("Блок using завершён. Ресурс батареи освобождён.\n");

            Console.WriteLine("--- Сборщик мусора ---\n");
            WeakReference<Battery> watcher = CreateTemporaryBattery();
            Console.WriteLine("Убираем ссылки и вызываем GC.Collect()...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Console.WriteLine($"Объект жив после сборки мусора? {watcher.TryGetTarget(out _)}\n");
            Console.WriteLine("========== Конец программы ==========");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static WeakReference<Battery> CreateTemporaryBattery()
        {
            Battery temp = new Battery(50, 100, "BAT-TEMP");
            var watcher = new WeakReference<Battery>(temp);
            Console.WriteLine($"Объект жив сразу после создания? {watcher.TryGetTarget(out _)}");
            return watcher;

        }
    }
}
















