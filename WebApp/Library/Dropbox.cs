using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dropbox.Api;
using Dropbox.Api.Team;
using System.Threading.Tasks;
using Dropbox.Api.Files;

namespace WebApp.Library
{
    public class OrvosiDropbox
    {
        private DropboxClient _Client;

        public DropboxTeamClient TeamClient { get; set; }
        public OrvosiDropbox()
        {
            TeamClient = new DropboxTeamClient("a1M0poGsG4AAAAAAAAAIsktLhZOWwR3RAtc-gyZK6oqF0uygRYjhcMgRy_YY65J7");
        }

        private DropboxClient _client;

        public async Task<DropboxClient> GetServiceAccountClientAsync()
        {
            if (_client == null)
            {
                List<UserSelectorArg> args = new List<UserSelectorArg>();
                args.Add(new UserSelectorArg.Email("lfarago@orvosi.ca"));

                var members = await TeamClient.Team.MembersGetInfoAsync(args);
                var teamMemberId = members.First().AsMemberInfo.Value.Profile.TeamMemberId;
                _client = TeamClient.AsMember(teamMemberId);
                return _client;
            }
            return _client;
        }

        internal async Task<DropboxClient> GetTeamMemberClientAsync(string email)
        {
            List<UserSelectorArg> args = new List<UserSelectorArg>();
            args.Add(new UserSelectorArg.Email(email));

            var members = await TeamClient.Team.MembersGetInfoAsync(args);
            var teamMemberId = members.First().AsMemberInfo.Value.Profile.TeamMemberId;
            return TeamClient.AsMember(teamMemberId);
        }
    }
}