﻿using CommonLibrary.Repositories;
using Discount.gRPC.Repositories.Context;
using Discount.gRPC.Repositories.Interfaces;

namespace Discount.gRPC.Repositories
{
    public class DiscountRepository(DiscountDbContext context) : GenericRepository<DiscountDbContext, Models.Discount, int>(context), IDiscountRepository
    {
    }
}
