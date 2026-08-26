using HotelManagement.DTOs;
using HotelManagement.Entities;
using Microsoft.EntityFrameworkCore;

public class PaymentRepository(ApplicationDbContext Context) : IPaymentService
{
    public async Task<ServicesResponse<PaymentDTO>> GetByReservationAsync(int reservationId)
    {
        var payment = Context.Payments.FirstOrDefault(p => p.ReservationId == reservationId);
        if (payment == null)
        {
            return new ServicesResponse<PaymentDTO>(false, "Payment not found", null);
        }
        return new ServicesResponse<PaymentDTO>(true, "Payment found", payment.PaymentToDtoMapper());
    }

    public async Task<ServicesResponse<PaymentDTO>> PayAsync(int reservationId, decimal amount, PaymentMethod method, string? transactionReference)
    {
        // 1) نتأكد إن الـ Reservation اللي بندفع ليه موجود أصلاً في الداتابيز
        var reservationExists = await Context.Reservations
            .AnyAsync(r => r.Id == reservationId);

        if (!reservationExists)
        {
            return new ServicesResponse<PaymentDTO>(false, "Reservation not found", null);
        }

        // 2) نتأكد إن الـ Reservation ده مالوش Payment سابق بحالة "مدفوع بالكامل"
        //    (يعني منسمحش بدفع مرتين لنفس الحجز)
        var existingPayment = await Context.Payments
            .FirstOrDefaultAsync(p => p.ReservationId == reservationId);

        if (existingPayment != null && existingPayment.Status == PaymentStatus.Paid)
        {
            return new ServicesResponse<PaymentDTO>(false, "This reservation is already fully paid", null);
        }

        Payment payment;

        if (existingPayment != null)
        {
            // 3أ) لو فيه Payment قديم بحالة Pending أو Failed أو PartiallyPaid → 
            // نحدّثه بدل ما نعمل واحد جديد
            existingPayment.Amount = amount;
            existingPayment.PaymentMethod = method;
            existingPayment.TransactionReference = transactionReference;
            existingPayment.Status = PaymentStatus.Paid;
            existingPayment.PaidAt = DateTime.UtcNow;

            payment = existingPayment;
        }
        else
        {
            // 3ب) مفيش Payment قبل كده → نعمل واحد جديد من الصفر
            payment = new Payment
            {
                ReservationId = reservationId,
                Amount = amount,
                PaymentMethod = method,
                TransactionReference = transactionReference,
                Status = PaymentStatus.Paid,
                PaidAt = DateTime.UtcNow
            };

            Context.Payments.Add(payment);
        }

        // 4) نحفظ التغييرات في الداتابيز (سواء إضافة جديدة أو تحديث)
        await Context.SaveChangesAsync();

        // 5) نرجع الـ Response بشكل DTO مش Entity (مبدأ مهم: الكونترولر ميشوفش الـ Entity الأصلي)
        return new ServicesResponse<PaymentDTO>(true, "Payment completed successfully", payment.PaymentToDtoMapper());
    }

    public async Task<ServicesResponse> RefundAsync(int reservationId)
    {
        // 1) نجيب الـ Payment المرتبط بالـ Reservation ده
        var payment = await Context.Payments
            .FirstOrDefaultAsync(p => p.ReservationId == reservationId);

        // 2) لو مفيش Payment أصلاً، مفيش حاجة نرجّعها فلوسها
        if (payment == null)
        {
            return new ServicesResponse(false, "Payment not found for this reservation");
        }

        // 3) لو الحالة مش "Paid" أو "PartiallyPaid"، معناها إما لسه معملش دفع أصلاً، أو اتعمله refund قبل كده
        if (payment.Status != PaymentStatus.Paid && payment.Status != PaymentStatus.PartiallyPaid)
        {
            return new ServicesResponse(false, $"Cannot refund a payment with status '{payment.Status}'");
        }

        // 4) نغيّر الحالة لـ Refunded (نحن مش بنمسح الـ Payment، بنسجل إنه اترجع بس)
        payment.Status = PaymentStatus.Refunded;

        // 5) نحفظ التغيير في الداتابيز
        await Context.SaveChangesAsync();

        return new ServicesResponse(true, "Payment refunded successfully");
    }
}