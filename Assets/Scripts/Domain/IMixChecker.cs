using System;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain
{
    public interface IMixChecker
    {
        Task<CheckResult> Check(Guid firstElementId, Guid secondElementId);
    }
}