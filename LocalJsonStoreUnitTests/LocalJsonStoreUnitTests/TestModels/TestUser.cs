using System;

namespace LocalJsonStoreUnitTests.LocalJsonStoreUnitTests.TestModels
{
    public class TestUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
