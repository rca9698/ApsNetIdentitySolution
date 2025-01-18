using Microsoft.AspNetCore.Authorization;

namespace Rca.Client.Authorization
{
    public class HRManagerProbationRequirement : IAuthorizationRequirement
    {
        public readonly int _probitionMonth;
        public HRManagerProbationRequirement(int probitionMonth)
        {
            _probitionMonth = probitionMonth;
        }

    }
    public class ManagerProbationRequirementHandler : AuthorizationHandler<HRManagerProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
        {
            if(!context.User.HasClaim(x => x.Type == "EmployementDate"))
                return Task.CompletedTask;

            if(DateTime.TryParse(context.User.FindFirst(x => x.Type == "EmployementDate")?.Value, out DateTime employementDate)){
                var period = DateTime.Now - employementDate;
                if (period.Days > 30*requirement._probitionMonth) {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
