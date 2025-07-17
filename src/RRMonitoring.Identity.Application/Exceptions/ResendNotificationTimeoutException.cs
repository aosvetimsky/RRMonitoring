using Nomium.Core.Exceptions;

namespace RRMonitoring.Identity.Application.Exceptions;

public class ResendNotificationTimeoutException : ValidationException
{
	private readonly int _secondsTillNextNotification;

	public ResendNotificationTimeoutException(string message, int secondsTillNextNotification)
		: base(message)
	{
		_secondsTillNextNotification = secondsTillNextNotification;
	}

	public override object ToResponse()
	{
		return new
		{
			Messages,
			Errors,
			LocalizationDetails,
			SecondsTillNextNotification = _secondsTillNextNotification
		};
	}
}
