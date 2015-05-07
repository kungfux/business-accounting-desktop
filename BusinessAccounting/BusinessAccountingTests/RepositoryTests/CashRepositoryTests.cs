using BusinessAccounting.Model;
using BusinessAccounting.Repositories;
using NUnit.Framework;
using System;
using System.IO;

namespace BusinessAccountingTests.RepositoryTests
{
    [TestFixture]
    public class CashRepositoryTests
    {
        [Test]
        public void AddCash()
        {
            var cashRepo = new TransactionRepository();

            var cash = new Transaction() { Date = DateTime.Now, Sum = 1.0m, Comment = "first comment" };
            cashRepo.Add(cash);
            Assert.AreEqual(cash, cashRepo.GetById(1));
        }
    }
}
