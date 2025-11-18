using System;
using Ardalis.Specification;
using Arkano.Transactions.Domain.Entities;

namespace Arkano.Transactions.Infraestructure.Specifications
{
    public class TransactionByExternalIdSpec : Specification<Transaction>
    {
        public TransactionByExternalIdSpec(Guid externalId)
        {
            Query.Where(transaction => transaction.TransactionExternalId == externalId)
                 .Take(1);
        }
    }
}