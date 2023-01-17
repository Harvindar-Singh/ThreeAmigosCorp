using System;

namespace Customer.Web.Services.Values
{
    public class FakeValuesService : IValuesService
    {
        public Task<ValuesGetDto> Get()
        {
            var num = new Random().Next();
            var result = new ValuesGetDto
            {
                Number = num,
                Message = $"Fake message of {num}"
            };
            return Task.FromResult(result);
        }
    }
}