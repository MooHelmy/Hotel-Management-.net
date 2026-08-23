namespace HotelManagement.Entities
{
    public enum RoomType
    {
        Single,
        Double,
        Twin,
        Deluxe,
        Suite,
        Family,
        Presidential
    }

    public enum RoomStatus
    {
        Available,
        Occupied,
        Reserved,
        UnderMaintenance,
        OutOfService
    }

    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        CheckedIn,
        CheckedOut,
        Cancelled,
        NoShow
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        PartiallyPaid,
        Refunded,
        Failed
    }

    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        Cash,
        BankTransfer,
        OnlineWallet
    }
}