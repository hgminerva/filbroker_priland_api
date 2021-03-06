﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using priland_api.Models;

namespace priland_api.Controllers
{
    [Authorize]
    [RoutePrefix("api/SysSettings")]
    public class SysSettingsController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        // detail
        [HttpGet, Route("Detail")]
        public SysSettings GetSysSettings()
        {
            var SysSettingsData = from d in db.SysSettings
                                  select new SysSettings
                                  {
                                        Id = d.Id,
                                        Company = d.Company,
                                        SoftwareVersion = d.SoftwareVersion,
                                        SoftwareDeveloper = d.SoftwareDeveloper,
                                        SoldUnitCheckedBy = d.SoldUnitCheckedBy,
                                        SoldUnitApprovedBy = d.SoldUnitApprovedBy,
                                        CommissionRequestCheckedBy = d.CommissionRequestCheckedBy,
                                        CommissionRequestApprovedBy = d.CommissionRequestApprovedBy,
                                        ProposalFootNote = d.ProposalFootNote,
                                        BrokerFootNote = d.BrokerFootNote,
                                        TotalInvestment = d.TotalInvestment,
                                        PaymentOptions = d.PaymentOptions,
                                        Financing = d.Financing
                                  };

            return SysSettingsData.FirstOrDefault();
        }

        // save
        [HttpPut, Route("Save")]
        public HttpResponseMessage SaveSysSettings(SysSettings settings)
        {
            try
            {
                var SysSettingsData = from d in db.SysSettings where d.Id == Convert.ToInt32(settings.Id) select d;

                if (SysSettingsData.Any())
                {
                    var UpdateSysSettingsData = SysSettingsData.FirstOrDefault();

                    UpdateSysSettingsData.Company = settings.Company;
                    UpdateSysSettingsData.SoftwareVersion = settings.SoftwareVersion;
                    UpdateSysSettingsData.SoftwareDeveloper = settings.SoftwareDeveloper;
                    UpdateSysSettingsData.SoldUnitCheckedBy = settings.SoldUnitCheckedBy;
                    UpdateSysSettingsData.SoldUnitApprovedBy = settings.SoldUnitApprovedBy;
                    UpdateSysSettingsData.CommissionRequestCheckedBy = settings.CommissionRequestCheckedBy;
                    UpdateSysSettingsData.CommissionRequestApprovedBy = settings.CommissionRequestApprovedBy;
                    UpdateSysSettingsData.ProposalFootNote = settings.ProposalFootNote;
                    UpdateSysSettingsData.BrokerFootNote = settings.BrokerFootNote;
                    UpdateSysSettingsData.TotalInvestment = settings.TotalInvestment;
                    UpdateSysSettingsData.PaymentOptions = settings.PaymentOptions;
                    UpdateSysSettingsData.Financing = settings.Financing;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch 
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

    }
}
