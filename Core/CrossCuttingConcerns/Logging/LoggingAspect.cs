using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;


namespace Core.CrossCuttingConcerns.Logging
{
    public class LoggingAspect : MethodInterception
    {
        private readonly ILoggerService _loggerService;

        public LoggingAspect(Type loggerType)
        {
            _loggerService = (ILoggerService)ServiceTool.ServiceProvider.GetService(loggerType);
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var logDetail = CreateLogDetail(invocation);
            _loggerService.Log(logDetail);
        }

        private LogDetail CreateLogDetail(IInvocation invocation) 
        {
            return new LogDetail
            {
                FullName = invocation.Method.DeclaringType.FullName,
                MethodName = invocation.Method.Name,
                Parameters = invocation.Method.GetParameters()
                .Select((p, i) => new LogParameter
                 {
                     Name = p.Name,
                     Value = invocation.Arguments[i],
                     Type = p.ParameterType.Name
                 }).ToList()
            };
        }

    }
}
