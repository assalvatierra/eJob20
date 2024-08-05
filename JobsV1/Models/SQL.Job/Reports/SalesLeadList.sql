


 SELECT sl.*, slc.CustEntMainId FROM SalesLeads sl 
              LEFT JOIN SalesLeadCompanies slc ON sl.Id = slc.SalesLeadId 


-- SQL Query
--.Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCode.SeqNo > 2 && ss.SalesStatusStatusId == 1)
--                                .OrderByDescending(ss => ss.SalesStatusCode.SeqNo).FirstOrDefault().SalesStatusCode.SeqNo == 3)
--                                .ToList();
