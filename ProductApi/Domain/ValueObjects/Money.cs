﻿namespace ProductApi.Domain.ValueObjects;

public class Money
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public override string ToString()
    {
        return $"{Currency} {Amount}";
    }
}