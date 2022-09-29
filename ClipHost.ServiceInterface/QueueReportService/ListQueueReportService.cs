using ClipHost.ServiceModel.ListQueueReportModels;
using Microsoft.AspNetCore.Hosting;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using System.Linq;
using ServiceStack.OrmLite;
using ServiceStack.FluentValidation;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ClipHost.ServiceModel.ListQueueReportService
{


    public class ListQueueReport : Service
    {

        private StreamerProcessWrangler _clipProcessWrangler;

        public ListQueueReport(StreamerProcessWrangler clipProcessWrangler)
        {
            _clipProcessWrangler = clipProcessWrangler;
        }

        public async Task<ListQueueReportResponse> Post(ListQueueReportRequest request)
        {

            try
            {

                List<DToProgramInstanceReport> items = new();
                _clipProcessWrangler.Report(items);

                return
                new ListQueueReportResponse()
                {
                    QueueReports = items.SelectMany(a => a.QueueReports).ToList(),               
                    Success = true,
                    Message = ""
                }
                ;
            }
            catch (Exception e)
            {
                return new ListQueueReportResponse()
                {
                    Success = false,
                    Message = e.Message
                };
            }
            finally
            {

            }




        }

    }
}
