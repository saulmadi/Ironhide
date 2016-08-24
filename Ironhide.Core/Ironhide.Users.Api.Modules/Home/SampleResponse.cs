using System;

namespace Ironhide.Api.Modules.Home
{
    public class SampleResponse
    {
        protected SampleResponse()
        {
        }

        public SampleResponse(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public virtual Guid Id { get; protected set; }
        public virtual string Name { get; set; }
    }
}