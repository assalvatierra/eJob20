using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Areas.Personel.Models
{
    public class PersonnelList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
      
    }

    public class PersonnelClass
    {

        private HrisDBContainer db = new HrisDBContainer();
    
        public List<PersonnelList> getPersonnelList()
        {
            List<PersonnelList> personnels = new List<PersonnelList>();
            var personnelList = db.HrPersonels.ToList();
            
            foreach (var person in personnelList)
            {
                personnels.Add(new PersonnelList {
                    Id = person.Id,
                    Name = person.Name,
                    Position = getPositionbyId(person.Id),
                    Status = person.HrPersonelStatu.Desc
                });
            }

            return personnels;
        }

        public string getPositionbyId(int personId)
        {
          if(db.HrPerPositions.Where(s => s.HrPersonelId == personId)
                .OrderByDescending(s => s.Id).FirstOrDefault() != null)
            {
                return db.HrPerPositions.Where(s => s.HrPersonelId == personId)
                      .OrderByDescending(s => s.Id).FirstOrDefault().HrPosition.Desc;
            }
            else
            {
                return "NA";
            }

        }
    }
}