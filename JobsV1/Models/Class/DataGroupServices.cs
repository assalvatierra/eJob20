using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public class DataGroupServices
    {

        private JobDBContainer db = new JobDBContainer();

        public List<string> GetMembersFromTeamId(int teamId)
        {

            List<string> memberLists = new List<string>();

            if (teamId == 1)
            {
                teamId =1;
            }

            var TeamGroup = db.DataGroups.Find(teamId);

            if (TeamGroup != null)
            {
                memberLists = TeamGroup.DataGroupAssigns.Select(g => g.User).ToList();
            }


            return memberLists;
        }

        public int GetUserTeamGroup(string user)
        {

            if (string.IsNullOrEmpty(user))
            {
                return 1; //default;
            }

            var teamGroup = db.DataGroupAssigns.Where(g=>g.User == user).First();

            return teamGroup.DataGroupId;
        }

        public string GetUserTeamGroupName(string user)
        {

            if (string.IsNullOrEmpty(user))
            {
                return "No Group"; //default;
            }

            if (db.DataGroupAssigns.Where(g => g.User == user).Count() == 0)
            {
                return "No Group"; //default;
            }

            var teamGroup = db.DataGroupAssigns.Where(g => g.User == user).First();

            return teamGroup.DataGroup.Name;
        }

    }
}