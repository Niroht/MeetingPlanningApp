using DataAccess.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Common.Model;
using GalaSoft.MvvmLight.Messaging;
using DataAccess.Messages;

namespace DataAccess
{
    public class MeetingPersister : IMeetingPersister
    {
        private readonly IMessenger _messenger;
        private readonly IMeetingStore _meetingStore;

        public Task SaveMeeting(Meeting meeting)
        {
            var existingMeeting = _meetingStore.GetMeetings().FirstOrDefault(x => x.Id == meeting.Id);

            if (existingMeeting != null)
            {
                _meetingStore.UpdateMeeting(meeting);
                _messenger.Send(new MeetingUpdatedMessage(meeting));
            }
            else
            {
                _meetingStore.AddNewMeeting(meeting);
                _messenger.Send(new MeetingCreatedMessage(meeting));
            }

            return Task.CompletedTask;
        }

        public Task DeleteMeeting(Meeting meeting)
        {
            _meetingStore.DeleteMeeting(meeting.Id);
            _messenger.Send(new MeetingDeletedMessage(meeting));

            return Task.CompletedTask;
        }

        public MeetingPersister(IMeetingStore meetingStore, IMessenger messenger)
        {
            _meetingStore = meetingStore;
            _messenger = messenger;
        }
    }
}
