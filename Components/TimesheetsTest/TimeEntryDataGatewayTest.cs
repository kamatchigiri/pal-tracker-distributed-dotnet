using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DatabaseSupport;
using TestSupport;
using Timesheets;
using Xunit;

namespace TimesheetsTest
{
    [Collection("Timesheets")]
    public class TimeEntryDataGatewayTest
    {
        private readonly DataSourceConfig _dataSourceConfig = new DataSourceConfig();
        private readonly TestDatabaseSupport _support = new TestDatabaseSupport();

        static TimeEntryDataGatewayTest() => TestEnvSupport.SetTimesheetsVcap();
        public TimeEntryDataGatewayTest() => _support.TruncateAllTables();

        [Fact]
        public void TestCreate()
        {
            var gateway = new TimeEntryDataGateway(new DatabaseTemplate(_dataSourceConfig));
            gateway.Create(22, 12, DateTime.Now, 8);

            // todo...
            var template = new DatabaseTemplate(_dataSourceConfig);
            var projectIds = template.Query("select project_id from time_entries", reader => reader.GetInt64(0),
                new List<DbParameter>());

            Assert.Equal(22, projectIds.First());
        }

        [Fact]
        public void TestFind()
        {
            _support.ExecSql(@"insert into time_entries (id, project_id, user_id, date, hours) 
values (2346, 22, 12, now(), 8);");

            var gateway = new TimeEntryDataGateway(new DatabaseTemplate(_dataSourceConfig));
            var list = gateway.FindBy(12);

            // todo...
            var actual = list.First();
            Assert.Equal(2346, actual.Id);
            Assert.Equal(22, actual.ProjectId);
            Assert.Equal(12, actual.UserId);
        }
    }
}