using Microsoft.AspNetCore.Http;

namespace Victory.VCash.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetCurrentOperator(this HttpContext context)
        {
            object currentOperator;
            if (context.Items.TryGetValue("OperatorUserName", out currentOperator))
                return (string)currentOperator;

            return null;
        }

        public static Current Current(this HttpContext context)
        {
            var current = new Current();

            object operatorUserName;
            //if (context.Items.TryGetValue("OperatorUserName", out operatorUserName))
            //    current.Operator = new CurrentOperator()
            //    {
            //        UserName = (string)operatorUserName
            //    };

            return current;
        }
    }

    public class Current
    {
        public CurrentOperator Operator { get; init; }
        public CurrentAgent Agent { get; init; }
        public CurrentAdminOperator AdminOperator { get; init; }
    }

    public class CurrentOperator
    {
        public string UserName { get; init; }
    }

    public class CurrentAgent
    {
        public int UserId { get; init; }
        public string UserName { get; init; }
        public string Name { get; init; }
    }

    public class CurrentAdminOperator
    {
        public string UserName { get; init; }
    }
}
