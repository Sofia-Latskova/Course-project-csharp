using System;
using System.Collections.Generic;

using SensorList = System.Collections.Generic.List<Firewatch.BaseSensor>;//псевдоним типа

namespace Firewatch
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("========== Lab-5 Полиморфизм (C#) ==========\n");
            var drone = new Quadcopter("DRN-001", 80, 100, "BAT-001");
            var temp = new TempSensor("TS-001", drone, 45.0);
            var smoke = new SmokeSensor("SS-001", drone, 0.2);
            var flame = new FlameSensor("FS-001", drone, 10);

            //работа через интерфейсы
            Console.WriteLine("\n===== работа через интерфейсы =====");
            var diag = new List<IDiagnosable> { temp, drone };
            foreach (var u in diag)
                Console.WriteLine($"  Health = {u.HealthPercent}% {(u.IsOperational ? "[OK]" : "[FAIL]")}");

            var report = new List<IReportable> { temp, smoke, flame, drone };
            foreach (var item in report)
                Console.WriteLine($"  [{item.ReportSection}] {item.ToReportLine()}");

            //раннее/позднее
            Console.WriteLine("\n===== раннее/позднее =====");
            BaseSensor s = new TempSensor("TS-002", drone, 45.0);
            Console.WriteLine($"  GetKind()     (невиртуальный, раннее):  {s.GetKind()}");
            Console.WriteLine($"  GetTypeName() (виртуальный, позднее):   {s.GetTypeName()}");

            //dynamic_cast
            Console.WriteLine("\n===== dynamic_cast =====");
            var units = new List<IDiagnosable> { temp, smoke, flame, drone };
            foreach (var u in units)
            {
                if (u is IScannable sc)
                {
                    try { Console.WriteLine($"  [OK]   {sc.SensorSerial} -> {sc.ReadValue()}"); }
                    catch (SensorFailure e) { Console.WriteLine($"  [FAIL] {e.Message}"); }
                }
                else Console.WriteLine("  [SKIP] объект не реализует IScannable");
            }

            //конфигурирование
            Console.WriteLine("\n===== конфигурирование AlertPolicy =====");
            var task = new FlightTask("ZONE-A", drone, new SirenAlert());
            var scan = new ScanResult { HasReliableData = true };
            scan.Readings.Add(new ScanResult.Reading { Serial = "TS-001", Kind = "температура", Value = 55.0, Reliable = true });
            scan.Readings.Add(new ScanResult.Reading { Serial = "SS-001", Kind = "дым", Value = 0.7, Reliable = true });
            scan.Readings.Add(new ScanResult.Reading { Serial = "FS-001", Kind = "пламя", Value = 45.0, Reliable = true });

            void PrintAlert(string label)
            {
                var (ch, msg) = task.TriggerAlert(scan);
                Console.WriteLine($"  {label} -> [{ch}] {msg}");
            }

            PrintAlert("Сирена       ");
            task.SetPolicy(new SilentLogAlert());
            PrintAlert("Тихий лог    ");
            task.SetPolicy(new EmercomCallAlert());
            PrintAlert("Вызов МЧС    ");

            //исключения
            Console.WriteLine("\n====исключения =====");

            Console.WriteLine("\n Успешный прогон:");
            var drone2 = new Quadcopter("DRN-002", 80, 100, "BAT-002");
            var ok = new FlightTask("ZONE-OK", drone2, new SirenAlert());
            try
            {
                ok.Start();
                drone2.Scan();
                drone2.Scan();
                ok.Complete();
            }
            catch (Exception e) { Console.WriteLine($"  Неожиданная ошибка: {e.Message}"); }

            Console.WriteLine("\n Низкий заряд при взлёте:");
            var lowBat = new Quadcopter("DRN-LOW", 5, 100, "BAT-LOW");
            var bad = new FlightTask("ZONE-LOW", lowBat, new SirenAlert());
            try
            {
                bad.Start();
            }
            catch (LowBatteryException e) { Console.WriteLine($"  [LowBatteryException] {e.Message}"); }
            catch (Exception e) { Console.WriteLine($"  [Exception] {e.Message}"); }

            Console.WriteLine("\n Сканирование не в полёте:"); 
            var drone3 = new Quadcopter("DRN-003", 80, 100, "BAT-003");
            try
            {
                drone3.Scan();
            }
            catch (InvalidMissionStateException e) { Console.WriteLine($"  [InvalidMissionStateException] {e.Message}"); }
            catch (Exception e) { Console.WriteLine($"  [Exception] {e.Message}"); }

            Console.WriteLine("\n Прервать неначатое задание:");
            var drone4 = new Quadcopter("DRN-004", 80, 100, "BAT-004");
            var t = new FlightTask("ZONE-4", drone4, new SirenAlert());
            try
            {
                t.Abort("тест");
            }
            catch (InvalidMissionStateException e) { Console.WriteLine($"  [InvalidMissionStateException] {e.Message}"); }
            catch (Exception e) { Console.WriteLine($"  [Exception] {e.Message}"); }

            Console.WriteLine("\n Неисправный датчик:");
            var broken = new TempSensor("TS-BROKEN", drone, 45.0);
            try
            {
                
                broken.Break();
                broken.ReadValue();
            }
            catch (SensorFailure e) { Console.WriteLine($"  [SensorFailure] {e.Message}"); }
            catch (Exception e) { Console.WriteLine($"  [Exception] {e.Message}"); }

            Console.WriteLine("\n Порядок catch (частный -> общий):"); 
            var lowBatOrder = new Quadcopter("DRN-X", 5, 100, "BAT-X");
            try
            {
               
                lowBatOrder.TakeOff();
            }
            catch (SensorFailure e) { Console.WriteLine($"  SensorFailure: {e.Message}"); }
            catch (LowBatteryException e) { Console.WriteLine($"  LowBatteryException: {e.Message}"); }
            catch (BatteryOverDischargeException e) { Console.WriteLine($"  BatteryOverDischargeException: {e.Message}"); }
            catch (InvalidMissionStateException e) { Console.WriteLine($"  InvalidMissionStateException: {e.Message}"); }
            catch (FirewatchException e) { Console.WriteLine($"  FirewatchException: {e.Message}"); }
            catch (Exception e) { Console.WriteLine($"  Exception: {e.Message}"); }
            Console.WriteLine("\n=== КОНЕЦ ===");

        }
    }
}
















