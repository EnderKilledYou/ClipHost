 
using ClipHost.ServiceModel.ListDtoProgramInstanceModels;
using ServiceStack;
using ServiceStack.FluentValidation;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClipHost.ServiceInterface.ListDtoProgramInstanceService
{
    public class ListDtoProgramInstance : Service
    {
        private readonly StreamerProcessWrangler _clipProcessWrangler;

        public ListDtoProgramInstance(StreamerProcessWrangler clipProcessWrangler)
        {
            _clipProcessWrangler = clipProcessWrangler;
        }


        public async Task<ListDtoProgramInstanceResponse> Get(ListDtoProgramInstanceRequest request)
        {
            try
            {
                 

                return
                new ListDtoProgramInstanceResponse()
                {
                    DtoProgramInstances = _clipProcessWrangler.Instances,
                    Success = true,
                    Message = ""
                }
                ;
            }
            catch (Exception e)
            {
                return new ListDtoProgramInstanceResponse()
                {
                    Success = false
,
                    Message = e.Message
                }
;
            }
            finally
            {
            }
        }
    }
}