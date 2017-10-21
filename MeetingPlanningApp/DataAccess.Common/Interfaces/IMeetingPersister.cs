using DataAccess.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common.Interfaces
{
    public interface IMeetingPersister
    {
        Task SaveMeeting(Meeting meeting);
    }
}
