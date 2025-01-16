using Microsoft.Extensions.Logging;
using OrchestrationFunctionApp.Persistence;
using OrchestrationFunctionApp.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchestrationFunctionApp.Services
{
    public class Repository : IRepository
    {
        private readonly ILogger<Repository> _logger;
        private readonly GreeeKingMessageBusContext _dbContext;

        public Repository(ILogger<Repository> logger, GreeeKingMessageBusContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        public async Task<int> SaveEmptyEvent(MsgEmptyEvent entity)
        {
            await _dbContext.MsgEmptyEvents.AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> SaveFlatFileEvent(MsgFlatFile entity )
        {
            await _dbContext.MsgFlatFiles.AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> SaveInlineJsonEvent(MsgInlineJson entity)
        {
            await _dbContext.MsgInlineJsons.AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> SaveJsonFileEvent(MsgJsonFile entity)
        {
            await _dbContext.MsgJsonFiles.AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
