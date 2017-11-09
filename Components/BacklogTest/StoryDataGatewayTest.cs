using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Backlog;
using DatabaseSupport;
using TestSupport;
using Xunit;

namespace BacklogTest
{
    [Collection("Backlog")]
    public class StoryDataGatewayTest
    {
        private readonly DataSourceConfig _dataSourceConfig = new DataSourceConfig();
        private readonly TestDatabaseSupport _support = new TestDatabaseSupport();

        static StoryDataGatewayTest() => TestEnvSupport.SetBacklogVcap();
        public StoryDataGatewayTest() => _support.TruncateAllTables();

        [Fact]
        public void TestCreate()
        {
            var gateway = new StoryDataGateway(new DatabaseTemplate(_dataSourceConfig));
            gateway.Create(22, "aStory");

            // todo...
            var template = new DatabaseTemplate(_dataSourceConfig);
            var projectIds = template.Query("select project_id from stories", reader => reader.GetInt64(0),
                new List<DbParameter>());

            Assert.Equal(22, projectIds.First());
        }

        [Fact]
        public void TestFind()
        {
            _support.ExecSql("insert into stories (id, project_id, name) values (1346, 22, 'aStory');");

            var gateway = new StoryDataGateway(new DatabaseTemplate(_dataSourceConfig));
            var list = gateway.FindBy(22);

            // todo...
            var actual = list.First();
            Assert.Equal(1346, actual.Id);
            Assert.Equal(22, actual.ProjectId);
            Assert.Equal("aStory", actual.Name);
        }
    }
}