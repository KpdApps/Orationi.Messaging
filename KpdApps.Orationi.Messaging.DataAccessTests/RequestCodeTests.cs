using KpdApps.Orationi.Messaging.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KpdApps.Orationi.Messaging.DataAccessTests
{
    [TestClass]
    public class RequestCodeTests : BaseDataAccessTest
    {
        [TestMethod]
        public void InsertRequestCode()
        {
            RequestCode requestCode = new RequestCode();
            requestCode.RequestCodeId = new Random().Next(800000, 900000);
            requestCode.Name = "Request Code Test";
            requestCode.Description = "Test";

            try
            {
                DbContext.Add(requestCode);
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                DbContext.Remove(requestCode);
                DbContext.SaveChanges();
            }
        }
    }
}
