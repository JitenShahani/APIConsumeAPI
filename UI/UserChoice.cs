namespace UI;

public class UserChoice : IChoice
{
	public string GetChoice()
	{
		string country;

		do
		{
			Write("Country > ");
			string input = ReadLine()!.ToString();

			if (!string.IsNullOrEmpty(input))
			{
				country = input;
				break;
			}
			else
			{
				WriteLine("Invalid input!");
			}
		} while (true);

		return country;
	}
}