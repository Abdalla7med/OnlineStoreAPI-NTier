﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL;
public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

[Index("CustomerId", Name = "IX_Orders_CustomerId")]

public partial class Order
{
    [Key]
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public int Status { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual Customer Customer { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public override bool Equals(object obj)
    {
        Order right = new Order();
        if (obj is Order) {
            right = obj as Order;
        }

        return this.Status.Equals(right.Status);
    }
}