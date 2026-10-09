using System;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    public class PhysicalPropertiesTests
    {
        [Fact]
        public void Constructor_AcceptsValidValues()
        {
            var props = new PhysicalProperties(mass: 0.5f, fragility: 0.3f, elasticity: 0.8f, articulation: Articulation.Rigid);
            Assert.Equal(0.5f, props.Mass);
            Assert.Equal(0.3f, props.Fragility);
            Assert.Equal(0.8f, props.Elasticity);
            Assert.Equal(Articulation.Rigid, props.Articulation);
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Constructor_RejectsNonPositiveMass(float mass)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new PhysicalProperties(mass, fragility: 0f, elasticity: 0f, articulation: Articulation.Rigid));
        }

        [Theory]
        [InlineData(-0.01f)]
        [InlineData(1.01f)]
        public void Constructor_RejectsOutOfRangeFragility(float fragility)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new PhysicalProperties(mass: 1f, fragility, elasticity: 0f, articulation: Articulation.Rigid));
        }

        [Theory]
        [InlineData(-0.01f)]
        [InlineData(1.01f)]
        public void Constructor_RejectsOutOfRangeElasticity(float elasticity)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new PhysicalProperties(mass: 1f, fragility: 0f, elasticity, articulation: Articulation.Rigid));
        }
    }
}
