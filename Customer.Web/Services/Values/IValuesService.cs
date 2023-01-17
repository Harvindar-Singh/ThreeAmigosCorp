using System;

namespace Customer.Web.Services.Values
{
    public interface IValuesService
    {
        Task<ValuesGetDto> Get();
    }
}