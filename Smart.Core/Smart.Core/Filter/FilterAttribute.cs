using Microsoft.AspNetCore.Mvc.Filters;
using System;


namespace Smart.Core.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class FilterAttribute : Attribute
    {
        public string Policy { get; set; }
        public virtual bool Executing(ActionExecutingContext context)
        {
            return true;
        }

        public virtual void Executed(ActionExecutingContext context)
        {

        }
    }
}
