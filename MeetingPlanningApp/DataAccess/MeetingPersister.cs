using DataAccess.Common.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Common.Model;
using GalaSoft.MvvmLight.Messaging;
using DataAccess.Messages;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using DataAccess.Azure;

namespace DataAccess
{
    public class MeetingPersister : IMeetingPersister
    {
        private readonly IMessenger _messenger;

        private CloudStorageAccount _storageAccount;
        CloudTableClient _tableClient;

        public MeetingPersister(IMessenger messenger)
        {
            _messenger = messenger;

            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            _tableClient = _storageAccount.CreateCloudTableClient();
        }

        public Task SaveMeeting(Meeting meeting)
        {
            var table = GetTable();
            var retrieveOperation = TableOperation.Retrieve("meeting", meeting.Id.ToString());

            var existingMeeting = table.Execute(retrieveOperation);

            TableOperation writeOperation;
            if (existingMeeting.Result != null)
            {
                var entity = meeting.ToMeetingEntity();
                entity.ETag = existingMeeting.Etag;
                writeOperation = TableOperation.Replace(entity);
                table.Execute(writeOperation);

                _messenger.Send(new MeetingUpdatedMessage(meeting));
            }
            else
            {
                writeOperation = TableOperation.Insert(meeting.ToMeetingEntity());
                table.Execute(writeOperation);

                _messenger.Send(new MeetingCreatedMessage(meeting));
            }

            return Task.CompletedTask;
        }

        public Task DeleteMeeting(Meeting meeting)
        {
            var table = GetTable();
            var retrieveOperation = TableOperation.Retrieve("meetings", meeting.Id.ToString());

            var existingMeeting = table.Execute(retrieveOperation);
            var entity = meeting.ToMeetingEntity();
            entity.ETag = existingMeeting.Etag;
            TableOperation.Delete(entity);
            _messenger.Send(new MeetingDeletedMessage(meeting));

            return Task.CompletedTask;
        }

        private CloudTable GetTable()
        {
            var table = _tableClient.GetTableReference("meetings");
            table.CreateIfNotExists();

            return table;
        }
    }
}
