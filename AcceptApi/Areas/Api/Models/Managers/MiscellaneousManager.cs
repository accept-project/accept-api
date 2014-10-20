using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcceptFramework.Interfaces;
using AcceptFramework.Interfaces.Miscellaneous;
using AcceptFramework.Business;
using AcceptApi.Areas.Api.Models.Core;
using AcceptFramework.Domain.Miscellaneous;
using AcceptApi.Areas.Api.Models.Interfaces;

namespace AcceptApi.Areas.Api.Models.Managers
{
    public class MiscellaneousManager : IMiscellaneousManager
    {
        private IAcceptApiServiceLocator _acceptServiceLocator;
        private IMiscellaneousManagerService _miscellaneousManagerService;
        
        public MiscellaneousManager()
        {
            _acceptServiceLocator = new AcceptApiServiceLocator();
            _miscellaneousManagerService = _acceptServiceLocator.GetMiscellaneousManagerService();
        }

        public IMiscellaneousManagerService MiscellaneousManagerService
        {
            get { return _miscellaneousManagerService; }
        }

        public CoreApiResponse CreateExternalUsageItem(string externalUsageId, int usageCount, string metadata)
        {
            try
            {
                ExternalUsage externalUsageItem = new ExternalUsage();
                externalUsageItem.InstanceIdentifier = externalUsageId;
                externalUsageItem.LastUpdate = DateTime.UtcNow;
                externalUsageItem.UsageCount = usageCount;
                externalUsageItem.Metadata = metadata;
                MiscellaneousManagerService.CreateExternalUsage(externalUsageItem);
                return new CoreApiResponse();
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "CreateExternalUsageItem");
            }
        
        }

        public CoreApiResponse GetExternalUsageCount(string externalUsageId)
        {
            try
            {
               List<ExternalUsage> externalUsages = MiscellaneousManagerService.GetAllExternalUsageItemByUsageExternalUsageIdentifier(externalUsageId);
               return new CoreApiCustomResponse(externalUsages.Count);

            }
            catch (Exception e)
            {

                return new CoreApiException(e.Message, "CreateExternalUsageItem");
            }
        
        }

        public CoreApiResponse UpdateExternalUsageItem(string externalUsageId, int usageCount, string metadata)
        {
            try
            {
                ExternalUsage externalUsageItem = null;
                try
                {
                     externalUsageItem = MiscellaneousManagerService.GetExternalUsageItem(externalUsageId);
                }
                catch (Exception e)
                {
                    return new CoreApiException(e.Message, "UpdateExternalUsageItem - Get");
                }                
                if (externalUsageId == null)
                {
                    externalUsageItem = MiscellaneousManagerService.CreateExternalUsage(externalUsageId, usageCount, DateTime.UtcNow,metadata);                    
                }
                else
                {
                    try
                    {
                        externalUsageItem.UsageCount = externalUsageItem.UsageCount + usageCount;
                        externalUsageItem.LastUpdate = DateTime.UtcNow;
                        externalUsageItem.Metadata = metadata;
                        MiscellaneousManagerService.UpdateExternalUsageCount(externalUsageItem);
                    }
                    catch (Exception e)
                    {
                        return new CoreApiException(e.Message, "UpdateExternalUsageItem - Populating Object");                    
                    }                   
                }
                return new CoreApiResponse();
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "UpdateExternalUsageItem");
            }

        }

        #region Reports

        public CoreApiResponse GetExternalUsageReport(string externalUsageId, DateTime start, DateTime end, string format)
        {
            try
            {
                switch (format)
                {
                    case "excel": 
                    {                                                                                                     
                    } break;
                    default: 
                    {
                        List<ExternalUsage> externalUsages = MiscellaneousManagerService.GetAllExternalUsageItemByUsageExternalUsageIdentifierRange(externalUsageId, start, end);
                        return new CoreApiCustomResponse(externalUsages);                    
                    }                 
                }

                throw new Exception();                
            }
            catch (Exception e)
            {
                return new CoreApiException(e.Message, "GetExternalUsageReport");
                throw;
            }
        
        
        }


        #endregion

    }
}