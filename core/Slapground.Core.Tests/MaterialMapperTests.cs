using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    public class MaterialMapperTests
    {
        [Fact]
        public void Bounciness_MatchesElasticityDirectly()
        {
            var props = new PhysicalProperties(mass: 1f, fragility: 0f, elasticity: 0.73f, articulation: Articulation.Rigid);
            var mat = MaterialMapper.ToPhysicsMaterialParams(props);
            Assert.Equal(0.73f, mat.Bounciness);
        }

        [Fact]
        public void Flail_HasLowerFrictionThanRigid()
        {
            var rigid = new PhysicalProperties(mass: 1f, fragility: 0f, elasticity: 0.5f, articulation: Articulation.Rigid);
            var flail = new PhysicalProperties(mass: 1f, fragility: 0f, elasticity: 0.5f, articulation: Articulation.Flail);

            var rigidMat = MaterialMapper.ToPhysicsMaterialParams(rigid);
            var flailMat = MaterialMapper.ToPhysicsMaterialParams(flail);

            Assert.True(flailMat.DynamicFriction < rigidMat.DynamicFriction);
            Assert.True(flailMat.StaticFriction < rigidMat.StaticFriction);
        }

        [Fact]
        public void Hinged_UsesSameFrictionAsRigid()
        {
            var rigid = new PhysicalProperties(mass: 1f, fragility: 0f, elasticity: 0.5f, articulation: Articulation.Rigid);
            var hinged = new PhysicalProperties(mass: 1f, fragility: 0f, elasticity: 0.5f, articulation: Articulation.Hinged);

            var rigidMat = MaterialMapper.ToPhysicsMaterialParams(rigid);
            var hingedMat = MaterialMapper.ToPhysicsMaterialParams(hinged);

            Assert.Equal(rigidMat.DynamicFriction, hingedMat.DynamicFriction);
        }
    }
}
