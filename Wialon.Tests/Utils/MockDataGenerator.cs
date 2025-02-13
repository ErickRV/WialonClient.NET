using AutoBogus;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wialon.RemoteClient.Models.Units;

namespace Wialon.Tests.Utils
{
    public class MockDataGenerator
    {
        private static Faker faker = new Faker();

        public static Unit_F1 GenUnit_F1() {
            Faker<Unit_F1> unit = new AutoFaker<Unit_F1>()
                .RuleFor(u => u.nm, faker.Random.AlphaNumeric(6))
                .RuleFor(u => u.id, faker.Random.Int(1))
                .RuleFor(u => u.mu, faker.Random.Int(0, 3));

            return unit.Generate();
        }

        public static Unit_F4194304 GenUnit_F4194304()
        {
            Faker<Unit_F4194304> unit = new AutoFaker<Unit_F4194304>();
            return unit.Generate();
        }
    }
}
