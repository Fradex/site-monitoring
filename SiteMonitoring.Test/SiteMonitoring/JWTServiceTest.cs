using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog.Fluent;
using SiteMonitoring.Services;

namespace SiteMonitoring.Test.SiteMonitoring
{
    [TestClass]
    public class JWTServiceTest
    {
        public const string Login = "Login";
        public const string Password = "Password";
        public Guid UserId = Guid.NewGuid();

        [TestMethod]
        public void CheckTokenGeneration()
        {
            var token = JWTService.GenerateUserToken(Login, Password, UserId.ToString());

            Assert.IsNotNull(token);
            Assert.IsTrue(token.Length > 0);
        }

        [TestMethod]
        public void CheckToken()
        {
            var token = JWTService.GenerateUserToken(Login, Password, UserId.ToString());

            var obj = JWTService.DecodeUserToken(token);

            Assert.AreEqual(obj.Login , Login);
            Assert.AreEqual(obj.Password , Password);
            Assert.AreEqual(obj.Userid , this.UserId.ToString());
        }
    }
}
