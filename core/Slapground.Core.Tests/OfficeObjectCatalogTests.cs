using System;
using Slapground.Core;
using Xunit;

namespace Slapground.Core.Tests
{
    public class OfficeObjectCatalogTests
    {
        [Theory]
        [InlineData(OfficeObjectKind.AlarmClock)]
        [InlineData(OfficeObjectKind.Printer)]
        [InlineData(OfficeObjectKind.EmailNotification)]
        [InlineData(OfficeObjectKind.CordedPhone)]
        public void EveryKind_HasACatalogEntry(OfficeObjectKind kind)
        {
            // PhysicalProperties' own constructor validates ranges - reaching this
            // line without throwing is the real assertion.
            var props = OfficeObjectCatalog.Get(kind);
            Assert.True(props.Mass > 0f);
        }

        [Fact]
        public void Printer_IsHeavierAndLessFragileThanAlarmClock()
        {
            var printer = OfficeObjectCatalog.Get(OfficeObjectKind.Printer);
            var clock = OfficeObjectCatalog.Get(OfficeObjectKind.AlarmClock);

            Assert.True(printer.Mass > clock.Mass);
            Assert.True(printer.Fragility < clock.Fragility);
        }

        [Fact]
        public void CordedPhone_IsTheOnlyFlail()
        {
            Assert.Equal(Articulation.Flail, OfficeObjectCatalog.Get(OfficeObjectKind.CordedPhone).Articulation);
            Assert.Equal(Articulation.Rigid, OfficeObjectCatalog.Get(OfficeObjectKind.AlarmClock).Articulation);
            Assert.Equal(Articulation.Rigid, OfficeObjectCatalog.Get(OfficeObjectKind.Printer).Articulation);
            Assert.Equal(Articulation.Rigid, OfficeObjectCatalog.Get(OfficeObjectKind.EmailNotification).Articulation);
        }

        [Fact]
        public void EmailNotification_IsTheLightestAndMostFragile()
        {
            var email = OfficeObjectCatalog.Get(OfficeObjectKind.EmailNotification);
            foreach (OfficeObjectKind kind in Enum.GetValues(typeof(OfficeObjectKind)))
            {
                if (kind == OfficeObjectKind.EmailNotification) continue;
                var other = OfficeObjectCatalog.Get(kind);
                Assert.True(email.Mass < other.Mass);
                Assert.True(email.Fragility > other.Fragility);
            }
        }

        [Fact]
        public void UndefinedKind_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => OfficeObjectCatalog.Get((OfficeObjectKind)999));
        }
    }
}
