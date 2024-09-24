using NPCarManagerApp.Models;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

List<string> MenuItems =
[
	"Get all cars",
	"Add car",
	"Edit car",
	"Delete car",
	"Exit"
];


var ip = IPAddress.Loopback;
var port = 27001;
var client = new TcpClient();
client.Connect(ip, port);

var stream = client.GetStream();
var br = new BinaryReader(stream);
var bw = new BinaryWriter(stream);

HttpCommand command;
string response = null;

int count = 0;
bool cont = true;

for (var i = 0; i < MenuItems.Count; i++)
{
	if (i == count)
		Console.ForegroundColor = ConsoleColor.Red;
	else
		Console.ForegroundColor = ConsoleColor.White;

	Console.WriteLine("\n\t\t"+MenuItems[i]);
}
ConsoleKeyInfo key;
while (cont)
{
	key = Console.ReadKey();
	Console.Clear();
	switch (key.Key)
	{
		case ConsoleKey.UpArrow:
			count--;
			break;
		case ConsoleKey.DownArrow:
			count++;
			break;
		case ConsoleKey.Enter:

			MainProcess(MenuItems[count]);

			break;
	}
	if (count < 0) count = 4;
	for (var i = 0; i < MenuItems.Count; i++)
	{
		if (i == count % MenuItems.Count)
			Console.ForegroundColor = ConsoleColor.Red;
		else
			Console.ForegroundColor = ConsoleColor.White;

		Console.WriteLine("\n\t\t"+MenuItems[i]);
	}
}


void MainProcess(string opt)
{
	Console.Clear();

	switch (opt)
	{
		case "Get all cars":
			command = new HttpCommand(HttpCommandMethods.GET);
			bw.Write(JsonSerializer.Serialize(command));
			bw.Flush();

			response = br.ReadString();
			var lst = JsonSerializer.Deserialize<List<Car>>(response);

			Console.WriteLine("\n\n");

			foreach (var car in lst)
				Console.WriteLine($"\n\n\t\t{car.ToString()}");

			Console.WriteLine("\n\n");
			break;
		case "Add car":

            Console.WriteLine("\nEnter Car Vendor: ");
			string vendor = Console.ReadLine();
            Console.WriteLine("\nEnter Car Model: ");
            string model = Console.ReadLine();
            Console.WriteLine("\nEnter Car Owner Name: ");
			string owner = Console.ReadLine();

			int year = 2024;

            Console.WriteLine("\nEnter Car Year: ");
			int.TryParse(Console.ReadLine(), out year);

			Car newCar = new Car(model,vendor,year,owner);

			command = new HttpCommand(HttpCommandMethods.POST, newCar);

			bw.Write(JsonSerializer.Serialize(command));
			bw.Flush();

			response = br.ReadString();
			Console.WriteLine($"\n\t\t{response}");
			break;
		case "Edit car":
			Console.WriteLine("\nEnter Car Id for Editing: ");
			int.TryParse(Console.ReadLine(), out int EditId);

			Console.WriteLine("\nEnter New Car Vendor: ");
			string newVendor = Console.ReadLine();
			Console.WriteLine("\nEnter New Car Model: ");
			string newModel = Console.ReadLine();
			Console.WriteLine("\nEnter New Car Owner Name: ");
			string newOwner = Console.ReadLine();

			int newYear = 2024;

			Console.WriteLine("\nEnter New Car Year: ");
			int.TryParse(Console.ReadLine(), out year);

			Car editedCar = new Car(newModel, newVendor, newYear, newOwner);
			editedCar.Id = EditId;

			command = new HttpCommand(HttpCommandMethods.PUT, editedCar);
			bw.Write(JsonSerializer.Serialize(command));
			bw.Flush();

			response = br.ReadString();
			Console.WriteLine($"\n\t\t{response}");
			break;
		case "Delete car":
			Console.WriteLine("\nEnter Car Id for Deleting: ");
			int.TryParse(Console.ReadLine(), out int id);

			Car del = new Car();
			del.Id = id;

			command = new HttpCommand(HttpCommandMethods.DELETE, del);
			bw.Write(JsonSerializer.Serialize(command));
			bw.Flush();

			response = br.ReadString();
			Console.WriteLine($"\n\t\t{response}");
			break;
		case "Exit":
			cont = false;

			br.Close();
			bw.Close();
			stream.Close();
			client.Close();
			break;
		default:
			break;
	}
}
