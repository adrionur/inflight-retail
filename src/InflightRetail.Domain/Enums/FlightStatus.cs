namespace InflightRetail.Domain.Enums;

public enum FlightStatus
{
    Scheduled = 1,  // Planlandı
    Boarding = 2,    // Kabin yüklemesi / Hazırlık
    Airborne = 3,    // Havada (Satış aktif)
    Completed = 4,   // İndi / Satış kapandı
    Reconciled = 5   // Stok mutabakatı tamamlandı
}