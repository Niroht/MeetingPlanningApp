using DataAccess.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Common.Model;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using DataAccess.Azure;

namespace DataAccess
{
    public class MeetingProvider : IMeetingProvider
    {
        private CloudStorageAccount _storageAccount;
        CloudTableClient _tableClient;

        public MeetingProvider()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            _tableClient = _storageAccount.CreateCloudTableClient();
        }

        public Task<IEnumerable<Meeting>> GetMeetings(DateTime startDate, DateTime endDate)
        {
            var table = _tableClient.GetTableReference("meetings");
            table.CreateIfNotExists();
            var query = new TableQuery<MeetingEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "meeting"));

            var results = table.ExecuteQuery(query).Select(x => x.ToMeeting());
            var filteredMeetings = results.Where(x => x.ScheduledTime.Date >= startDate.Date && x.EndTime.Date <= endDate.Date);

            return Task.FromResult(filteredMeetings);
        }
    }
}
