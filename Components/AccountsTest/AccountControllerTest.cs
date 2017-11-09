using Accounts;
using DatabaseSupport;
using Microsoft.AspNetCore.Mvc;
using TestSupport;
using Xunit;

namespace AccountsTest
{
    [Collection("Accounts")]
    public class AccountControllerTest
    {
        private readonly DataSourceConfig _dataSourceConfig = new DataSourceConfig();
        private readonly TestDatabaseSupport _support = new TestDatabaseSupport();

        static AccountControllerTest() => TestEnvSupport.SetRegistrationVcap();
        public AccountControllerTest() => _support.TruncateAllTables();

        [Fact]
        public void TestGet()
        {
            _support.ExecSql("insert into users (id, name) values (4765, 'Jack'), (4766, 'Fred');");
            _support.ExecSql(@"insert into accounts (id, owner_id, name) 
            values (1673, 4765, 'Jack''s account'), (1674, 4766, 'Fred''s account');");

            var controller =
                new AccountController(new AccountDataGateway(new DatabaseTemplate(_dataSourceConfig)));
            var result = controller.Get(4765);

            var info = (AccountInfo) ((ObjectResult) result).Value;

            Assert.Equal(1673, info.Id);
            Assert.Equal(4765, info.OwnerId);
            Assert.Equal("Jack's account", info.Name);
            Assert.Equal("account info", info.Info);
        }
    }
}