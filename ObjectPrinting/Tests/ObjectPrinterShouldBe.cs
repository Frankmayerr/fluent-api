using System;
using System.Globalization;
using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
	[TestFixture]
	public class ObjectPrinterShouldBe
	{
		private Person person;

		[SetUp]
		public void SetUp()
		{
			person = new Person { Name = "Alex", Age = 19, Height = 174.6 };
		}

		[Test]
		public void ExcludeType_CorrectResult()
		{
			var expected = string.Join(Environment.NewLine,
				"Person",
				"\tId = Guid",
				$"\tHeight = {person.Height}",
				$"\tAge = {person.Age}")
				+ Environment.NewLine;
			var actual = ObjectPrinter.For<Person>()
				.ExcludeType<string>()
				.PrintToString(person);

			actual.Should().Be(expected);
		}

		[Test]
		public void MakeSpecialTypeSerialization_ChangeAge()
		{
			var expected = string.Join(Environment.NewLine,
							  "Person",
							  "\tId = Guid",
							  $"\tName = {person.Name}",
							  $"\tHeight = {person.Height}",
							  $"\tAge = {person.Age + 10}")
						  + Environment.NewLine;
			var actual = ObjectPrinter.For<Person>()
				.Print<int>().Using(i => (i + 10).ToString())
				.PrintToString(person);
			actual.Should().Be(expected);
		}

		[Test]
		public void MakeSpecialCultureForDouble_RuCulture()
		{
			var expected = string.Join(Environment.NewLine,
							  "Person",
							  "\tId = Guid",
							  $"\tName = {person.Name}",
							  $"\tHeight = {person.Height.ToString(CultureInfo.GetCultureInfo("ru"))}",
							  $"\tAge = {person.Age}")
						  + Environment.NewLine;
			var actual = ObjectPrinter.For<Person>()
				.Print<double>().UsingCulture(CultureInfo.GetCultureInfo("ru"))
				.PrintToString(person);
			actual.Should().Be(expected);

		}

		[Test]
		public void MakeSpecialPropertySerialization_ChangeHeight()
		{
			var expected = string.Join(Environment.NewLine,
							  "Person",
							  "\tId = Guid",
							  $"\tName = {person.Name}",
							  $"\tHeight = MyOwnHeight",
							  $"\tAge = {person.Age}")
						  + Environment.NewLine;
			var actual = ObjectPrinter.For<Person>()
				.Print(p => p.Height).Using(height => "MyOwnHeight")
				.PrintToString(person);
			actual.Should().Be(expected);
		}

		[Test]
		public void StringTrimming_ShorterName()
		{
			var expected = string.Join(Environment.NewLine,
							  "Person",
							  "\tId = Guid",
							  $"\tName = {person.Name.Substring(0, 3)}",
							  $"\tHeight = {person.Height}",
							  $"\tAge = {person.Age}")
						  + Environment.NewLine;
			var actual = ObjectPrinter.For<Person>()
				.Print(p => p.Name).Trim(3)
				.PrintToString(person);
			actual.Should().Be(expected);
		}

		[Test]
		public void ExcludePropertyHeight_ResultWithoutHeight()
		{
			var expected = string.Join(Environment.NewLine,
							  "Person",
							  "\tId = Guid",
							  $"\tName = {person.Name}",
							  $"\tAge = {person.Age}")
						  + Environment.NewLine;
			var actual = ObjectPrinter.For<Person>()
				.ExcludeProperty(p => p.Height)
				.PrintToString(person);
			actual.Should().Be(expected);
		}

		[Test]
		public void ObjectDefaultSerialixationWithConfiguration_ExcludePersonName()
		{
			var expected = string.Join(Environment.NewLine,
				               "Person",
				               "\tId = Guid",
				               $"\tHeight = {person.Height}",
				               $"\tAge = {person.Age}")
			               + Environment.NewLine;
			var actual = person.PrintToString(
				o => o
					.ExcludeProperty(p => p.Name)
			);
			actual.Should().Be(expected);
		}
	}
}