namespace NPCarManagerApp.Models;

public class HttpCommand
{
	public HttpCommandMethods Method { get; set; }
	public Car? Value { get; set; }

	public HttpCommand() { }

	public HttpCommand(HttpCommandMethods method)
	{
		Method=method;
		Value=null;
	}
	public HttpCommand(HttpCommandMethods method, Car value)
	{
		Method=method;
		Value=value;
	}
	public override string? ToString() => Value?.ToString();
}

public class Car
{
	public int Id { get; set; }
	public string Model { get; set; }
	public string Vendor { get; set; }
	public int ReleaseYear { get; set; }
	public string Owner { get; set; }

	public Car()
	{
		Id = IdCounter.GetNewId();
	}

	public Car(string model, string vendor, int releaseYear, string owner)
		: this()
	{
		Model=model;
		Vendor=vendor;
		ReleaseYear=releaseYear;
		Owner=owner;
	}

	public override string ToString() =>
		$" Id: {Id}. The {Vendor} {Model} {ReleaseYear} owned by {Owner}.";
}
public static class IdCounter
{
	private static int _counter = 1;
	public static int GetNewId() => _counter++;
}

public enum HttpCommandMethods
{
	/// <summary>
	/// GET Value From Server.
	/// </summary>
	GET,

	/// <summary>
	/// Add New Value To Server.
	/// </summary>
	POST,

	/// <summary>
	/// Update Value In Server.
	/// </summary>
	PUT, 

	/// <summary>
	/// DELETE Value From Server.
	/// </summary>
	DELETE
}