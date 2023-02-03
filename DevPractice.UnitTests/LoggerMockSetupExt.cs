using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace DevPractice.UnitTests
{
    [ExcludeFromCodeCoverage]
    public static class LoggerMockSetupExt
    {
        public static void SetupMock<T>(this Mock<ILogger<T>> logger, List<string> messages)
        {
            logger.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation => {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var state = invocation.Arguments[2];
                    var exception = (Exception)invocation.Arguments[3];
                    var formatter = invocation.Arguments[4];

                    var invokeMethod = formatter.GetType().GetMethod("Invoke");
                    var logMessage = (string)invokeMethod?.Invoke(formatter, new[] { state, exception });

                    messages.Add(logMessage);
                }));
        }
    }
}
