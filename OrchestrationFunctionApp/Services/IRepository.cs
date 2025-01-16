using OrchestrationFunctionApp.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Services
{
    public interface IRepository
    {
        Task<int> SaveEmptyEvent(MsgEmptyEvent entity);
        Task<int> SaveInlineJsonEvent(MsgInlineJson entity);
        Task<int> SaveJsonFileEvent(MsgJsonFile entity);
        Task<int> SaveFlatFileEvent(MsgFlatFile entity);
    }
}


