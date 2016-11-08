using System;

namespace Orvosi.Shared.Accounting
{
    public interface ICustomer
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string BillingEmail { get; set; }
    }
}