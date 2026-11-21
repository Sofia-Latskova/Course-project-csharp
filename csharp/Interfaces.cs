namespace Firewatch
{
    // Контракт: умею снимать показание датчика
    public interface Scannable
    {
        double ReadValue();
        string SensorSerial { get; }
    }

    // Контракт: умею сообщать о своей исправности
    public interface Diagnosable
    {
        bool IsOperational { get; }
        int HealthPercent { get; }
    }

    // Контракт: умею попасть в итоговый отчёт
    public interface Reportable
    {
        string ReportSection { get; }
        string ToReportLine();
    }

    // Объект поведения как подать сигнал тревоги
    public interface AlertPolicy
    {
        string AlertChannel { get; }
        string FormatAlert(ScanResult scan);
    }
}