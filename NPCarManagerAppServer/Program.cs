using NPCarManagerApp.Models;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

List<Car> Cars = [
new Car("320","BMW",2019,"Germany"),
new Car("430","BMW",2021,"Italy"),
new Car("540","BMW",2017,"France"),
new Car("750","BMW",2023,"Spain")];


var ip = IPAddress.Loopback;
var port = 27001;
var listener = new TcpListener(ip, port);
listener.Start();

Console.WriteLine("Server is listening...");

while (true)
{
	var client = listener.AcceptTcpClient();
	var stream = client.GetStream();
	var br = new BinaryReader(stream);
	var bw = new BinaryWriter(stream);

	try
	{
		while (true)
		{
			var input = br.ReadString();
			var command = JsonSerializer.Deserialize<HttpCommand>(input);

			Console.WriteLine(command.Method);

			switch (command.Method)
			{
				case HttpCommandMethods.GET:
					bw.Write(JsonSerializer.Serialize(Cars));
					break;
				case HttpCommandMethods.POST:
					string msgPost = "POST Result : null";
					try
					{
						Cars.Add(command.Value);
						msgPost = "Car adding sucess.";
					}
					catch (Exception)
					{
						msgPost = "Car adding failed.";
					}
					finally
					{
						bw.Write($"POST: {msgPost}");
					}
					break;
				case HttpCommandMethods.PUT:
					string msgPut = "PUT Result : null";
					try
					{
						var c = Cars.FirstOrDefault(c => c.Id == command.Value.Id);

						c.Vendor = command.Value.Vendor;
						c.Model = command.Value.Model;
						c.ReleaseYear = command.Value.ReleaseYear;
						c.Owner = command.Value.Owner;

						msgPut = "Car updating sucess.";
					}
					catch (Exception)
					{
						msgPut = "Car updating failed.";
					}
					finally
					{
						bw.Write($"PUT: {msgPut}");
					}

					break;
				case HttpCommandMethods.DELETE:
					string msgDel = "DELETE Result : null";
					try
					{
						Cars.RemoveAll(c => c.Id == command.Value.Id);
						msgDel = "Car deleting sucess.";
					}
					catch (Exception)
					{
						msgDel = "Car deleting failed.";
					}
					finally
					{
						bw.Write($"DELETE: {msgDel}");
					}

					break;
				default: break;
			}

			bw.Flush();
		}
	}
	catch (IOException)
	{
		Console.WriteLine("\tClient disconnected.\n");
	}
	finally
	{
		client.Close();
	}
}
